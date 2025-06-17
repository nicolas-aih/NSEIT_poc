package com.example.util;

import org.apache.poi.ss.usermodel.*;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;
import org.springframework.stereotype.Component;
import org.springframework.web.multipart.MultipartFile;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;
import java.util.Map;

@Component
public class ExcelHelper {
    
    public static final String TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    
    public boolean hasExcelFormat(MultipartFile file) {
        return TYPE.equals(file.getContentType());
    }
    
    public byte[] generateExcel(List<Map<String, Object>> data, List<String> headers) throws IOException {
        try (Workbook workbook = new XSSFWorkbook();
             ByteArrayOutputStream out = new ByteArrayOutputStream()) {
            
            Sheet sheet = workbook.createSheet("Data");
            
            // Create header row
            Row headerRow = sheet.createRow(0);
            for (int i = 0; i < headers.size(); i++) {
                Cell cell = headerRow.createCell(i);
                cell.setCellValue(headers.get(i));
            }
            
            // Create data rows
            int rowIdx = 1;
            for (Map<String, Object> rowData : data) {
                Row row = sheet.createRow(rowIdx++);
                int colIdx = 0;
                for (String header : headers) {
                    Cell cell = row.createCell(colIdx++);
                    Object value = rowData.get(header);
                    if (value != null) {
                        if (value instanceof Number) {
                            cell.setCellValue(((Number) value).doubleValue());
                        } else {
                            cell.setCellValue(value.toString());
                        }
                    }
                }
            }
            
            // Auto-size columns
            for (int i = 0; i < headers.size(); i++) {
                sheet.autoSizeColumn(i);
            }
            
            workbook.write(out);
            return out.toByteArray();
        }
    }
    
    public List<Map<String, String>> readExcel(MultipartFile file) throws IOException {
        List<Map<String, String>> data = new ArrayList<>();
        
        try (Workbook workbook = WorkbookFactory.create(file.getInputStream())) {
            Sheet sheet = workbook.getSheetAt(0);
            Iterator<Row> rows = sheet.iterator();
            
            if (rows.hasNext()) {
                Row headerRow = rows.next();
                List<String> headers = new ArrayList<>();
                headerRow.forEach(cell -> headers.add(cell.getStringCellValue()));
                
                while (rows.hasNext()) {
                    Row currentRow = rows.next();
                    Map<String, String> rowData = new java.util.HashMap<>();
                    
                    for (int i = 0; i < headers.size(); i++) {
                        Cell cell = currentRow.getCell(i);
                        String value = "";
                        if (cell != null) {
                            switch (cell.getCellType()) {
                                case STRING:
                                    value = cell.getStringCellValue();
                                    break;
                                case NUMERIC:
                                    value = String.valueOf(cell.getNumericCellValue());
                                    break;
                                case BOOLEAN:
                                    value = String.valueOf(cell.getBooleanCellValue());
                                    break;
                                default:
                                    value = "";
                            }
                        }
                        rowData.put(headers.get(i), value);
                    }
                    data.add(rowData);
                }
            }
        }
        
        return data;
    }
} 