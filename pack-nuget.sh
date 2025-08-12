#!/bin/bash

set -e

PACKAGE_VERSION=${1:-1.0.0}
OUTPUT_DIR="packages"

echo "Creating NuGet package version $PACKAGE_VERSION..."

# Ensure output directory exists
mkdir -p "$OUTPUT_DIR"

# Check if dotnet is available
if ! command -v dotnet &> /dev/null; then
    echo "Error: dotnet CLI not found"
    exit 1
fi

# Check if nuget is available (fallback to dotnet)
if command -v nuget &> /dev/null; then
    NUGET_CMD="nuget"
else
    NUGET_CMD="dotnet nuget"
fi

# Ensure build is complete
if [ ! -f "build/bin/IdaPro.dll" ]; then
    echo "Error: Build not found. Run './build.sh' first."
    exit 1
fi

# Ensure native library is available for packaging
if [ ! -f "build/bin/libidasharp.1.0.0.dylib" ]; then
    echo "Copying native library to bin directory..."
    cp build/lib/libidasharp.1.0.0.dylib build/bin/ 2>/dev/null || true
fi

# Create the NuGet package using dotnet pack (better integration)
echo "Creating NuGet package using dotnet pack..."
dotnet pack IdaPro/IdaPro.csproj -o "$OUTPUT_DIR" -c Release --include-symbols -p:PackageVersion="$PACKAGE_VERSION" --verbosity normal

echo "NuGet package created successfully!"
echo "Output: $OUTPUT_DIR/IdaSharp.$PACKAGE_VERSION.nupkg"

# List created packages
ls -la "$OUTPUT_DIR"/*.nupkg 2>/dev/null || echo "No .nupkg files found in $OUTPUT_DIR"