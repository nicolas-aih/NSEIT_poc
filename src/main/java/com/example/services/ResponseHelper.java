// File: src/main/java/com/example/services/ResponseHelper.java
package com.example.services; // <-- Make sure this matches your directory structure (com/example/services)

import java.util.HashMap;
import java.util.List;
import java.util.Map; // <-- Make sure this matches your directory structure (com/example/web/response)

import org.springframework.stereotype.Service; // <-- Make sure this matches your directory structure (com/example/common)

import com.example.common.Errorlogger;
import com.example.web.response.ApiResponse;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;

@Service // Mark as a Spring service so it can be injected
public class ResponseHelper {

    // Jackson ObjectMapper for converting List<Map> to JSON string for _EXTRA_ field
    // Ideally, you would inject this, but instantiating is fine for the stub/basic service
    private final ObjectMapper objectMapper = new ObjectMapper();


    // --- Helper methods to create standard ApiResponse objects ---

    public <T> ApiResponse<T> createSuccessResponse(String message, T data) {
        return new ApiResponse.Builder<T>()
                .status(true)
                .message(message)
                .data(data)
                .build();
    }

     // Use Map for extras as expected by the client
     public <T> ApiResponse<T> createSuccessResponse(String message, T data, Map<String, Object> extra) {
         return new ApiResponse.Builder<T>()
                .status(true)
                .message(message)
                .data(data)
                .extra(extra) // Set the entire extra map
                .build();
     }


    public <T> ApiResponse<T> createErrorResponse(String message) {
        return new ApiResponse.Builder<T>()
                .status(false)
                .message(message)
                .build();
    }

     public <T> ApiResponse<T> createErrorResponse(String message, T data) {
        return new ApiResponse.Builder<T>()
                .status(false)
                .message(message)
                 .data(data)
                .build();
    }

    // Helper to create a success response with a single extra item
     public <T> ApiResponse<T> createSuccessResponseWithExtra(String message, T data, String extraKey, Object extraValue) {
          Map<String, Object> extraMap = new HashMap<>();
          extraMap.put(extraKey, extraValue);
          return createSuccessResponse(message, data, extraMap);
     }

    // Helper to create an error response with a single extra item
      public <T> ApiResponse<T> createErrorResponseWithExtra(String message, T data, String extraKey, Object extraValue) {
          Map<String, Object> extraMap = new HashMap<>();
          extraMap.put(extraKey, extraValue);
          ApiResponse<T> response = new ApiResponse.Builder<T>()
                .status(false)
                .message(message)
                 .data(data)
                 .extra(extraMap)
                .build();
           return response;
     }


     // --- NEW: Method to add an extra key/value to an *existing* ApiResponse ---
     // This method is needed to match the calls made in the ExamBatchesController.
    // --- Method to add an extra key/value to an *existing* ApiResponse ---
    public <T> ApiResponse<T> addExtraToResponse(ApiResponse<T> response, String extraKey, Object extraValue) {
        if (response == null) {
            // Handle case where the input response is null
            return new ApiResponse.Builder<T>()
                    .status(false)
                    .message("Internal error: Attempted to add extra to null response.")
                    .build();
        }
        Map<String, Object> extraMap = response.get_EXTRA_();
            
        // if (extraMap == null) {
        //  // If the response doesn't have an extras map yet, create one
        //     extraMap = new HashMap in the `ApiResponse` class, but that field is `private`.;
         // Add the new key/value to the extras map
// Now, add the key/value to the retrieved or newly created map
        extraMap.put(extraKey, extraValue);

        // Return the modified response object

         // Return the modified response object
         return response;
     }


     // --- Specific method to convert List<Map> to JSON string ---
     // This is needed specifically for the nested "PaymentMode" JSON string expected by the client JS
     public String convertListMapToJsonString(List<Map<String, Object>> data) {
         if (data == null) return "[]"; // Return empty array JSON string if data is null
         try {
              // Use the instantiated ObjectMapper to serialize the List<Map> to a JSON string
             return objectMapper.writeValueAsString(data);
         } catch (JsonProcessingException e) {
             System.err.println("Error converting List<Map> to JSON string: " + e.getMessage());
             // Log the error using your Errorlogger stub
             Errorlogger.logError(this.getClass().getName(), "convertListMapToJsonString", e, data);
             return "[]"; // Return empty array JSON string on error
         }
     }
}