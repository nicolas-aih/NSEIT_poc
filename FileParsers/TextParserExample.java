import java.io.*;
import java.text.SimpleDateFormat;
import java.util.*;
import java.util.regex.Pattern;

// Field class to represent field definitions
class Field {
    private String name;
    private String displayName;
    private Class<?> datatype;
    private int ordinal;
    private int length;
    private boolean allowNull;
    private String pattern;
    private String format;
    
    public Field(String name, String displayName, Class<?> datatype, int ordinal, 
                 int length, boolean allowNull, String pattern, String format) {
        this.name = name;
        this.displayName = displayName;
        this.datatype = datatype;
        this.ordinal = ordinal;
        this.length = length;
        this.allowNull = allowNull;
        this.pattern = pattern;
        this.format = format;
    }
    
    // Getters
    public String getName() { return name; }
    public String getDisplayName() { return displayName; }
    public Class<?> getDatatype() { return datatype; }
    public int getOrdinal() { return ordinal; }
    public int getLength() { return length; }
    public boolean isAllowNull() { return allowNull; }
    public String getPattern() { return pattern; }
    public String getFormat() { return format; }
}

// Text validator class
class TextValidator {
    public static boolean validate(String value, String pattern) {
        if (pattern == null || pattern.isEmpty()) {
            return true;
        }
        try {
            return Pattern.matches(pattern, value);
        } catch (Exception e) {
            return false;
        }
    }
}

// Functional interface for status updates
@FunctionalInterface
interface UpdateStatusListener {
    void updateStatus(String fileFormatCode, String fileName, String message, int value);
}

// Parse result class
class ParseResult {
    private List<Map<String, Object>> records;
    private List<String> headers;
    private List<String> footers;
    private String message;
    
    public ParseResult(List<Map<String, Object>> records, List<String> headers, List<String> footers) {
        this.records = records;
        this.headers = headers;
        this.footers = footers;
        this.message = "";
    }
    
    public ParseResult(List<Map<String, Object>> records, List<String> headers, String message) {
        this.records = records;
        this.headers = headers;
        this.footers = null;
        this.message = message;
    }
    
    // Getters
    public List<Map<String, Object>> getRecords() { return records; }
    public List<String> getHeaders() { return headers; }
    public List<String> getFooters() { return footers; }
    public String getMessage() { return message; }
}

// Main TextParser class
class TextParser {
    private UpdateStatusListener updateStatusListener;
    
    public TextParser() {
    }
    
    public void setUpdateStatusListener(UpdateStatusListener listener) {
        this.updateStatusListener = listener;
    }
    
    protected void updateUploadStatus(String fileFormatCode, String fileName, String message, int value) {
        if (updateStatusListener != null) {
            updateStatusListener.updateStatus(fileFormatCode, fileName, message, value);
        }
    }
    
    public ParseResult parse(String fileName, int headerRowCount, boolean isFixedWidth, 
                           String delimiter, Field[] fields, int footerRowCount) throws Exception {
        List<Map<String, Object>> records = new ArrayList<>();
        List<String> headers = new ArrayList<>();
        List<String> footers = new ArrayList<>();
        int totalCount = 0;
        int count = 0;
        
        File file = new File(fileName);
        if (!file.exists()) {
            throw new Exception("File Not Found");
        }
        
        // Count total lines
        try (BufferedReader reader = new BufferedReader(new FileReader(file))) {
            while (reader.readLine() != null) {
                totalCount++;
            }
        }
        
        if (totalCount == 0) {
            throw new Exception("Empty File : Zero Rows Found In File");
        }
        
        if (totalCount < (headerRowCount + footerRowCount)) {
            throw new Exception("Invalid File : The Total Number Of Rows Is Less Than The Sum Of Header Row Count And Footer Row Count");
        }
        
        if (totalCount == (headerRowCount + footerRowCount)) {
            throw new Exception("Empty File : Zero Processable Rows Found In File.\nTotal Row Count Equals Sum Of Header Row Count And Footer Row Count");
        }
        
        // Parse file
        try (BufferedReader reader = new BufferedReader(new FileReader(file))) {
            if (!isFixedWidth && "\\t".equals(delimiter)) {
                delimiter = "\t";
            }
            
            String line;
            int rowNum = 0;
            
            while ((line = reader.readLine()) != null) {
                rowNum++;
                count++;
                
                if (line.trim().isEmpty()) {
                    continue;
                }
                
                if (count <= headerRowCount) {
                    headers.add(line);
                }
                if (count > (totalCount - footerRowCount)) {
                    footers.add(line);
                }
                if (count > headerRowCount && count <= (totalCount - footerRowCount)) {
                    Map<String, Object> record = new HashMap<>();
                    record.put("ROW_ID", rowNum);
                    record.put("ORIGINAL_ROW", line);
                    
                    StringBuilder error = new StringBuilder();
                    
                    if (isFixedWidth) {
                        processFixedWidth(line, fields, record, error, rowNum);
                    } else {
                        processDelimited(line, delimiter, fields, record, error, rowNum);
                    }
                    
                    record.put("ERROR_FLAG", error.length() == 0 ? "N" : "Y");
                    record.put("ERROR", error.toString().replaceAll("/$", ""));
                    
                    records.add(record);
                }
            }
        }
        
        return new ParseResult(records, headers, footers);
    }
    
