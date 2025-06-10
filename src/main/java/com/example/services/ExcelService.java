// File: src/main/java/com/yourcompany/services/ExcelService.java
package com.example.services;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.Date;
import java.util.List; // To get ServletContext
import java.util.Map;

import org.apache.poi.ss.usermodel.Cell;
import org.apache.poi.ss.usermodel.Row;
import org.apache.poi.ss.usermodel.Sheet;
import org.apache.poi.ss.usermodel.Workbook;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;
import org.springframework.stereotype.Service;
import org.springframework.web.context.ServletContextAware;

import jakarta.servlet.ServletContext; // For handling Date type in Excel

// Service to handle Excel writing using Apache POI (replaces XLXporter)
// Implements ServletContextAware to get the ServletContext for path mapping
@Service // Mark as a Spring service
public class ExcelService implements ServletContextAware {

     private ServletContext servletContext;

     @Override
     public void setServletContext(ServletContext servletContext) {
         this.servletContext = servletContext;
     }


    // Replicates the C# XLXporter.WriteExcel signature roughly
    // C# DataTable -> Java List<Map<String, Object>>
    public void writeExcel(String filePath, List<Map<String, Object>> dataTable, String[] displayColumns, String[] displayHeaders, String[] displayFormats) throws IOException {
        System.out.println("ExcelService.writeExcel called for file: " + filePath);

        if (dataTable == null || dataTable.isEmpty()) {
            System.out.println("No data to write to Excel.");
            // Optionally create an empty file or just return
             // Create a dummy file to ensure the link is still valid
             File dummyFile = new File(filePath);
             if (!dummyFile.exists()) {
                 dummyFile.getParentFile().mkdirs(); // Ensure parent dirs exist
                 dummyFile.createNewFile();
                 System.out.println("Created empty placeholder Excel file: " + filePath);
             }
            return;
        }

        try (Workbook workbook = new XSSFWorkbook()) { // Use XSSFWorkbook for .xlsx
            Sheet sheet = workbook.createSheet("Sheet1"); // Use a default sheet name

            // Create Header Row
            Row headerRow = sheet.createRow(0);
            for (int i = 0; i < displayHeaders.length; i++) {
                Cell cell = headerRow.createCell(i);
                cell.setCellValue(displayHeaders[i]);
            }

            // Create Data Rows
            int rowNum = 1;
            for (Map<String, Object> rowData : dataTable) {
                Row row = sheet.createRow(rowNum++);
                for (int i = 0; i < displayColumns.length; i++) {
                    String columnName = displayColumns[i];
                    Object value = rowData.get(columnName); // Get value using column name
                    Cell cell = row.createCell(i);

                    // Basic value setting based on common types
                    if (value != null) {
                        if (value instanceof String) {
                            cell.setCellValue((String) value);
                        } else if (value instanceof Number) {
                            cell.setCellValue(((Number) value).doubleValue()); // Use double for numbers
                        } else if (value instanceof Boolean) {
                            cell.setCellValue((Boolean) value);
                        } else if (value instanceof Date) {
                            // Needs proper date formatting based on displayFormats if necessary
                            // For now, setting as date will use Excel's default date format
                            cell.setCellValue((Date) value);
                        } else {
                             cell.setCellValue(value.toString()); // Fallback to string
                        }
                    } else {
                        cell.setCellValue(""); // Set empty string for nulls
                    }
                }
            }

            // Write the workbook to the file system
            File outputFile = new File(filePath);
             File parentDir = outputFile.getParentFile();
             if (parentDir != null && !parentDir.exists()) {
                 Files.createDirectories(Paths.get(parentDir.getAbsolutePath())); // Create parent directories
             }

            try (FileOutputStream fileOut = new FileOutputStream(outputFile)) {
                workbook.write(fileOut);
            }
             System.out.println("Excel file written successfully: " + filePath);

        } catch (IOException e) {
            System.err.println("Error writing Excel file: " + e.getMessage());
            throw e; // Re-throw the exception for the controller to handle
        }
    }

    // Method to get the real server file system path from a web path (replaces Server.MapPath)
    // This requires the ServletContext, which is injected via ServletContextAware
     public String getServerFilePath(String webPath) {
        // Basic implementation for translating web paths like "../downloads" or "/Uploads/"
        // This needs careful handling depending on your exact deployment structure and
        // whether the target directories are within or outside the web application archive.

        // For paths relative to webapp root (like "/uploads/", "/downloads/"), getRealPath works.
        // Note: webPath might need to be resolved relative to the context root if not starting with /
         if (servletContext == null) {
             System.err.println("ServletContext is null. Cannot resolve path: " + webPath);
              // Fallback: use a path relative to the application's current working directory or temp dir
              // This is NOT recommended for production web apps but works for basic testing.
              String fallbackPath = System.getProperty("java.io.tmpdir") + File.separator + "webapp_files" + webPath.replace("../", "").replace("/", File.separator).replace("\\", File.separator);
              new File(fallbackPath).getParentFile().mkdirs(); // Ensure parent dirs exist
              System.out.println("Using fallback path for '" + webPath + "': " + fallbackPath);
              return fallbackPath;
         }


         // Handle "../downloads" - this is tricky as it implies a path *outside* the web root.
         // ServletContext.getRealPath is often null or unreliable for paths outside the war/jar.
         // A robust solution involves configuring these directories externally.
         // For this translation, let's make an assumption: Server.MapPath("../downloads")
         // might map to a location sibling to the web application root in the file system.
         // Or, perhaps the C# application *did* place downloads inside the web root but relative to a specific page path.
         // Let's assume webPath starting with / are absolute within the webapp, and "../" implies navigating up from the webapp root.
         // A better approach is to define upload/download locations in application.properties.
         // For now, let's simulate getRealPath, but be aware this might need refinement.
         String realPath = servletContext.getRealPath(webPath);

         if (realPath == null) {
             // getRealPath can be null in packaged JARs or for paths outside the web root.
             // If webPath is like "../downloads", this is likely the case.
             // You need a configurable path here. Let's simulate a path relative to the temp dir or current dir as a fallback.
              System.err.println("ServletContext.getRealPath returned null for: " + webPath + ". Using a fallback path.");
              String relativeAdjustedPath = webPath.replace("../", "").replace("/", File.separator).replace("\\", File.separator);
              String fallbackPath = System.getProperty("java.io.tmpdir") + File.separator + "web_mapping_fallback" + File.separator + relativeAdjustedPath;
              new File(fallbackPath).getParentFile().mkdirs(); // Ensure parent dirs exist
              System.out.println("Using fallback path for '" + webPath + "': " + fallbackPath);
              return fallbackPath;

         }
         System.out.println("Mapped web path '" + webPath + "' to file system path: " + realPath);
         return realPath;
     }
}