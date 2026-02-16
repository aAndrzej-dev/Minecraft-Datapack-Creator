using System.Runtime.InteropServices;

namespace MinecraftDatapackCreator;
internal static partial class PInvoke
{
    [LibraryImport("user32.dll")]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static partial int GetScrollPos(IntPtr hWnd, Orientation nBar);

    [LibraryImport("user32.dll")]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static partial int SetScrollPos(IntPtr hWnd, Orientation nBar, int nPos, [MarshalAs(UnmanagedType.Bool)] bool bRedraw);

    [LibraryImport("user32.dll", EntryPoint = "SendMessageA")]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    public static partial int SendMessage(nint hwnd, int wMsg, nint wParam, ref Rect lParam);

    internal struct Rect
    {
        public int x; // Do not rename (binary serialization)
        public int y; // Do not rename (binary serialization)
        public int width; // Do not rename (binary serialization)
        public int height; // Do not rename (binary serialization)

        public Rect(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
    }
}
