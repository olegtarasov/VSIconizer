//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Common NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace NotLimited.Framework.Common.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class NotEmptyAttribute : ValidationAttribute
    {
        private const string DefaultError = "'{0}' must have at least one element.";

        public NotEmptyAttribute() : base(DefaultError)
        {
        }

        public override bool IsValid(object value)
        {
            var en = value as IEnumerable;
            return (en != null && en.GetEnumerator().MoveNext());
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(ErrorMessageString, name);
        }
    }
}