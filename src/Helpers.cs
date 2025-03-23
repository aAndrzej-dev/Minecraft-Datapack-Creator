using System.Globalization;

namespace MinecraftDatapackCreator;
internal static class Helpers
{
    public static string SetStringLengthMiddle(this string value, int size)
    {
        if (value.Length <= size)
            return value;

        double count = (size - 3) * 0.5;

        int left = (int)Math.Floor(count);
        int right = (int)Math.Ceiling(count);

        return string.Concat(value.AsSpan(0, left), "...", value.AsSpan(value.Length - right, right));
    }
    public static bool HasFileExtension(ReadOnlySpan<char> filename, ReadOnlySpan<char> extension)
    {
        if (extension.Length == 0)
            return true;
        int extensionLength = extension.Length;

        if (filename.Length < extensionLength + 1)
            return false;

        if (filename[^(extensionLength + 1)] != '.')
            return false;

        int start = filename.Length - extensionLength;
        
        for (int i = 0; i < extensionLength; i++)
        {
            if (!char.ToLowerInvariant(filename[start + i]).Equals(char.ToLowerInvariant(extension[i])))
                return false;
        }
        return true;
    }

    public static int IndexOf(this ReadOnlySpan<char> str, char value, int startIndex)
    {
        int index = str[startIndex..].IndexOf(value);
        if (index < 0) return -1;
        return index + startIndex;
    }

    public static string ConvertToFriendlyName(string name)
    {
        return string.Create(name.Length, name, new System.Buffers.SpanAction<char, string>((span, n) =>
        {
            span[0] = char.ToUpper(n[0], CultureInfo.CurrentCulture);
            for (int i = 1; i < name.Length; i++)
            {
                if (name[i] is '_')
                {
                    span[i] = ' ';
                    if (name.Length <= i + 1)
                    {
                        continue;
                    }

                    i++;
                    span[i] = char.ToUpper(name[i], CultureInfo.CurrentCulture);
                    continue;
                }

                span[i] = name[i];
            }
        }));
    }
    public static TreeNode? GetNodeByKey(this TreeNodeCollection nodeCollection, ReadOnlySpan<char> key)
    {
        for (int i = 0; i < nodeCollection.Count; i++)
        {
            TreeNode item = nodeCollection[i];
            if (key.SequenceEqual(item.Name))
            {
                return item;
            }
        }
        return null;
    }
    public static TreeNode? GetNodeByKey(this TreeNode node, ReadOnlySpan<char> key) => node.Nodes.GetNodeByKey(key);
}
