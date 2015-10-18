//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NotLimited.Framework.Common.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MinCollectionCount : ValidationAttribute
    {
        private const string DefaultError = "'{0}' must have at least {1} elements.";

        public MinCollectionCount(int minCount) : base(DefaultError)
        {
            MinCount = minCount;
        }

        public int MinCount { get; set; }

        public override bool IsValid(object value)
        {
            var en = value as IEnumerable;

            if (en == null)
                return false;

            int cnt = 0;
            var enumerator = en.GetEnumerator();
            while (enumerator.MoveNext())
            {
                cnt++;
            }

            return cnt >= MinCount;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(ErrorMessageString, name, MinCount);
        }
    }
}