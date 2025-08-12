using System.Runtime.InteropServices;

namespace IdaPro.Native;

internal static class Libidalib
{
    #region Platform-specific library names
#if MACOS
    private const string Lib = "libidalib.dylib";
#elif WINDOWS
    private const string Lib = "libidalib.dll";
#elif LINUX
    private const string Lib = "libidalib.so";
#else
    #error Unknown platform
#endif
    #endregion
    
    
    [DllImport(Lib, EntryPoint = "init_library", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I4)]
    internal static extern int init_library(int argc, string[]? argv);
    
    [DllImport(Lib, EntryPoint = "open_database", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I4)]
    internal static extern int open_database([MarshalAs(UnmanagedType.LPStr)] string filename, 
        [MarshalAs(UnmanagedType.I1)] bool auto_run, 
        [MarshalAs(UnmanagedType.LPStr)] string? args);

    [DllImport(Lib, EntryPoint = "close_database", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void close_database([MarshalAs(UnmanagedType.I1)] bool save);

    [DllImport(Lib, EntryPoint = "enable_console_messages", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void enable_console_messages([MarshalAs(UnmanagedType.I1)] bool enable);
    
    
}
