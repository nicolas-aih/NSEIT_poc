package com.example.util;

import com.itextpdf.text.*;
import com.itextpdf.text.pdf.*;
import org.springframework.stereotype.Component;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

@Component
public class PDFGenerator {
    
    private static final Font TITLE_FONT = new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD);
    private static final Font HEADER_FONT = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD);
    private static final Font NORMAL_FONT = new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL);
    
    public byte[] generateExamReport(String urn, String applicantName, String examRollNumber, 
                                   LocalDateTime examDate, Integer marks, String result) throws DocumentException, IOException {
        Document document = new Document();
        ByteArrayOutputStream out = new ByteArrayOutputStream();
        
        try {
            PdfWriter.getInstance(document, out);
            document.open();
            
            // Add title
            Paragraph title = new Paragraph("Exam Report", TITLE_FONT);
            title.setAlignment(Element.ALIGN_CENTER);
            title.setSpacingAfter(20);
            document.add(title);
            
            // Add content
            PdfPTable table = new PdfPTable(2);
            table.setWidthPercentage(100);
            
            addTableRow(table, "URN", urn);
            addTableRow(table, "Applicant Name", applicantName);
            addTableRow(table, "Exam Roll Number", examRollNumber);
            addTableRow(table, "Exam Date", examDate.format(DateTimeFormatter.ofPattern("dd/MM/yyyy HH:mm")));
            addTableRow(table, "Marks Obtained", String.valueOf(marks));
            addTableRow(table, "Result", result);
            
            document.add(table);
            
            // Add footer
            Paragraph footer = new Paragraph("Generated on: " + 
                LocalDateTime.now().format(DateTimeFormatter.ofPattern("dd/MM/yyyy HH:mm")), 
                NORMAL_FONT);
            footer.setAlignment(Element.ALIGN_RIGHT);
            document.add(footer);
            
        } finally {
            document.close();
        }
        
        return out.toByteArray();
    }
    
    private void addTableRow(PdfPTable table, String label, String value) {
        PdfPCell labelCell = new PdfPCell(new Phrase(label, HEADER_FONT));
        labelCell.setBorder(Rectangle.NO_BORDER);
        labelCell.setPadding(5);
        
        PdfPCell valueCell = new PdfPCell(new Phrase(value, NORMAL_FONT));
        valueCell.setBorder(Rectangle.NO_BORDER);
        valueCell.setPadding(5);
        
        table.addCell(labelCell);
        table.addCell(valueCell);
    }
} 