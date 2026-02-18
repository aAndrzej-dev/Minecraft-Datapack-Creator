using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftDatapackCreator.FileStructure
{
    internal static class DatapackFsHelpers
    {
        internal static int IndexOfSeparator(ReadOnlySpan<char> span)
        {
            int i1 = span.IndexOf('\\');
            int i2 = span.IndexOf('/');
            if (i1 == -1) return i2;
            if (i2 == -1) return i1;
            return i1 < i2 ? i1 : i2;
        }
        internal static int IndexOfSeparator(ReadOnlySpan<char> span, int start)
        {
            if ((uint)start >= (uint)span.Length) return -1;
            int i1 = span.Slice(start).IndexOf('\\');
            int i2 = span.Slice(start).IndexOf('/');
            if (i1 == -1) return i2 == -1 ? -1 : i2 + start;
            if (i2 == -1) return i1 + start;
            int pos1 = i1 + start;
            int pos2 = i2 + start;
            return pos1 < pos2 ? pos1 : pos2;
        }
    }
}
