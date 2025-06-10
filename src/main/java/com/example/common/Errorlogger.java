// File: src/main/java/com/yourcompany/common/Errorlogger.java
package com.example.common;

import java.util.Map;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory; // To get method name

// Using SLF4j as a standard Java logging facade
public class Errorlogger {

    private static Logger getLogger(String name) {
        return LoggerFactory.getLogger(name);
    }

    // Replicates the C# signature roughly
    public static void logError(String className, String methodName, Exception ex, Object context) {
        Logger logger = getLogger(className);
        // Format context based on type if known, otherwise use toString
        String contextString = "N/A";
        if (context instanceof Map) {
             contextString = "Parameters: " + parametersToString((Map<String, String[]>) context);
        } else if (context != null) {
             contextString = context.toString();
        }
        logger.error("Error in {}.{}(): {}\nContext: {}", className, methodName, ex.getMessage(), contextString, ex);
    }

     // Helper to convert parameter map to string for logging context
     private static String parametersToString(Map<String, String[]> parameterMap) {
         if (parameterMap == null) return "{}";
         StringBuilder sb = new StringBuilder();
         sb.append("{");
         boolean first = true;
         for (Map.Entry<String, String[]> entry : parameterMap.entrySet()) {
             if (!first) sb.append(", ");
             sb.append("\"").append(entry.getKey()).append("\": ");
             if (entry.getValue() == null) {
                 sb.append("null");
             } else if (entry.getValue().length == 1) {
                 sb.append("\"").append(entry.getValue()[0]).append("\""); // Simple value
             } else {
                  sb.append(java.util.Arrays.toString(entry.getValue())); // Array value
             }
             first = false;
         }
         sb.append("}");
         return sb.toString();
     }

    // Add other logging methods (Warn, Info, Debug) if needed
}