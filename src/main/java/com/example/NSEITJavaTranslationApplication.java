package com.example; // <-- Make sure this matches your com/example directory

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.autoconfigure.domain.EntityScan;
import org.springframework.context.annotation.ComponentScan;
import org.springframework.data.jpa.repository.config.EnableJpaRepositories;

// @SpringBootApplication is a convenience annotation that adds:
// @Configuration
// @EnableAutoConfiguration (sets up Spring Boot based on classpath dependencies)
// @ComponentScan (scans the current package and sub-packages for Spring components like @RestController, @Service, @Component)
@SpringBootApplication
// Optional: If your BLL/DAL/Helper classes are outside the com.example package (like iiibl, iiidl, databases, utilities),
// you might need to explicitly tell Spring to scan those packages as well, if you want Spring
// to manage them as beans (e.g., inject BLLs into the controller).
// Since our controller instantiates BLLs with 'new', you might not need this *yet*,
// but it's good practice if you move towards Spring-managed BLLs/DALs.
// If your BLL/DAL are @Service or @Repository, uncomment and adjust basePackages:
//@ComponentScan(basePackages = {"com.example", "iiibl", "iiidl", "databases.sqlserver", "utilities"})
@ComponentScan(basePackages = "com.example")
@EntityScan("com.example.model")
@EnableJpaRepositories("com.example.repository")
public class NSEITJavaTranslationApplication { // Use your chosen application name

    public static void main(String[] args) {
        // This is the standard way to launch a Spring Boot application
        SpringApplication.run(NSEITJavaTranslationApplication.class, args);
    }

}