#!/bin/bash

set -e

PACKAGE_PATH="packages/IdaSharp.1.0.2.nupkg"

echo "Testing NuGet package: $PACKAGE_PATH"

if [ ! -f "$PACKAGE_PATH" ]; then
    echo "❌ Package not found: $PACKAGE_PATH"
    exit 1
fi

echo "✅ Package exists"

# Check package contents
echo "📦 Package contents:"
unzip -l "$PACKAGE_PATH" | grep -E "(lib/|runtimes/|build/)" | head -10

# Check native libraries
NATIVE_LIBS=$(unzip -l "$PACKAGE_PATH" | grep -c "native/.*dylib" || echo "0")
echo "🔧 Native libraries found: $NATIVE_LIBS"

if [ "$NATIVE_LIBS" -gt 0 ]; then
    echo "✅ Native libraries are included"
else
    echo "❌ No native libraries found"
    exit 1
fi

# Check managed assembly
MANAGED_LIBS=$(unzip -l "$PACKAGE_PATH" | grep -c "lib/.*dll" || echo "0")
echo "📚 Managed libraries found: $MANAGED_LIBS"

if [ "$MANAGED_LIBS" -gt 0 ]; then
    echo "✅ Managed assembly is included"
else
    echo "❌ No managed assembly found"
    exit 1
fi

# Check build files
BUILD_FILES=$(unzip -l "$PACKAGE_PATH" | grep -c "build/.*\(props\|targets\)" || echo "0")
echo "🛠️  Build files found: $BUILD_FILES"

if [ "$BUILD_FILES" -gt 0 ]; then
    echo "✅ Build files are included"
else
    echo "❌ No build files found"
    exit 1
fi

echo ""
echo "🎉 Package validation successful!"
echo "📋 Summary:"
echo "   - Native libraries: $NATIVE_LIBS"
echo "   - Managed libraries: $MANAGED_LIBS" 
echo "   - Build files: $BUILD_FILES"