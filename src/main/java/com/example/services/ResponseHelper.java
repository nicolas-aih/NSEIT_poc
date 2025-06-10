// File: src/main/java/com/example/services/ResponseHelper.java
package com.example.services;

import java.util.HashMap; // Assuming Errorlogger is in com.example.common
import java.util.List; // Import ApiResponse from its correct package
import java.util.Map;

import org.springframework.stereotype.Service; // Jackson is standard in Spring Boot for JSON

import com.example.common.Errorlogger; // Annotation to make this a Spring bean
import com.example.web.response.ApiResponse;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;

@Service // Mark as a Spring service so it can be injected into controllers
public class ResponseHelper {

    // Jackson ObjectMapper for converting List<Map> to JSON string for _EXTRA_ field.
    // Spring Boot auto-configures an ObjectMapper; ideally you would inject it,
    // but instantiating directly is acceptable for a simple helper if injection is complex.
    private final ObjectMapper objectMapper = new ObjectMapper();


    // --- Helper methods to create standard ApiResponse objects using the Builder ---

    public <T> ApiResponse<T> createSuccessResponse(String message, T data) {
        // Uses the Builder pattern from ApiResponse
        return new ApiResponse.Builder<T>()
                .status(true) // Set status to true for success
                .message(message)
                .data(data)
                // Extra is not set here; it will be null or the default from the Builder
                .build();
    }

     // Overload to create a success response and set the entire extra Map directly
     public <T> ApiResponse<T> createSuccessResponse(String message, T data, Map<String, Object> extra) {
         // Uses the Builder pattern from ApiResponse
         return new ApiResponse.Builder<T>()
                .status(true) // Set status to true for success
                .message(message)
                .data(data)
                .extra(extra) // Set the entire extra map using the builder's overload
                .build();
     }


    public <T> ApiResponse<T> createErrorResponse(String message) {
         // Uses the Builder pattern from ApiResponse
        return new ApiResponse.Builder<T>()
                .status(false) // Set status to false for error
                .message(message)
                // Data and Extra are not set here; they will be null
                .build();
    }

     // Overload to create an error response and include data (less common, but possible)
     public <T> ApiResponse<T> createErrorResponse(String message, T data) {
         // Uses the Builder pattern from ApiResponse
        return new ApiResponse.Builder<T>()
                .status(false) // Set status to false for error
                .message(message)
                 .data(data)
                // Extra is not set here; will be null
                .build();
    }

    // Helper to create a success response and add a single extra item to a new Map
     public <T> ApiResponse<T> createSuccessResponseWithExtra(String message, T data, String extraKey, Object extraValue) {
          Map<String, Object> extraMap = new HashMap<>();
          extraMap.put(extraKey, extraValue);
          // Call the overload that takes a Map for extra
          return createSuccessResponse(message, data, extraMap);
     }

    // Helper to create an error response and add a single extra item to a new Map
      public <T> ApiResponse<T> createErrorResponseWithExtra(String message, T data, String extraKey, Object extraValue) {
          Map<String, Object> extraMap = new HashMap<>();
          extraMap.put(extraKey, extraValue);
           // Use the builder pattern directly and set the created map
           return new ApiResponse.Builder<T>()
                .status(false) // Error status
                .message(message)
                 .data(data)
                 .extra(extraMap) // Set the created extra map
                .build();
     }


     // --- Method to add an extra key/value to an *existing* ApiResponse object ---
     // This method is called by the controller to add extra data *after* the initial response object is created.
     // This is the method where the NullPointerException occurred previously.
      public <T> ApiResponse<T> addExtraToResponse(ApiResponse<T> response, String key, Object value) {
        // Get the current extra value from the response object
        Object currentExtra = response.getExtra();

        // Determine the map we will work with
        Map<String, Object> extraMap;

        // Check if the current extra value is already a Map
        if (currentExtra instanceof Map) {
            // If it is, use that Map directly
            extraMap = (Map<String, Object>) currentExtra;
            // No need to call response.setExtra() again here as we are modifying the existing map reference
        } else {
            // If extra is null OR not a Map (e.g., it was set to a String, Number, etc.)
            // We need to create a new Map.
            // Based on the observed C# pattern of adding keys to 'Extra' when it's a JObject,
            // if it's not a Map, we discard the old value and start a new Map for extras.
            if (currentExtra != null) {
                 // Optional: Log a warning if we are discarding a non-Map extra value
                 System.err.println("ResponseHelper: WARNING - Overwriting existing 'extra' value which was not a Map (" + currentExtra.getClass().getName() + "). Key: " + key + ", Value: " + value);
            }
            // Create a new, empty Map
            extraMap = new HashMap<>();
            // Set this new Map as the 'extra' field of the ApiResponse object
            response.setExtra(extraMap); // <-- This is crucial to ensure the response object holds the map
        }

        // Now, 'extraMap' is guaranteed to be a non-null Map instance that is also set
        // as the 'extra' field of the 'response' object (if it wasn't already).
        // We can safely put the new key-value pair into this map.
        extraMap.put(key, value); // This line should now be safe from NPE on 'extraMap'

        System.out.println("ResponseHelper: Added extra data '" + key + "' to response. Current response.extra state: " + response.getExtra());
        return response; // Return the modified response object (useful for chaining or assignment)
    }


     // --- Specific method to convert List<Map> to JSON string ---
     // This is needed specifically for the nested "PaymentMode" JSON string expected by the client JS.
     // Requires Jackson library for ObjectMapper.
     public String convertListMapToJsonString(List<Map<String, Object>> data) {
         if (data == null) return "[]"; // Return empty array JSON string if data is null
         try {
              // Use the instantiated ObjectMapper to serialize the List<Map> to a JSON string
             return objectMapper.writeValueAsString(data);
         } catch (JsonProcessingException e) {
             System.err.println("Error converting List<Map> to JSON string: " + e.getMessage());
             // Log the error using your Errorlogger stub
             Errorlogger.logError(this.getClass().getName(), "convertListMapToJsonString", e, "Data: " + data);
             return "[]"; // Return empty array JSON string on error
         }
     }
}