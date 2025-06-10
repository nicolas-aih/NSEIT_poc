// File: src/main/java/com/yourcompany/web/response/ApiResponse.java
package com.example.web.response;

import java.util.HashMap;
import java.util.Map;

// Represents the standard JSON response format expected by the client JS
// Use boolean for Status for better representation in Java
public class ApiResponse<T> {
    private boolean _STATUS_;
    private String _MESSAGE_;
    private T _DATA_;
    private Map<String, Object> _EXTRA_; // For KVPairs like PaymentMode, _RESPONSE_FILE_

    // Private constructor to enforce using the builder
    private ApiResponse() {}

    // Builder pattern for flexible object creation
    public static class Builder<T> {
        private boolean status = true; // Default status success
        private String message = "";
        private T data;
        private Map<String, Object> extra;

        public Builder<T> status(boolean status) {
            this.status = status;
            return this;
        }

        public Builder<T> message(String message) {
            this.message = message;
            return this;
        }

        public Builder<T> data(T data) {
            this.data = data;
            return this;
        }

         // Method to set the entire extra map
         public Builder<T> extra(Map<String, Object> extra) {
             this.extra = extra;
             return this;
         }

        // Method to add individual items to the extra map
        public Builder<T> addExtra(String key, Object value) {
            if (this.extra == null) {
                this.extra = new HashMap<>();
            }
            this.extra.put(key, value);
            return this;
        }

        public ApiResponse<T> build() {
            ApiResponse<T> response = new ApiResponse<>();
            response._STATUS_ = this.status;
            response._MESSAGE_ = this.message;
            response._DATA_ = this.data;
            response._EXTRA_ = this.extra;
            return response;
        }
    }

    // Getters (for Jackson serialization)
    public boolean is_STATUS_() { return _STATUS_; }
    public String get_MESSAGE_() { return _MESSAGE_; }
    public T get_DATA_() { return _DATA_; }
    public Map<String, Object> get_EXTRA_() { return _EXTRA_; }

    // No setters needed if using builder pattern
}