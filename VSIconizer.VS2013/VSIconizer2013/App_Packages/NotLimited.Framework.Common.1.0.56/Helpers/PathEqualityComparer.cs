//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;

namespace NotLimited.Framework.Common.Helpers
{
    public class PathEqualityComparer : IEqualityComparer<string>
    {
        private static readonly PathEqualityComparer _instanse = new PathEqualityComparer();
        public static PathEqualityComparer Instance { get { return _instanse; } }

        public bool Equals(string x, string y)
        {
            return PathHelpers.PathEquals(x, y);
        }

        public int GetHashCode(string obj)
        {
            return PathHelpers.GetPathHashCode(obj);
        }
    }
}