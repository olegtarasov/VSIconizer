//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Threading;

namespace NotLimited.Framework.Common.Helpers
{
    public sealed class CountdownEventSlim : IDisposable
    {
        private int _count = 0;
        private ManualResetEventSlim _resetEvent = new ManualResetEventSlim(true);

        public void Increment()
        {
            if (_count == 0)
                _resetEvent.Reset();

            Interlocked.Increment(ref _count);
        }

        public void Decrement()
        {
            if (Interlocked.Decrement(ref _count) == 0)
                _resetEvent.Set();
        }

        public void Wait()
        {
            _resetEvent.Wait();
        }

        public void Dispose()
        {
            _resetEvent.Dispose();
        }
    }
}