//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
// ReSharper disable ImpureMethodCallOnReadonlyValueField

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace NotLimited.Framework.Common.Misc
{
	public class NamedPipeMessage
	{
		private readonly NamedPipeServerStream pipeServer;
		private readonly NamedPipeServer server;

		public NamedPipeMessage(NamedPipeServerStream pipeServer, NamedPipeServer server, byte[] message)
		{
			this.pipeServer = pipeServer;
			this.server = server;
			Message = message;
		}

		public byte[] Message { get; private set; }

		public void Respond(byte[] response)
		{
			server.Write(pipeServer, response);
		}
	}

	public class NamedPipeServer : IDisposable
	{
		private class ReadDto
		{
			public ReadDto(NamedPipeServerStream server, MemoryStream data, byte[] buffer)
			{
				Server = server;
				Data = data;
				Buffer = buffer;
			}

			public readonly NamedPipeServerStream Server;
			public readonly byte[] Buffer;
			public MemoryStream Data;
		}


		private readonly LinkedList<NamedPipeServerStream> servers = new LinkedList<NamedPipeServerStream>();
		private readonly SpinLock spinLock = new SpinLock(false);

		private NamedPipeServerStream listener;
		private int shouldListen;

		#region MessageReceived event

		public event Action<NamedPipeMessage> MessageReceived;

		protected void OnMessageReceived(NamedPipeMessage message)
		{
			if (MessageReceived != null)
				MessageReceived(message);
		}

		#endregion

		#region ClientConnected event

		public event Action<NamedPipeServer> ClientConnected;

		protected void OnClientConnected(NamedPipeServer arg)
		{
			if (ClientConnected != null)
				ClientConnected(arg);
		}

		#endregion

		#region ClientDisconnected event

		public event Action<NamedPipeServer> ClientDisconnected;

		protected void OnClientDisconnected(NamedPipeServer arg)
		{
			if (ClientDisconnected != null)
				ClientDisconnected(arg);
		}

		#endregion

		public string PipeName { get; private set; }

		public NamedPipeServer(string pipeName)
		{
			PipeName = pipeName;
		}

		public void StartListen()
		{
			if (servers.Count > 0)
				return;

			Interlocked.Exchange(ref shouldListen, 1);
			SpawnListener();
		}

		public void StopListen()
		{
			if (shouldListen == 0 || listener == null)
				return;

			Interlocked.Exchange(ref shouldListen, 0);

			listener.Dispose();
			listener = null;
		}

		public void StopServers()
		{
			foreach (var server in servers)
				server.Dispose();
			servers.Clear();
		}

		public void Write(byte[] message)
		{
			List<NamedPipeServerStream> dead = null;

			bool locked = false;
			spinLock.Enter(ref locked);
			foreach (var server in servers)
			{
				if (!server.CanWrite)
					continue;

				try
				{
					server.BeginWrite(message, 0, message.Length, EndWrite, server);
				}
				catch (Exception e)
				{
					if (e is ObjectDisposedException ||
						e is InvalidOperationException ||
						e is IOException)
					{
						// If the server is dead, we stash its corpse for further disposal
						if (dead == null)
							dead = new List<NamedPipeServerStream>();
						dead.Add(server);
					}
				}
			}
			spinLock.Exit();

			// Get rid of the corpses
			if (dead != null && dead.Count > 0)
				for (int i = 0; i < dead.Count; i++)
					RemoveServer(dead[i]);
		}

		internal void Write(NamedPipeServerStream server, byte[] message)
		{
			if (!server.CanWrite)
				return;

			try
			{
				server.BeginWrite(message, 0, message.Length, EndWrite, server);
			}
			catch (Exception e)
			{
				if (e is ObjectDisposedException ||
					e is InvalidOperationException ||
					e is IOException)
				{
					// The pipe is effectively dead at this point
					RemoveServer(server);
				}
			}
		}

		private void EndWrite(IAsyncResult result)
		{
			var server = (NamedPipeServerStream)result.AsyncState;

			try
			{
				server.EndWrite(result);
			}
			catch (Exception e)
			{
				if (e is ObjectDisposedException ||
					e is OperationCanceledException ||
					e is IOException)
				{
					// The pipe is effectively dead at this point
					RemoveServer(server);
				}
			}
		}

		private void EndWaitForConnection(IAsyncResult result)
		{
			var server = (NamedPipeServerStream)result.AsyncState;

			try
			{
				server.EndWaitForConnection(result);
			}
			catch (ObjectDisposedException)
			{
				return;
			}

			AddServer(server);

			if (server.CanRead)
				BeginRead(new ReadDto(server, null, new byte[1024]));

			if (shouldListen == 1)
				SpawnListener();

			OnClientConnected(this);
		}

		private void EndRead(IAsyncResult result)
		{
			var dto = (ReadDto)result.AsyncState;

			int read = 0;

			try
			{
				read = dto.Server.EndRead(result);
			}
			catch (IOException)
			{
			}

			if (read == 0)
			{
				RemoveServer(dto.Server);
				return;
			}

			// If we got the whole message in one read, don't fuck around
			if (dto.Server.IsMessageComplete && dto.Data == null)
			{
				var msg = new byte[read];

				Array.Copy(dto.Buffer, msg, read);
				RaiseMessageRecieved(dto, msg);
				return;
			}

			if (dto.Data == null)
				dto.Data = new MemoryStream();

			dto.Data.Write(dto.Buffer, 0, read);

			if (dto.Server.IsMessageComplete)
				RaiseMessageRecieved(dto, dto.Data.ToArray());
			else
				BeginRead(dto);
		}

		private void AddServer(NamedPipeServerStream server)
		{
			bool locked = false;
			spinLock.Enter(ref locked);
			servers.AddLast(server);
			spinLock.Exit();
		}

		private void RemoveServer(NamedPipeServerStream server)
		{
			bool removed, locked = false;

			spinLock.Enter(ref locked);
			removed = servers.Remove(server);
			spinLock.Exit();

			if (removed)
				OnClientDisconnected(this);
		}

		private void RaiseMessageRecieved(ReadDto dto, byte[] msg)
		{
			dto.Data = null;
			BeginRead(dto);
			OnMessageReceived(new NamedPipeMessage(dto.Server, this, msg));
		}

		private void BeginRead(ReadDto dto)
		{
			dto.Server.BeginRead(dto.Buffer, 0, dto.Buffer.Length, EndRead, dto);
		}

		private void SpawnListener()
		{
			listener = GetServer();
			listener.BeginWaitForConnection(EndWaitForConnection, listener);
		}

		private NamedPipeServerStream GetServer()
		{
			return new NamedPipeServerStream(PipeName, PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances, PipeTransmissionMode.Message, PipeOptions.Asynchronous | PipeOptions.WriteThrough);
		}

		public void Dispose()
		{
			StopListen();
			StopServers();
		}
	}
}
// ReSharper restore ImpureMethodCallOnReadonlyValueField