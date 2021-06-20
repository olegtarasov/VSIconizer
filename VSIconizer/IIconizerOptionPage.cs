using System;

using VSIconizer.Core;

namespace VSIconizer
{
    public interface IIconizerOptionPage
    {
        void Apply(VSIconizerConfiguration newConfiguration);
    }
}