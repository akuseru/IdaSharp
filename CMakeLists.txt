cmake_minimum_required(VERSION 3.16)
project(idasharp VERSION 1.0.0 LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

set(CMAKE_POSITION_INDEPENDENT_CODE ON)

if(WIN32)
    set(CMAKE_WINDOWS_EXPORT_ALL_SYMBOLS ON)
endif()

set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/bin)
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/lib)
set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/lib)

if(CMAKE_BUILD_TYPE STREQUAL "Debug")
    add_compile_definitions(DEBUG)
    if(MSVC)
        add_compile_options(/W4 /WX)
    else()
        add_compile_options(-Wall -Wextra -Werror -g)
    endif()
else()
    add_compile_definitions(NDEBUG)
    if(MSVC)
        add_compile_options(/O2)
    else()
        add_compile_options(-O3)
    endif()
endif()

if(APPLE)
    set(CMAKE_MACOSX_RPATH ON)
    set(CMAKE_INSTALL_RPATH "@loader_path")
elseif(UNIX)
    set(CMAKE_INSTALL_RPATH "$ORIGIN")
endif()

# Check for IDA SDK (Required)
set(IDASDKDIR $ENV{IDASDKDIR})

if(NOT IDASDKDIR)
    message(FATAL_ERROR "IDASDKDIR environment variable not set")
endif()

if(NOT EXISTS "${IDASDKDIR}")
    message(FATAL_ERROR "IDA SDK directory not found: ${IDASDKDIR}")
endif()

# Check for include and lib directories
set(IDA_INCLUDE_DIR "${IDASDKDIR}/include")
set(IDA_LIB_DIR "${IDASDKDIR}/lib")

if(NOT EXISTS "${IDA_INCLUDE_DIR}")
    message(FATAL_ERROR "IDA SDK include directory not found at: ${IDA_INCLUDE_DIR}")
endif()

if(NOT EXISTS "${IDA_LIB_DIR}")
    message(FATAL_ERROR "IDA SDK lib directory not found at: ${IDA_LIB_DIR}")
endif()

# Determine platform-specific library directory
if(WIN32)
    if(CMAKE_SIZEOF_VOID_P EQUAL 8)
        set(IDA_PLATFORM_LIB_DIR "${IDA_LIB_DIR}/x64_win_vc_64")
    else()
        set(IDA_PLATFORM_LIB_DIR "${IDA_LIB_DIR}/x86_win_vc_32")
    endif()
elseif(APPLE)
    if(CMAKE_SYSTEM_PROCESSOR MATCHES "arm64")
        if(CMAKE_SIZEOF_VOID_P EQUAL 8)
            set(IDA_PLATFORM_LIB_DIR "${IDA_LIB_DIR}/arm64_mac_clang_64")
        else()
            set(IDA_PLATFORM_LIB_DIR "${IDA_LIB_DIR}/arm64_mac_clang_32")
        endif()
    else()
        if(CMAKE_SIZEOF_VOID_P EQUAL 8)
            set(IDA_PLATFORM_LIB_DIR "${IDA_LIB_DIR}/x64_mac_clang_64")
        else()
            set(IDA_PLATFORM_LIB_DIR "${IDA_LIB_DIR}/x64_mac_clang_32")
        endif()
    endif()
elseif(UNIX)
    if(CMAKE_SIZEOF_VOID_P EQUAL 8)
        set(IDA_PLATFORM_LIB_DIR "${IDA_LIB_DIR}/x64_linux_gcc_64")
    else()
        set(IDA_PLATFORM_LIB_DIR "${IDA_LIB_DIR}/x86_linux_gcc_32")
    endif()
endif()

if(NOT EXISTS "${IDA_PLATFORM_LIB_DIR}")
    message(FATAL_ERROR "IDA SDK platform library directory not found at: ${IDA_PLATFORM_LIB_DIR}")
endif()

message(STATUS "IDA SDK found at: ${IDASDKDIR}")
message(STATUS "Platform library directory: ${IDA_PLATFORM_LIB_DIR}")

file(GLOB_RECURSE CPP_SOURCES "src/*.cpp" "src/*.c")
file(GLOB_RECURSE CPP_HEADERS "src/*.hpp" "src/*.h" "include/*.hpp" "include/*.h")

message(STATUS "Found C++ sources: ${CPP_SOURCES}")
message(STATUS "Found headers: ${CPP_HEADERS}")

