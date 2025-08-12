@echo off
set BUILD_TYPE=%1
if "%BUILD_TYPE%"=="" set BUILD_TYPE=Release

echo Building FFXIVPlayground C++ library in %BUILD_TYPE% mode...

REM Check IDA SDK status
if defined IDASDKDIR (
    echo IDA SDK directory: %IDASDKDIR%
    if exist "%IDASDKDIR%" (
        echo ✓ IDA SDK directory exists
    ) else (
        echo ✗ IDA SDK directory does not exist
    )
) else (
    echo IDA SDK not configured (IDASDKDIR not set)
)

if not exist build mkdir build
cd build

cmake -DCMAKE_BUILD_TYPE=%BUILD_TYPE% ..
cmake --build . --config %BUILD_TYPE%

echo Build complete!
echo Native library (C++): build\bin\idasharp.dll
echo Managed library (C#): build\bin\IdaSharp.dll