// File: src/main/java/com/yourcompany/common/PortalApplicationStub.java
package com.example.common;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Component;

import jakarta.annotation.PostConstruct;

// This should be a Spring Bean (e.g., singleton)
@Component
public class PortalApplicationStub {

    @Value("${app.connection-string:dummy_connection_string}") // Get from properties, provide default
    private String connectionString;

    private static String appConnectionString;

    @PostConstruct
    public void init() {
        // Set the static field using the value injected by Spring
        appConnectionString = this.connectionString;
        System.out.println("PortalApplicationStub initialized. Connection String: " + connectionString);
    }

    public static String getConnectionString() {
        // In a real app, you'd get this from a proper config or DataSource bean
        return appConnectionString;
    }

    // Add other application-level properties/methods as needed
}