namespace IdaPro.Enums;

[Flags]
public enum NamedTypeFlags : ushort
{
    /// <summary>
    /// type name
    /// </summary>
    NTF_TYPE = 0x0001,
    /// <summary>
    /// symbol, name is unmangled ('func')
    /// </summary>
    NTF_SYMU = 0x0008, 
    /// <summary>
    /// symbol, name is mangled ('_func');
    /// only one of #NTF_TYPE and #NTF_SYMU, #NTF_SYMM can be used
    /// </summary>
    NTF_SYMM = 0x0000,
    /// <summary>
    /// don't inspect base tils (for get_named_type)
    /// </summary>
    NTF_NOBASE = 0x0002,
    /// <summary>
    /// replace original type (for set_named_type)
    /// </summary>
    NTF_REPLACE = 0x0004,
    /// <summary>
    /// name is unmangled (don't use this flag)
    /// </summary>
    NTF_UMANGLED = 0x0008,
    /// <summary>
    /// don't inspect current til file (for get_named_type)
    /// </summary>
    NTF_NOCUR = 0x0020,
    /// <summary>
    /// value is 64bit
    /// </summary>
    NTF_64BIT = 0x0040,
    
    /// <summary>
    /// force-validate the name of the type when setting
    /// (set_named_type, set_numbered_type only)
    /// </summary>
    NTF_FIXNAME = 0x0080,
    
    /// <summary>
    /// the name is given in the IDB encoding;
    /// non-ASCII bytes will be decoded accordingly
    /// (set_named_type, set_numbered_type only)
    /// </summary>
    NTF_IDBENC = 0x0100,
    
    /// <summary>
    /// check that synchronization to IDB passed OK
    /// (set_numbered_type, set_named_type)
    /// </summary>
    NTF_CHKSYNC = 0x0200,
    
    /// <summary>
    /// do not validate type name
    /// (set_numbered_type, set_named_type)
    /// </summary>
    NTF_NO_NAMECHK = 0x0400,
    
    /// <summary>
    /// save a new type definition, not a typeref (tinfo_t::set_numbered_type, tinfo_t::set_named_type)
    /// </summary>
    NTF_COPY = 0x1000,
}