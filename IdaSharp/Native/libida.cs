using System.Runtime.InteropServices;

namespace IdaPro.Native;

internal class Libida
{
    #region Platform-specific library names
#if MACOS
    private const string Ida = "libida.dylib";
#elif WINDOWS
    private const string Ida = "libida.dll";
#elif LINUX
    private const string Ida = "libida.so";
#else
    #error Unknown platform
#endif
    #endregion
    

    [DllImport(Ida, EntryPoint = "auto_wait", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool auto_wait();
    
    [DllImport(Ida, EntryPoint = "init_database", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int init_database(int argc, string[]? argv, out int newFile);
    
    /// <summary>
    /// Get named typeinfo.
    /// The returned pointers are pointers to static storage.
    /// They are valid until free_til(), set_named_type(), del_named_type(),
    /// rename_named_type(), set_numbered_type(), del_numbered_type(),
    /// and idb structure/enum manipulation (in other words, until ::til_t is changed).
    /// </summary>
    /// <param name="ti"> pointer to type information library</param>
    /// <param name="name">name of type</param>
    /// <param name="ntf_flags">combination of <see cref="Enums.NamedTypeFlags">NamedTypeFlags</see></param>
    /// <param name="type">ptr to ptr to output buffer for the type info</param>
    /// <param name="fields">ptr to ptr to the field/args names. may be nullptr</param>
    /// <param name="cmt">ptr to ptr to the main comment. may be nullptr. the comment may has TPOS_REGCMT as its first byte</param>
    /// <param name="fieldcmts">ptr to ptr to the field/args comments. may be nullptr</param>
    /// <param name="sclass">ptr to storage class</param>
    /// <param name="value">ptr to symbol value. for types, ptr to the ordinal number</param>
    /// <returns></returns>
    [DllImport(Ida, EntryPoint = "get_named_type", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int get_named_type(IntPtr ti, string name, int ntf_flags,
        ref IntPtr type,
        ref IntPtr fields, 
        ref IntPtr cmt,
        ref IntPtr fieldcmts,
        IntPtr sclass, 
        IntPtr value);
    
}