# IdaSharp - IDA Pro SDK for .NET

A cross-platform .NET wrapper for IDA Pro SDK functionality, providing access to IDA's reverse engineering capabilities from managed code.

## Features

- **Cross-Platform**: Works on Windows, macOS (Intel & ARM64), and Linux
- **Native Integration**: Direct access to IDA SDK functions through P/Invoke
- **Memory Safe**: Proper memory management for IDA SDK structures
- **Easy to Use**: Simple C# API wrapping complex IDA SDK functionality

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package IdaSharp
```

Or via Package Manager Console:

```powershell
Install-Package IdaSharp
```

## Quick Start
todo...

## Requirements

- .NET 9.0 or later
- IDA Pro SDK 9.1 or later.

## Supported Functions

- `create_enum_type_wrapper()` - Create IDA enum types
- `free_enum_type_data()` - Free enum type memory
- `get_version()` - Get library version
- `add_numbers()` - Example function

## Building from Source

1. Set the IDA SDK path:
   ```bash
   export IDASDKDIR=/path/to/idasdk
   ```

2. Build the project:
   ```bash
   ./build.sh
   ```

3. Create NuGet package:
   ```bash
   ./pack-nuget.sh
   ```

## License

MIT License - see LICENSE file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.