    public ParseResult parse2(String fileName, int headerRowCount, boolean isFixedWidth,
                            String delimiter, Field[] fields, List<String> expectedHeaders) throws Exception {
        List<Map<String, Object>> records = new ArrayList<>();
        List<String> headers = new ArrayList<>();
        String message = "";
        int totalCount = 0;
        int count = 0;
        
        if (headerRowCount != expectedHeaders.size()) {
            throw new IllegalArgumentException("The HeaderRowCount and no of elements in ExpectedHeaders do not match");
        }
        
        File file = new File(fileName);
        if (!file.exists()) {
            return new ParseResult(records, headers, "File Not Found");
        }
        
        // Count total lines
        try (BufferedReader reader = new BufferedReader(new FileReader(file))) {
            while (reader.readLine() != null) {
                totalCount++;
            }
        }
        
        if (totalCount == 0) {
            return new ParseResult(records, headers, "Empty File : Zero Rows Found In File");
        }
        
        if (totalCount < headerRowCount) {
            return new ParseResult(records, headers, "Invalid File : The Total Number Of Rows Is Less Than The Header Row Count");
        }
        
        if (totalCount == headerRowCount) {
            return new ParseResult(records, headers, "Empty File : Zero Processable Rows Found In File. Total Row Count Equals the Header Row Count");
        }
        
        // Parse file
        try (BufferedReader reader = new BufferedReader(new FileReader(file))) {
            if (!isFixedWidth && "\\t".equals(delimiter)) {
                delimiter = "\t";
            }
            
            String line;
            int rowNum = 0;
            
            while ((line = reader.readLine()) != null) {
                rowNum++;
                count++;
                
                if (count <= headerRowCount) {
                    headers.add(line);
                    continue;
                }
                
                Map<String, Object> record = new HashMap<>();
                record.put("ROW_ID", rowNum);
                record.put("ORIGINAL_ROW", line);
                
                if (line.trim().isEmpty()) {
                    record.put("ERROR_FLAG", "Y");
                    record.put("MESSAGE", "Empty Row Found");
                    records.add(record);
                    continue;
                }
                
                StringBuilder error = new StringBuilder();
                
                if (isFixedWidth) {
                    processFixedWidth(line, fields, record, error, rowNum);
                } else {
                    processDelimited(line, delimiter, fields, record, error, rowNum);
                }
                
                record.put("ERROR_FLAG", error.length() == 0 ? "N" : "Y");
                record.put("MESSAGE", error.toString().trim());
                
                records.add(record);
            }
            
            // Check expected headers
            for (int i = 0; i < expectedHeaders.size(); i++) {
                if (i >= headers.size() || !expectedHeaders.get(i).equals(headers.get(i))) {
                    message = "Expected File Header(s) Not Found. Please check the template";
                    break;
                }
            }
        }
        
        return new ParseResult(records, headers, message);
    }
    
    private void processFixedWidth(String line, Field[] fields, Map<String, Object> record, 
                                 StringBuilder error, int rowNum) throws Exception {
        for (Field field : fields) {
            if (field.getOrdinal() + field.getLength() > line.length()) {
                if (error.length() > 0) error.append(" ");
                error.append("Invalid File Format: Field Not Found : ").append(field.getName()).append(".");
                continue;
            }
            
            String fieldValue = line.substring(field.getOrdinal(), field.getOrdinal() + field.getLength()).trim();
            processFieldValue(field, fieldValue, record, error);
        }
    }
    
