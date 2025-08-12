# IdaSharp Usage Examples

## Installation

```bash
dotnet add package IdaSharp
```

## Basic Usage

### 1. Simple Version Check

```csharp
using System;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("idasharp", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr get_version();
    
    [DllImport("idasharp", CallingConvention = CallingConvention.Cdecl)]
    public static extern int add_numbers(int a, int b);

    static void Main()
    {
        // Get library version
        var versionPtr = get_version();
        var version = Marshal.PtrToStringAnsi(versionPtr);
        Console.WriteLine($"IdaSharp Version: {version}");
        
        // Test basic functionality
        var result = add_numbers(5, 3);
        Console.WriteLine($"5 + 3 = {result}");
    }
}
```

### 2. Creating IDA Enum Types

```csharp
using System;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("idasharp", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr create_enum_type_wrapper(
        string enumName,
        int enumWidth,
        int sign,
        bool convertToBitmask,
        string enumComment
    );
    
    [DllImport("idasharp", CallingConvention = CallingConvention.Cdecl)]
    public static extern void free_enum_type_data(IntPtr enumData);

    static void Main()
    {
        // Create a new enum type
        var enumData = create_enum_type_wrapper(
            "MyCustomEnum",     // enum name
            4,                  // 4 bytes wide
            0,                  // unsigned (0) or signed (1)
            false,              // not a bitmask
            "Custom enum for demo"  // comment
        );

        if (enumData != IntPtr.Zero)
        {
            Console.WriteLine("✅ Enum type created successfully!");
            
            // Always free the allocated memory
            free_enum_type_data(enumData);
            Console.WriteLine("✅ Memory freed");
        }
        else
        {
            Console.WriteLine("❌ Failed to create enum type");
        }
    }
}
```

### 3. Using with the IdaPro Managed Wrapper

```csharp
using IdaPro;

class Program
{
    static void Main()
    {
        try
        {
            // Use the managed wrapper (if available in your IdaPro namespace)
            var ida = new Ida();
            
            // The managed wrapper handles P/Invoke calls for you
            Console.WriteLine("IdaSharp initialized successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
```

## Platform-Specific Notes

### Windows
- Native library: `idasharp.dll`
- Automatically copied to output directory

### macOS
- Native library: `libidasharp.dylib`
- Supports both Intel (x64) and Apple Silicon (ARM64)

### Linux
- Native library: `libidasharp.so`
- Requires compatible IDA SDK libraries

## Error Handling

Always check return values and handle potential errors:

```csharp
var enumData = create_enum_type_wrapper("TestEnum", 4, 0, false, null);

if (enumData == IntPtr.Zero)
{
    Console.WriteLine("Failed to create enum - check IDA SDK availability");
    return;
}

try
{
    // Use the enum data...
}
finally
{
    // Always free memory in a finally block
    free_enum_type_data(enumData);
}
```

## Troubleshooting

1. **Native library not found**: Ensure the native library is in your output directory
2. **IDA SDK errors**: Verify IDA SDK is properly installed and accessible
3. **Platform issues**: Check that you're using the correct runtime identifier

For more examples, see the test projects in the IdaSharp repository.