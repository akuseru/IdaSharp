namespace IdaPro.Enums;

[Flags]
public enum NefFlags : ushort
{
    /// <summary>
    /// Create segments
    /// </summary>
    NEF_SEGS = 0x0001,
    /// <summary>
    /// Load resources
    /// </summary>
    NEF_RSCS = 0x0002,
    /// <summary>
    /// Rename entries
    /// </summary>
    NEF_NAME = 0x0004,
    /// <summary>
    /// Manual load
    /// </summary>
    NEF_MAN = 0x0008,
    /// <summary>
    /// Fill segment gaps
    /// </summary>
    NEF_FILL = 0x0010,
    
    /// <summary>
    /// Create import segment
    /// </summary>
    NEF_IMPS = 0x0020,
    
    /// <summary>
    /// This is the first file loaded into the database. 
    /// </summary>
    NEF_FIRST = 0x0080,
    
    /// <summary>
    /// for load_binary_file(): load as a code segment
    /// </summary>
    NEF_CODE = 0x0100,
    
    /// <summary>
    /// reload the file at the same place:
    /// </summary>
    NEF_RELOAD = 0x0200,
    
    /// <summary>
    /// Autocreate FLAT group (PE)
    /// </summary>
    NEF_FLAT = 0x0400,
    
    /// <summary>
    /// Create mini database
    /// do not copy segment bytes from the input file; use only the file header metadata
    /// </summary>
    NEF_MINI = 0x0800,
    
    /// <summary>
    /// Display additional loader options dialog
    /// </summary>
    NEF_LOPT = 0x1000,
    
    /// <summary>
    /// Load all segments without questions
    /// </summary>
    NEF_LALL =  0x2000,
}