package com.example;

import com.example.model.ExamCenters;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;

public class Main {
    public static void main(String[] args) {
        // Replace with your actual connection string
        String connectionString = "jdbc:sqlserver://localhost:1433;";
        
        ExamCenters examCenters = new ExamCenters();
        
        try {
            // Example 1: Find nearest exam center
            System.out.println("Finding nearest exam centers...");
            ResultSet rs = examCenters.findNearestExamCenter(connectionString, 400001, 5);
            printResultSet(rs);
            
            // Example 2: Save center details
            System.out.println("\nSaving exam center...");
            String result = examCenters.saveCenterDetails(
                connectionString, 
                123,               // centerId
                "Test Center",     // centerName
                "TC001",           // centerCode
                1,                 // cssCode
                5,                 // examBody
                "123 Main St",     // addressLine1
                "Downtown",        // addressLine2
                "Mumbai",         // addressLine3
                1,                 // districtId
                400001,            // pincode
                true,              // isActive
                "REGULAR",         // centerType
                1                  // currentUser
            );
            System.out.println("Save result: " + result);
            
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
    
    private static void printResultSet(ResultSet rs) throws Exception {
        if (rs == null) {
            System.out.println("No results");
            return;
        }
        
        ResultSetMetaData metaData = rs.getMetaData();
        int columnCount = metaData.getColumnCount();
        
        // Print column headers
        for (int i = 1; i <= columnCount; i++) {
            System.out.print(metaData.getColumnName(i) + "\t");
        }
        System.out.println();
        
        // Print data rows
        while (rs.next()) {
            for (int i = 1; i <= columnCount; i++) {
                System.out.print(rs.getString(i) + "\t");
            }
            System.out.println();
        }
        
        rs.close();
    }
}