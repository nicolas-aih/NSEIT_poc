package utilities.fileparser;

import java.util.List;
import java.util.Map;
import java.util.ArrayList; // For dummy data
import java.util.HashMap;   // For dummy data

// Stub for ExcelParser
public class ExcelParser {

    // Stub: Simulates parsing an Excel file and returning data as List<Map<String, Object>>
    // C# DataTable -> Java List<Map<String, Object>>
    public List<Map<String, Object>> parseAllAsData(String filePath, String sheetName, boolean firstRowIsHeader) {
        System.out.println("ExcelParser.parseAllAsData called for file: " + filePath + ", sheet: " + sheetName);
        // --- STUB IMPLEMENTATION ---
        // In a real app, use Apache POI to read the Excel file
        // This stub returns the same dummy data as IIIDL.ExamRegistrations.getExcelData
        // to keep the example consistent. In a real scenario, IIIDL would likely
        // *not* read the file itself, the BLL/Utility would.
        List<Map<String, Object>> dummyData = new ArrayList<>();
         try {
            // Simulate reading data from Sheet1$
            Map<String, Object> row1 = new HashMap<>();
            row1.put("IRDA URN", "URN123");
            row1.put("Payment Mode", "Online Payment");
            row1.put("Insurer Remark", "Remark A");
            row1.put("Enrollment No", "ENR456");
            row1.put("OnOrAfterDate", "15-Dec-2023");
            row1.put("EmailIds", "user1@example.com,user2@example.com"); // Ensure comma separated for split
            row1.put("Batch Mode", "ModeB");
            row1.put("Scheduling Mode", "ModeS");
             row1.put("ExtraColumn", "ShouldBeRemoved"); // Keep extra columns initially
            dummyData.add(row1);

            Map<String, Object> row2 = new HashMap<>();
            row2.put("IRDA URN", "URN789");
            row2.put("Payment Mode", "Credit");
            row2.put("Insurer Remark", "");
            row2.put("Enrollment No", "ENR012");
            row2.put("OnOrAfterDate", "01-Jan-2024");
            row2.put("EmailIds", "user3@example.com,invalid-email");
            row2.put("Batch Mode", "ModeB");
            row2.put("Scheduling Mode", "ModeS");
             dummyData.add(row2);

             Map<String, Object> row3 = new HashMap<>();
            row3.put("IRDA URN", "URNXYZ");
            row3.put("Payment Mode", "Credit");
            row3.put("Insurer Remark", "");
            row3.put("Enrollment No", "ENRABC");
            row3.put("OnOrAfterDate", "01-Jan-2025");
            row3.put("EmailIds", "user4@example.com");
            row3.put("Batch Mode", "ModeB");
            row3.put("Scheduling Mode", "ModeS");
             dummyData.add(row3);

         } catch (Exception ex) {
              System.err.println("Stub ExcelParser failed: " + ex.getMessage());
         }
         // --- END STUB IMPLEMENTATION ---
        return dummyData;
    }
}