    private void processDelimited(String line, String delimiter, Field[] fields, 
                                Map<String, Object> record, StringBuilder error, int rowNum) throws Exception {
        String[] parts = line.split(Pattern.quote(delimiter));
        
        for (Field field : fields) {
            if (field.getOrdinal() >= parts.length) {
                if (error.length() > 0) error.append(" ");
                error.append("Invalid File Format: Field Not Found : ").append(field.getName()).append(".");
                continue;
            }
            
            String fieldValue = parts[field.getOrdinal()].trim();
            processFieldValue(field, fieldValue, record, error);
        }
    }
    
    private void processFieldValue(Field field, String fieldValue, Map<String, Object> record, StringBuilder error) {
        if (fieldValue.isEmpty() || "-".equals(fieldValue)) {
            if (field.isAllowNull()) {
                record.put(field.getName(), null);
            } else {
                if (error.length() > 0) error.append(" ");
                error.append("Field Cannot Be Blank - ").append(field.getDisplayName()).append(".");
            }
            return;
        }
        
        try {
            if (field.getDatatype() == Date.class) {
                if (TextValidator.validate(fieldValue.toLowerCase(), field.getPattern())) {
                    SimpleDateFormat sdf = new SimpleDateFormat(field.getFormat());
                    record.put(field.getName(), sdf.parse(fieldValue.toUpperCase()));
                } else {
                    if (error.length() > 0) error.append(" ");
                    error.append("Invalid Value For ").append(field.getName()).append(":").append(fieldValue).append(".");
                }
            } else if (field.getDatatype() == String.class) {
                record.put(field.getName(), fieldValue);
            } else if (isNumericType(field.getDatatype())) {
                fieldValue = fieldValue.replace(",", "");
                if (TextValidator.validate(fieldValue, field.getPattern())) {
                    Object convertedValue = convertToNumericType(fieldValue, field.getDatatype());
                    record.put(field.getName(), convertedValue);
                } else {
                    if (error.length() > 0) error.append(" ");
                    error.append("Invalid Value For ").append(field.getName()).append(":").append(fieldValue).append(".");
                }
            }
        } catch (Exception e) {
            if (error.length() > 0) error.append(" ");
            error.append("Invalid Value For ").append(field.getName()).append(":").append(fieldValue).append(".");
        }
    }
    
    private boolean isNumericType(Class<?> type) {
        return type == Integer.class || type == Long.class || type == Double.class || 
               type == Float.class || type == Short.class;
    }
    
    private Object convertToNumericType(String value, Class<?> type) throws Exception {
        if (type == Integer.class) return Integer.parseInt(value);
        if (type == Long.class) return Long.parseLong(value);
        if (type == Double.class) return Double.parseDouble(value);
        if (type == Float.class) return Float.parseFloat(value);
        if (type == Short.class) return Short.parseShort(value);
        throw new Exception("Unsupported numeric type: " + type.getName());
    }
    
    public void processDelimited(String line, String delimiter, Field[] fields, 
                               List<Map<String, Object>> records, int rowNum) {
        Map<String, Object> record = new HashMap<>();
        record.put("ROW_ID", rowNum);
        record.put("ORIGINAL_ROW", line);
        
        String[] parts = line.split(Pattern.quote(delimiter));
        StringBuilder error = new StringBuilder();
        
        for (Field field : fields) {
            if (field.getOrdinal() >= parts.length) {
                if (error.length() > 0) error.append("/");
                error.append("Invalid File Format: Field Not Found : ").append(field.getName());
                continue;
            }
            
            String fieldValue = parts[field.getOrdinal()].trim();
            processFieldValueForPublicMethod(field, fieldValue, record, error);
        }
        
        record.put("ERROR_FLAG", error.length() == 0 ? "N" : "Y");
        record.put("ERROR", error.toString());
        records.add(record);
    }
    
