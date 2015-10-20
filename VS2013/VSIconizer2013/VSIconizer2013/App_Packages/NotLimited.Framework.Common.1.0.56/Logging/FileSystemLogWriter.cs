//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System.IO;

namespace NotLimited.Framework.Common.Logging
{
    public class FileSystemLogWriter : ILogWriter
    {
        private readonly StreamWriter _writer;

        public FileSystemLogWriter(string path, bool append = true)
        {
            _writer = new StreamWriter(path, append);
        }

        public void WriteLine(string text)
        {
            _writer.WriteLine(text);
            _writer.Flush();
        }

        public async void Write(string text)
        {
            _writer.Write(text);
            _writer.Flush();
        }
    }
}