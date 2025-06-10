// File: src/main/java/com/example/web/response/ApiResponse.java
package com.example.web.response;

import java.util.Map; // Import Map

// Equivalent of your C# ApiResponse or similar JSON response structure
// Using generics <T> for the data payload type
public class ApiResponse<T> {
    private boolean status;
    private String message;
    private T data; // The main payload, e.g., List<Map<String, Object>>
    private Object extra; // Additional data, e.g., JSON string for PaymentMode (using Object for flexibility based on C# JObject)

    // Private constructor to force use of the Builder
    private ApiResponse(Builder<T> builder) {
        this.status = builder.status;
        this.message = builder.message;
        this.data = builder.data;
        this.extra = builder.extra;
    }

    // Default constructor (needed by some frameworks like Jackson for deserialization)
    // It's recommended to keep this public or protected if your framework needs it.
     public ApiResponse() {}


    // --- Getters and Setters (REQUIRED by ResponseHelper.addExtraToResponse and likely JSON serialization) ---
    // These *must* be public for JSON serialization/deserialization and for ResponseHelper to access them.
    public boolean isStatus() {
        return status;
    }

    public void setStatus(boolean status) {
        this.status = status;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public T getData() {
        return data;
    }

    public void setData(T data) {
        this.data = data;
    }

    public Object getExtra() { // Return as Object as per original stub field type for flexibility
        return extra;
    }

    public void setExtra(Object extra) { // Method used by ResponseHelper.addExtraToResponse
        this.extra = extra;
    }


    @Override
    public String toString() {
        return "ApiResponse{" +
               "status=" + status +
               ", message='" + message + '\'' +
               ", data=" + (data != null ? "..." + data.getClass().getSimpleName() + " payload" : "null") + // Avoid printing large data payloads
               ", extra=" + extra +
               '}';
    }

    // --- Builder Static Inner Class ---
    // This is used by ResponseHelper's create methods to construct ApiResponse objects.
    public static class Builder<T> {
        private boolean status = false; // Default status to false
        private String message = null;
        private T data = null;
        private Object extra = null; // Default extra to null

        public Builder<T> status(boolean status) {
            this.status = status;
            return this; // Return builder for chaining
        }

        public Builder<T> message(String message) {
            this.message = message;
            return this;
        }

        public Builder<T> data(T data) {
            this.data = data;
            return this;
        }

        // Builder method to set the extra field (as Object)
        public Builder<T> extra(Object extra) {
            this.extra = extra;
            return this;
        }

        // Optional: Overload the extra builder method to specifically take Map<String, Object>
        // This can improve type safety when you know extra will be a Map.
        public Builder<T> extra(Map<String, Object> extraMap) {
             this.extra = extraMap; // Assign the Map to the Object field
             return this;
        }

        public ApiResponse<T> build() {
            // You could add validation here before building, e.g., require message on error
            return new ApiResponse<>(this); // Call the private ApiResponse constructor
        }
    }
}