    public void processFixedWidth(String line, Field[] fields, 
                                List<Map<String, Object>> records, int rowNum) {
        Map<String, Object> record = new HashMap<>();
        record.put("ROW_ID", rowNum);
        record.put("ORIGINAL_ROW", line);
        
        StringBuilder error = new StringBuilder();
        
        for (Field field : fields) {
            if (field.getOrdinal() + field.getLength() > line.length()) {
                if (error.length() > 0) error.append("/");
                error.append("Invalid File Format: Field Not Found : ").append(field.getName());
                continue;
            }
            
            String fieldValue = line.substring(field.getOrdinal(), field.getOrdinal() + field.getLength()).trim();
            processFieldValueForPublicMethod(field, fieldValue, record, error);
        }
        
        record.put("ERROR_FLAG", error.length() == 0 ? "N" : "Y");
        record.put("ERROR", error.toString());
        records.add(record);
    }
    
    private void processFieldValueForPublicMethod(Field field, String fieldValue, Map<String, Object> record, StringBuilder error) {
        if (fieldValue.isEmpty() || "-".equals(fieldValue)) {
            if (field.isAllowNull()) {
                record.put(field.getName(), null);
            } else {
                if (error.length() > 0) error.append("/");
                error.append("Field Cannot Be Blank - ").append(field.getDisplayName());
            }
            return;
        }
        
        try {
            if (field.getDatatype() == Date.class) {
                if (TextValidator.validate(fieldValue.toLowerCase(), field.getPattern())) {
                    SimpleDateFormat sdf = new SimpleDateFormat(field.getFormat());
                    record.put(field.getName(), sdf.parse(fieldValue.toUpperCase()));
                } else {
                    if (error.length() > 0) error.append("/");
                    error.append("Invalid Value For ").append(field.getName()).append(":").append(fieldValue);
                }
            } else if (field.getDatatype() == String.class) {
                record.put(field.getName(), fieldValue);
            } else if (isNumericType(field.getDatatype())) {
                fieldValue = fieldValue.replace(",", "");
                if (TextValidator.validate(fieldValue, field.getPattern())) {
                    Object convertedValue = convertToNumericType(fieldValue, field.getDatatype());
                    record.put(field.getName(), convertedValue);
                } else {
                    if (error.length() > 0) error.append("/");
                    error.append("Invalid Value For ").append(field.getName()).append(":").append(fieldValue);
                }
            }
        } catch (Exception e) {
            if (error.length() > 0) error.append("/");
            error.append("Invalid Value For ").append(field.getName()).append(":").append(fieldValue);
        }
    }
}

// Main class with console application for testing
public class TextParserExample {
    private static Scanner scanner = new Scanner(System.in);
    private static TextParser parser = new TextParser();
    
    public static void main(String[] args) {
        System.out.println("=== Text Parser Test Application ===");
        
        parser.setUpdateStatusListener((fileFormatCode, fileName, message, value) -> {
            System.out.println("[Status] " + message + " (" + value + "%)");
        });
        
        while (true) {
            showMenu();
            int choice = getIntInput("Enter your choice: ");
            
            switch (choice) {
                case 1:
                    testDelimitedFile();
                    break;
                case 2:
                    testFixedWidthFile();
                    break;
                case 3:
                    testParse2Method();
                    break;
                case 4:
                    testPublicMethods();
                    break;
                case 5:
                    createSampleFiles();
                    break;
                case 6:
                    System.out.println("Goodbye!");
                    return;
                default:
                    System.out.println("Invalid choice. Please try again.");
            }
            
            System.out.println("\nPress Enter to continue...");
            scanner.nextLine();
        }
    }
    
    private static void showMenu() {
        System.out.println("\n--- Menu ---");
        System.out.println("1. Test Delimited File Parsing");
        System.out.println("2. Test Fixed Width File Parsing");
        System.out.println("3. Test Parse2 Method (with header validation)");
        System.out.println("4. Test Public Processing Methods");
        System.out.println("5. Create Sample Files");
        System.out.println("6. Exit");
    }
    
    private static void testDelimitedFile() {
        System.out.println("\n--- Testing Delimited File Parsing ---");
        
        String fileName = getStringInput("Enter file name (or press Enter for 'sample_delimited.csv'): ");
        if (fileName.isEmpty()) fileName = "sample_delimited.csv";
        
        String delimiter = getStringInput("Enter delimiter (or press Enter for ','): ");
        if (delimiter.isEmpty()) delimiter = ",";
        
        int headerRows = getIntInput("Enter number of header rows (default 1): ");
        if (headerRows == 0) headerRows = 1;
        
        int footerRows = getIntInput("Enter number of footer rows (default 0): ");
        
        Field[] fields = createSampleDelimitedFields();
        
        try {
            ParseResult result = parser.parse(fileName, headerRows, false, delimiter, fields, footerRows);
            displayResults(result);
        } catch (Exception e) {
            System.out.println("Error: " + e.getMessage());
        }
    }
    