if(CPP_SOURCES)
    add_library(LIBIDASHARP SHARED ${CPP_SOURCES} ${CPP_HEADERS})
    
    target_include_directories(LIBIDASHARP
        PUBLIC 
            $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}/include>
            $<INSTALL_INTERFACE:include>
        PRIVATE 
            ${CMAKE_CURRENT_SOURCE_DIR}/src
    )
    
    # Add IDA SDK include directory
    target_include_directories(LIBIDASHARP PRIVATE "${IDA_INCLUDE_DIR}")
    
    # Find and link IDA libraries based on platform
    if(WIN32)
        # Windows libraries
        find_library(IDA_LIB ida PATHS "${IDA_PLATFORM_LIB_DIR}" NO_DEFAULT_PATH)
        find_library(IDALIB_LIB idalib PATHS "${IDA_PLATFORM_LIB_DIR}" NO_DEFAULT_PATH)
        find_library(NETWORK_LIB network PATHS "${IDA_PLATFORM_LIB_DIR}" NO_DEFAULT_PATH)
        
        if(IDA_LIB)
            target_link_libraries(LIBIDASHARP PRIVATE "${IDA_LIB}")
        endif()
        if(IDALIB_LIB)
            target_link_libraries(LIBIDASHARP PRIVATE "${IDALIB_LIB}")
        endif()
        if(NETWORK_LIB)
            target_link_libraries(LIBIDASHARP PRIVATE "${NETWORK_LIB}")
        endif()
        
    elseif(APPLE)
        # macOS libraries
        find_library(LIBIDA_LIB ida PATHS "${IDA_PLATFORM_LIB_DIR}" NO_DEFAULT_PATH)
        find_library(LIBIDALIB_LIB idalib PATHS "${IDA_PLATFORM_LIB_DIR}" NO_DEFAULT_PATH)
        find_library(NETWORK_LIB network PATHS "${IDA_PLATFORM_LIB_DIR}" NO_DEFAULT_PATH)
        
        if(LIBIDA_LIB)
            target_link_libraries(LIBIDASHARP PRIVATE "${LIBIDA_LIB}")
            message(STATUS "Found libida: ${LIBIDA_LIB}")
        endif()
        if(LIBIDALIB_LIB)
            target_link_libraries(LIBIDASHARP PRIVATE "${LIBIDALIB_LIB}")
            message(STATUS "Found libidalib: ${LIBIDALIB_LIB}")
        endif()
        if(NETWORK_LIB)
            target_link_libraries(LIBIDASHARP PRIVATE "${NETWORK_LIB}")
            message(STATUS "Found network: ${NETWORK_LIB}")
        endif()
        
    elseif(UNIX)
        # Linux libraries
        find_library(LIBIDA_LIB libida PATHS "${IDA_PLATFORM_LIB_DIR}" NO_DEFAULT_PATH)
        find_library(LIBIDALIB_LIB libidalib PATHS "${IDA_PLATFORM_LIB_DIR}" NO_DEFAULT_PATH)
        find_library(NETWORK_LIB network PATHS "${IDA_PLATFORM_LIB_DIR}" NO_DEFAULT_PATH)
        find_library(PRO_LIB pro PATHS "${IDA_PLATFORM_LIB_DIR}" NO_DEFAULT_PATH)
        find_library(COMPRESS_LIB compress PATHS "${IDA_PLATFORM_LIB_DIR}" NO_DEFAULT_PATH)
        find_library(UNICODE_LIB unicode PATHS "${IDA_PLATFORM_LIB_DIR}" NO_DEFAULT_PATH)
        
        if(LIBIDA_LIB)
            target_link_libraries(LIBIDASHARP PRIVATE "${LIBIDA_LIB}")
        endif()
        if(LIBIDALIB_LIB)
            target_link_libraries(LIBIDASHARP PRIVATE "${LIBIDALIB_LIB}")
        endif()
        if(NETWORK_LIB)
            target_link_libraries(LIBIDASHARP PRIVATE "${NETWORK_LIB}")
        endif()
        if(PRO_LIB)
            target_link_libraries(LIBIDASHARP PRIVATE "${PRO_LIB}")
        endif()
        if(COMPRESS_LIB)
            target_link_libraries(LIBIDASHARP PRIVATE "${COMPRESS_LIB}")
        endif()
        if(UNICODE_LIB)
            target_link_libraries(LIBIDASHARP PRIVATE "${UNICODE_LIB}")
        endif()
    endif()
    
    target_compile_definitions(LIBIDASHARP PRIVATE HAS_IDA_SDK=1 BUILDING_IDASHARP)
    message(STATUS "Linking idasharp with IDA SDK libraries from ${IDA_PLATFORM_LIB_DIR}")
    
    set_target_properties(LIBIDASHARP PROPERTIES
        OUTPUT_NAME "idasharp"
    )
    
    if(WIN32)
        set_target_properties(LIBIDASHARP PROPERTIES
            PREFIX ""
            SUFFIX ".dll"
        )
    elseif(APPLE)
        set_target_properties(LIBIDASHARP PROPERTIES
            PREFIX "lib"
            SUFFIX ".dylib"
        )
    else()
        set_target_properties(LIBIDASHARP PROPERTIES
            PREFIX "lib"
            SUFFIX ".so"
        )
    endif()
    
    install(TARGETS LIBIDASHARP
        LIBRARY DESTINATION bin
        RUNTIME DESTINATION bin
        ARCHIVE DESTINATION lib
    )
    
    install(DIRECTORY include/ DESTINATION include)
