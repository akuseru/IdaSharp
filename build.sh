#!/bin/bash

BUILD_TYPE=${1:-Release}
BUILD_DIR="build"

echo "Building FFXIVPlayground C++ library in $BUILD_TYPE mode..."

# Check IDA SDK status
if [ -n "$IDASDKDIR" ]; then
    echo "IDA SDK directory: $IDASDKDIR"
    if [ -d "$IDASDKDIR" ]; then
        echo "✓ IDA SDK directory exists"
    else
        echo "✗ IDA SDK directory does not exist"
    fi
else
    echo "IDA SDK not configured (IDASDKDIR not set)"
fi

if [ ! -d "$BUILD_DIR" ]; then
    mkdir "$BUILD_DIR"
fi

cd "$BUILD_DIR"

cmake -DCMAKE_BUILD_TYPE="$BUILD_TYPE" ..
cmake --build . --config "$BUILD_TYPE"

echo "Build complete!"
echo "Native library (C++): $BUILD_DIR/lib/libidasharp.dylib"
echo "Managed library (C#): $BUILD_DIR/bin/IdaSharp.dll"