    private static void testFixedWidthFile() {
        System.out.println("\n--- Testing Fixed Width File Parsing ---");
        
        String fileName = getStringInput("Enter file name (or press Enter for 'sample_fixed.txt'): ");
        if (fileName.isEmpty()) fileName = "sample_fixed.txt";
        
        int headerRows = getIntInput("Enter number of header rows (default 1): ");
        if (headerRows == 0) headerRows = 1;
        
        int footerRows = getIntInput("Enter number of footer rows (default 0): ");
        
        Field[] fields = createSampleFixedWidthFields();
        
        try {
            ParseResult result = parser.parse(fileName, headerRows, true, "", fields, footerRows);
            displayResults(result);
        } catch (Exception e) {
            System.out.println("Error: " + e.getMessage());
        }
    }
    
    private static void testParse2Method() {
        System.out.println("\n--- Testing Parse2 Method ---");
        
        String fileName = getStringInput("Enter file name (or press Enter for 'sample_delimited.csv'): ");
        if (fileName.isEmpty()) fileName = "sample_delimited.csv";
        
        String delimiter = getStringInput("Enter delimiter (or press Enter for ','): ");
        if (delimiter.isEmpty()) delimiter = ",";
        
        List<String> expectedHeaders = Arrays.asList("ID,Name,Age,Salary");
        Field[] fields = createSampleDelimitedFields();
        
        try {
            ParseResult result = parser.parse2(fileName, 1, false, delimiter, fields, expectedHeaders);
            displayResults(result);
            if (!result.getMessage().isEmpty()) {
                System.out.println("Message: " + result.getMessage());
            }
        } catch (Exception e) {
            System.out.println("Error: " + e.getMessage());
        }
    }
    
    private static void testPublicMethods() {
        System.out.println("\n--- Testing Public Processing Methods ---");
        
        List<Map<String, Object>> records = new ArrayList<>();
        Field[] fields = createSampleDelimitedFields();
        
        // Test with sample data
        String[] testLines = {
            "1,John Doe,30,50000.50",
            "2,Jane Smith,25,45000",
            "3,Bob Johnson,,35000.75"
        };
        
        for (int i = 0; i < testLines.length; i++) {
            parser.processDelimited(testLines[i], ",", fields, records, i + 1);
        }
        
        System.out.println("Processed " + records.size() + " records using processDelimited method:");
        for (Map<String, Object> record : records) {
            System.out.println("Row " + record.get("ROW_ID") + ": " + record);
        }
        
        // Test fixed width
        records.clear();
        Field[] fixedFields = createSampleFixedWidthFields();
        String[] fixedTestLines = {
            "1    John Doe            30  50000.50  ",
            "2    Jane Smith          25  45000     "
        };
        
        for (int i = 0; i < fixedTestLines.length; i++) {
            parser.processFixedWidth(fixedTestLines[i], fixedFields, records, i + 1);
        }
        
        System.out.println("\nProcessed " + records.size() + " records using processFixedWidth method:");
        for (Map<String, Object> record : records) {
            System.out.println("Row " + record.get("ROW_ID") + ": " + record);
        }
    }
    
    private static void createSampleFiles() {
        System.out.println("\n--- Creating Sample Files ---");
        
        // Create sample delimited file
        try (PrintWriter writer = new PrintWriter("sample_delimited.csv")) {
            writer.println("ID,Name,Age,Salary");
            writer.println("1,John Doe,30,50000.50");
            writer.println("2,Jane Smith,25,45000");
            writer.println("3,Bob Johnson,,35000.75");
            writer.println("4,Alice Brown,35,");
            writer.println("5,Charlie Wilson,abc,60000");  // Invalid age for testing
            System.out.println("Created sample_delimited.csv");
        } catch (IOException e) {
            System.out.println("Error creating delimited file: " + e.getMessage());
        }
        
        // Create sample fixed width file
        try (PrintWriter writer = new PrintWriter("sample_fixed.txt")) {
            writer.println("ID   Name                Age Salary    ");
            writer.println("1    John Doe            30  50000.50  ");
            writer.println("2    Jane Smith          25  45000     ");
            writer.println("3    Bob Johnson             35000.75  ");
            writer.println("4    Alice Brown         35            ");
            writer.println("5    Charlie Wilson      abc 60000     ");  // Invalid age for testing
            System.out.println("Created sample_fixed.txt");
        } catch (IOException e) {
            System.out.println("Error creating fixed width file: " + e.getMessage());
        }
        
        System.out.println("Sample files created successfully!");
    }
    
