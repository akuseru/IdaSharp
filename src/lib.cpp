#include "idasharp.h"
#include <pro.h>
#include <typeinf.hpp>

extern "C" {
 
    IDASHARP_EXPORT enum_type_data_t* create_enum_type_wrapper(
        const char* enum_name,
        int enum_width,
        int sign,
        bool convert_to_bitmask,
        const char* enum_cmt
    ) {
        if (!enum_name) {
            return nullptr;
        }
    
        try {
            // Allocate memory for the enum_type_data_t that will be returned
            enum_type_data_t* ei = new enum_type_data_t();
        
            // Call the IDA SDK function
            bool result = create_enum_type(
                enum_name,
                *ei,
                enum_width,
                static_cast<type_sign_t>(sign),
                convert_to_bitmask,
                enum_cmt
            );
        
            if (result) {
                return ei;
            } else {
                delete ei;
                return nullptr;
            }
        } catch (...) {
            return nullptr;
        }
    }

    IDASHARP_EXPORT void free_enum_type_data(enum_type_data_t* ei) {
        if (ei) {
            delete ei;
        }
    }

}