endif()

option(BUILD_TESTS "Build test programs" OFF)
if(BUILD_TESTS AND CPP_SOURCES)
    enable_testing()
    add_subdirectory(tests)
endif()

# C# IdaSharp Project Configuration using dotnet CLI
find_program(DOTNET_EXECUTABLE dotnet)

if(DOTNET_EXECUTABLE)
    message(STATUS "Found dotnet: ${DOTNET_EXECUTABLE}")
    
    # Check if IdaSharp.csproj exists
    if(EXISTS "${CMAKE_CURRENT_SOURCE_DIR}/IdaSharp/IdaSharp.csproj")
        # Create custom target for C# project
        add_custom_target(IDASHARPNET ALL
            COMMAND ${DOTNET_EXECUTABLE} build "${CMAKE_CURRENT_SOURCE_DIR}/IdaSharp/IdaSharp.csproj" 
                    --configuration Release 
                    --output "${CMAKE_BINARY_DIR}/bin"
                    --verbosity minimal
            WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}/IdaSharp"
            COMMENT "Building C# IdaSharp project"
            VERBATIM
        )
        
        # Make C# project depend on the native library
        if(TARGET LIBIDASHARP)
            add_dependencies(IDASHARPNET LIBIDASHARP)
        endif()
        
        # Copy native library to C# output directory after build
        add_custom_command(TARGET IDASHARPNET POST_BUILD
            COMMAND ${CMAKE_COMMAND} -E copy_if_different
                    "$<TARGET_FILE:LIBIDASHARP>"
                    "${CMAKE_BINARY_DIR}/bin/"
            COMMAND ${CMAKE_COMMAND} -E copy_if_different
                    "$<TARGET_FILE:LIBIDASHARP>"
                    "${CMAKE_BINARY_DIR}/bin/$<IF:$<PLATFORM_ID:Windows>,idasharp.dll,$<IF:$<PLATFORM_ID:Darwin>,libidasharp.dylib,libidasharp.so>>"
            COMMENT "Copying native library to C# output directory"
        )
        
        message(STATUS "C# IdaSharp project configured with dotnet CLI")
        
        # Add NuGet package creation target
        add_custom_target(nuget-pack
            COMMAND ${CMAKE_COMMAND} -E copy_if_different
                    "${CMAKE_BINARY_DIR}/bin/libidasharp.dylib"
            COMMAND ${DOTNET_EXECUTABLE} pack "${CMAKE_CURRENT_SOURCE_DIR}/IdaSharp.nuspec"
                    -o "${CMAKE_BINARY_DIR}/packages"
                    -p:Configuration=Release
                    -p:PackageVersion=1.0.0
            WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}"
            DEPENDS IDASHARPNET
            COMMENT "Creating NuGet package"
            VERBATIM
        )
        
        # Alternative: Create NuGet package using dotnet pack on the csproj
        add_custom_target(nuget-pack-csproj
            COMMAND ${DOTNET_EXECUTABLE} pack "${CMAKE_CURRENT_SOURCE_DIR}/IdaSharp/IdaSharp.csproj"
                    -o "${CMAKE_BINARY_DIR}/packages"
                    -c Release
                    --no-build
                    --include-symbols
            WORKING_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}"
            DEPENDS IDASHARPNET
            COMMENT "Creating NuGet package from csproj"
            VERBATIM
        )
    else()
        message(WARNING "IdaSharp.csproj not found - C# project will not be built")
    endif()
else()
    message(WARNING "dotnet CLI not found - C# project will not be built")
endif()