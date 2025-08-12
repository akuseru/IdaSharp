#pragma once

#ifdef _WIN32
    #ifdef BUILDING_IDASHARP
        #define IDASHARP_EXPORT __declspec(dllexport)
    #else
        #define IDASHARP_EXPORT __declspec(dllimport)
    #endif
#else
    #define IDASHARP_EXPORT __attribute__((visibility("default")))
#endif

// Forward declaration for enum_type_data_t
typedef struct enum_type_data_t enum_type_data_t;

#ifdef __cplusplus
extern "C" {
#endif


// IDA SDK functions
IDASHARP_EXPORT enum_type_data_t* create_enum_type_wrapper(
    const char* enum_name,
    int enum_width,
    int sign,
    bool convert_to_bitmask,
    const char* enum_cmt
);

IDASHARP_EXPORT void free_enum_type_data(enum_type_data_t* ei);

#ifdef __cplusplus
}
#endif