    private static Field[] createSampleDelimitedFields() {
        return new Field[] {
            new Field("ID", "ID", Integer.class, 0, 0, false, "\\d+", ""),
            new Field("NAME", "Name", String.class, 1, 0, false, "", ""),
            new Field("AGE", "Age", Integer.class, 2, 0, true, "\\d+", ""),
            new Field("SALARY", "Salary", Double.class, 3, 0, true, "\\d+(\\.\\d+)?", "")
        };
    }
    
    private static Field[] createSampleFixedWidthFields() {
        return new Field[] {
            new Field("ID", "ID", Integer.class, 0, 5, false, "\\d+", ""),
            new Field("NAME", "Name", String.class, 5, 20, false, "", ""),
            new Field("AGE", "Age", Integer.class, 25, 3, true, "\\d+", ""),
            new Field("SALARY", "Salary", Double.class, 28, 10, true, "\\d+(\\.\\d+)?", "")
        };
    }
    
    private static void displayResults(ParseResult result) {
        System.out.println("\n--- Parse Results ---");
        
        System.out.println("Headers (" + result.getHeaders().size() + "):");
        for (int i = 0; i < result.getHeaders().size(); i++) {
            System.out.println("  " + (i + 1) + ": " + result.getHeaders().get(i));
        }
        
        if (result.getFooters() != null && !result.getFooters().isEmpty()) {
            System.out.println("\nFooters (" + result.getFooters().size() + "):");
            for (int i = 0; i < result.getFooters().size(); i++) {
                System.out.println("  " + (i + 1) + ": " + result.getFooters().get(i));
            }
        }
        
        System.out.println("\nTotal records found: " + result.getRecords().size());
        
        // Show error summary first
        long errorCount = result.getRecords().stream()
            .mapToLong(record -> "Y".equals(record.get("ERROR_FLAG")) ? 1 : 0)
            .sum();
        System.out.println("Records with errors: " + errorCount);
        
        // Show first few records
        int displayCount = Math.min(5, result.getRecords().size());
        for (int i = 0; i < displayCount; i++) {
            Map<String, Object> record = result.getRecords().get(i);
            System.out.println("\n--- Record " + (i + 1) + " ---");
            System.out.println("  ROW_ID: " + record.get("ROW_ID"));
            System.out.println("  ERROR_FLAG: " + record.get("ERROR_FLAG"));
            
            if (record.containsKey("ERROR") && record.get("ERROR") != null && !record.get("ERROR").toString().isEmpty()) {
                System.out.println("  ERROR: " + record.get("ERROR"));
            }
            if (record.containsKey("MESSAGE") && record.get("MESSAGE") != null && !record.get("MESSAGE").toString().isEmpty()) {
                System.out.println("  MESSAGE: " + record.get("MESSAGE"));
            }
            
            // Show parsed field values
            for (Map.Entry<String, Object> entry : record.entrySet()) {
                String key = entry.getKey();
                if (!key.equals("ROW_ID") && !key.equals("ERROR_FLAG") && 
                    !key.equals("ERROR") && !key.equals("MESSAGE") && !key.equals("ORIGINAL_ROW")) {
                    System.out.println("  " + key + ": " + entry.getValue());
                }
            }
            System.out.println("  ORIGINAL_ROW: " + record.get("ORIGINAL_ROW"));
        }
        
        if (result.getRecords().size() > displayCount) {
            System.out.println("\n... and " + (result.getRecords().size() - displayCount) + " more records");
        }
    }
    
    private static String getStringInput(String prompt) {
        System.out.print(prompt);
        return scanner.nextLine().trim();
    }
    
    private static int getIntInput(String prompt) {
        while (true) {
            try {
                System.out.print(prompt);
                String input = scanner.nextLine().trim();
                return input.isEmpty() ? 0 : Integer.parseInt(input);
            } catch (NumberFormatException e) {
                System.out.println("Please enter a valid number.");
            }
        }
    }
}