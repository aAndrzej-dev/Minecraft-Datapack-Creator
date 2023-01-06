namespace MinecraftDatapackCreator;
internal static class Helpers
{
    public static string SetStringLenghtMiddle(this string value, int size)
    {
        if (value.Length <= size)
            return value;

        double count = (size - 3) * 0.5;

        int left = (int)Math.Floor(count);
        int right = (int)Math.Ceiling(count);

        return string.Concat(value.AsSpan(0, left), "...", value.AsSpan(value.Length - right, right));

    }
}
