//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.IO;

namespace NotLimited.Framework.Common.Helpers
{
	public static class StreamExtensions
	{
		public static byte[] ReadBytes(this Stream stream, int count)
		{
			var result = new byte[count];
			int totalRead = 0;
			int read;

			while (totalRead < count)
			{
				if ((read = stream.Read(result, totalRead, count - totalRead)) == -1)
					throw new EndOfStreamException();
				totalRead += read;
			}

			return result;
		}

		public static void WriteBuff(this Stream stream, byte[] buff)
		{
			stream.Write(buff, 0, buff.Length);
		}

        /// <summary>
        /// Reads the given stream up to the end, returning the data as a byte
        ///             array, using the given buffer for transferring data. Note that the
        ///             current contents of the buffer is ignored, so the buffer needn't
        ///             be cleared beforehand.
        /// 
        /// </summary>
        public static byte[] ReadAllBytes(this Stream input)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            
            using (var stream = new MemoryStream())
            {
                input.CopyTo(stream);
                return stream.ToArray();
            }
        }
	}
}