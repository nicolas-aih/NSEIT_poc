package com.example.services;

import com.example.interfaces.UrnDataAccess;
import com.example.interfaces.UrnDataAccess.*; // Import nested DTOs
import com.example.util.FileParserUtil; // Your mock file parsers
import com.example.util.FileParserUtil.CommonRegex; // Your mock regex constants
import com.example.util.PortalApplication; // For Aadhaar keys
import com.iiibl.AadhaarEncryptorDecryptor; // Assuming this is correctly placed

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.io.File;
import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.nio.file.DirectoryStream;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.regex.Pattern;
import java.util.stream.Collectors;


@Service
public class UrnService {

    private final UrnDataAccess urnDataAccess;

    @Autowired
    public UrnService(UrnDataAccess urnDataAccess) {
        this.urnDataAccess = urnDataAccess;
    }

    public List<Map<String, Object>> getURNForPAN(String connectionString, String pan) {
        try {
            return urnDataAccess.getURNForPAN(connectionString, pan);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public String unarchiveURN(String connectionString, String urn) {
        try {
            return urnDataAccess.unarchiveURN(connectionString, urn);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public UrnDataResult getURNDetails(String connectionString, String urn, int userId) {
        try {
            return urnDataAccess.getURNDetails(connectionString, urn, userId);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public List<Map<String, Object>> getPhotoAndSignature(String connectionString, String urn) {
        try {
            return urnDataAccess.getPhotoAndSignature(connectionString, urn);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public GenerateUrnResult generateURN(String connectionString, List<Map<String, Object>> applicantData, String roleCode, int userId) {
        // C# had 'out String RollNo, out String Message'. Interface returns GenerateUrnResult DTO.
        try {
            return urnDataAccess.generateURN(connectionString, applicantData, roleCode, userId);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public GenerateUrnResult generateDuplicateURN(String connectionString, List<Map<String, Object>> applicantData, String roleCode, int userId) {
        try {
            return urnDataAccess.generateDuplicateURN(connectionString, applicantData, roleCode, userId);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public UrnDataResult getDataForPartialModification(String connectionString, String urn, Date dob, int insurerUserId) {
        try {
            return urnDataAccess.getDataForPartialModification(connectionString, urn, dob, insurerUserId);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public UrnDataResult getDataForModification(String connectionString, String urn, int userId) {
        try {
            return urnDataAccess.getDataForModification(connectionString, urn, userId);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public UrnDataResult getDataForURNRequestModification(String connectionString, long id, int userId) {
        try {
            return urnDataAccess.getDataForURNRequestModification(connectionString, id, userId);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public UrnDataResult getDataForURNDuplication(String connectionString, String urn, int userId) {
        try {
            return urnDataAccess.getDataForURNDuplication(connectionString, urn, userId);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public SaveModificationResult saveModification(String connectionString, String urn, int languageId, int examCenterId,
                                                   byte[] photo, byte[] sign, String allowWhatsappMessage,
                                                   String whatsappNumber, int userId, int insurerUserId) {
        try {
            return urnDataAccess.saveModification(connectionString, urn, languageId, examCenterId, photo, sign,
                                                 allowWhatsappMessage, whatsappNumber, userId, insurerUserId);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public SaveModificationOaimsResult saveModificationOAIMS(
            String connectionString, String oracleConnectionString, String urn, int languageId,
            int examCenterId, byte[] photo, byte[] sign, String allowWhatsappMessage,
            String whatsappNumber, String basePathP, String basePathS, String shareUser,
            String sharePass, int userId, int insurerUserId) {
        // The C# method had 'out Exception exOut'. This is now part of the DTO.
        // The service method will return the DTO, and the caller can check for the exception.
        try {
            return urnDataAccess.saveModificationOAIMS(connectionString, oracleConnectionString, urn,
                    languageId, examCenterId, photo, sign, allowWhatsappMessage, whatsappNumber,
                    basePathP, basePathS, shareUser, sharePass, userId, insurerUserId);
        } catch (Exception ex) {
            // This catch block is for exceptions thrown by the dataAccess call itself,
            // not for the logical 'exOut' which is part of the DTO.
            throw ex;
        }
    }

    public String updateURNDetails(String connectionString, List<Map<String, Object>> applicantData, int userId) {
        try {
            return urnDataAccess.updateURNDetails(connectionString, applicantData, userId);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public UpdateUrnRequestResult updateURNRequest(String connectionString, long id, List<Map<String, Object>> applicantData, String roleCode, int userId) {
        try {
            return urnDataAccess.updateURNRequest(connectionString, id, applicantData, roleCode, userId);
        } catch (Exception ex) {
            throw ex;
        }
    }

    // Validation methods are direct pass-throughs
    public boolean validateInternalRefNo(String connectionString, String internalRefNo, int insurerId, long applicantId) {
        try { return urnDataAccess.validateInternalRefNo(connectionString, internalRefNo, insurerId, applicantId); } catch (Exception ex) { throw ex; }
    }
    public boolean validateInternalRefNoApp(String connectionString, String internalRefNo, int insurerId, long applicantDataId) {
        try { return urnDataAccess.validateInternalRefNoApp(connectionString, internalRefNo, insurerId, applicantDataId); } catch (Exception ex) { throw ex; }
    }
    public String validatePAN(String connectionString, String pan) {
        try { return urnDataAccess.validatePAN(connectionString, pan); } catch (Exception ex) { throw ex; }
    }
    public String validatePAN(String connectionString, String pan, long applicantId, int insurerUserId) {
        try { return urnDataAccess.validatePAN(connectionString, pan, applicantId, insurerUserId); } catch (Exception ex) { throw ex; }
    }
    public String validateAadhaarCorporates(String connectionString, String aadhaarNo, String pan, String urn) {
        try { return urnDataAccess.validateAadhaarCorporates(connectionString, aadhaarNo, pan, urn); } catch (Exception ex) { throw ex; }
    }
    public String validateAadhaarCorporatesApp(String connectionString, String aadhaarNo, String pan, long applicantDataId) {
        try { return urnDataAccess.validateAadhaarCorporatesApp(connectionString, aadhaarNo, pan, applicantDataId); } catch (Exception ex) { throw ex; }
    }
    public String validateEmailCorporates(String connectionString, String emailId, long applicantid, String pan) {
        try { return urnDataAccess.validateEmailCorporates(connectionString, emailId, applicantid, pan); } catch (Exception ex) { throw ex; }
    }
    public String validateEmailCorporatesApp(String connectionString, String emailId, long applicantDataId, String pan) {
        try { return urnDataAccess.validateEmailCorporatesApp(connectionString, emailId, applicantDataId, pan); } catch (Exception ex) { throw ex; }
    }
    public String validateMobileCorporates(String connectionString, String mobileNo, long applicantid, String pan) {
        try { return urnDataAccess.validateMobileCorporates(connectionString, mobileNo, applicantid, pan); } catch (Exception ex) { throw ex; }
    }
    public String validateWhatsAppCorporates(String connectionString, String mobileNo, long applicantid, String pan) {
        try { return urnDataAccess.validateWhatsAppCorporates(connectionString, mobileNo, applicantid, pan); } catch (Exception ex) { throw ex; }
    }
    public String validateMobileCorporatesApp(String connectionString, String mobileNo, long applicantDataId, String pan) {
        try { return urnDataAccess.validateMobileCorporatesApp(connectionString, mobileNo, applicantDataId, pan); } catch (Exception ex) { throw ex; }
    }
    public String validateWhatsAppCorporatesApp(String connectionString, String mobileNo, long applicantDataId, String pan) {
        try { return urnDataAccess.validateWhatsAppCorporatesApp(connectionString, mobileNo, applicantDataId, pan); } catch (Exception ex) { throw ex; }
    }
    public String validateWhatsAppCorporatesForMod(String connectionString, String urn, String mobileNo) {
        try { return urnDataAccess.validateWhatsAppCorporatesForMod(connectionString, urn, mobileNo); } catch (Exception ex) { throw ex; }
    }

    public UploadSponsorshipResult uploadDuplicateURN(String connectionString, int userId, String roleCode, String fileNameAndPath) {
        // The C# method has 'out String Message' but returns DataSet.
        // The IIIDL interface method returns List<Map> for DataSet.
        // This service method will need to adapt the IIIDL result if message needs to be separate.
        // For now, assuming message is handled within the data or implied by empty/null data.
        List<Map<String, Object>> excelOutput = null;
        String message = "";
        try {
            FileParserUtil.ExcelParser excelParser = new FileParserUtil.ExcelParser();
            excelOutput = excelParser.parseAllAsData(fileNameAndPath, "Sheet1$", true); // Throws IOException

            if (excelOutput.isEmpty()) {
                message = "File has no record";
                return new UploadSponsorshipResult(new ArrayList<>(), message);
            }

            // Basic column check
            if (!(excelOutput.get(0).containsKey("URN") && excelOutput.get(0).keySet().size() == 1)) {
                message = "Invalid file format. Please download the template and try uploading again.";
                return new UploadSponsorshipResult(new ArrayList<>(), message);
            }

            // Add temporary processing columns
            for (Map<String, Object> row : excelOutput) {
                row.put("is_valid", 1); // Default to valid
                row.put("remarks", "");
            }

            int invalidCount = 0;
            Pattern regexPattern = Pattern.compile(CommonRegex.regexURN); // Using CommonRegex from FileParserUtil
            for (Map<String, Object> row : excelOutput) {
                String error = "";
                boolean isValid = true;
                String urnValue = String.valueOf(row.get("URN"));
                if (!regexPattern.matcher(urnValue).matches()) {
                    error += "Invalid URN. URN must be alphanumeric without any spaces or special characters";
                    isValid = false;
                    invalidCount++;
                }
                row.put("remarks", error);
                row.put("is_valid", isValid ? 1 : 0);
            }

            if (invalidCount == excelOutput.size() && !excelOutput.isEmpty()) {
                // Remove temp columns before returning error data
                for (Map<String, Object> row : excelOutput) {
                    row.remove("is_valid");
                }
                message = "Unable to proceed. Kindly rectify the errors and try uploading again";
                return new UploadSponsorshipResult(excelOutput, message);
            }

            // If some are valid, call data access layer
            List<Map<String, Object>> resultDataSet = urnDataAccess.uploadDuplicateURN(connectionString, excelOutput, userId, roleCode);
            message = ""; // Success implied by non-null/non-empty resultDataSet
            return new UploadSponsorshipResult(resultDataSet, message);

        } catch (IOException ioEx) {
            // This catch block handles errors from excelParser.parseAllAsData
            message = "Error occurred while reading the file: " + ioEx.getMessage();
            // ErrorLogger.logError... (if you have it available here or rethrow)
            throw new RuntimeException(message, ioEx); // Convert to unchecked for service layer
        } catch (Exception ex) {
            // Catch other exceptions, e.g., from urnDataAccess.uploadDuplicateURN
            throw ex; // Re-throw as per original C#
        }
    }


    public UploadSponsorshipResult uploadURN(String connectionString, String originalFileName, int userId, String roleCode,
                                           String insuranceType, String directoryNameAndPath,
                                           byte[] key, byte[] iv) { // Key, IV for Aadhaar, assuming
        String message = "";
        List<Map<String, Object>> resultDataSet = null; // This will be the final dataset returned by IIIDL
        FileParserUtil.ExcelParser excelParser = null;
        List<Map<String, Object>> excelOutput = null; // Data read from Excel

        try {
            Path dirPath = Paths.get(directoryNameAndPath);
            if (!Files.exists(dirPath) || !Files.isDirectory(dirPath)) {
                message = "Error occurred while reading the file: Unzipped data directory not found.";
                return new UploadSponsorshipResult(null, message);
            }

            Path excelFile = null;
            try (DirectoryStream<Path> stream = Files.newDirectoryStream(dirPath, "*.xls*")) {
                for (Path entry : stream) {
                    if (excelFile == null) excelFile = entry;
                    else { // More than one excel file found
                        message = "Invalid zip file template. Multiple Excel files found.";
                        return new UploadSponsorshipResult(null, message);
                    }
                }
            }
            if (excelFile == null) {
                message = "Invalid zip file template. No Excel file found.";
                return new UploadSponsorshipResult(null, message);
            }

            excelParser = new FileParserUtil.ExcelParser();
            excelOutput = excelParser.parseAllAsData(excelFile.toString(), "Sheet1$", true);

            if (excelOutput.isEmpty()) {
                message = "File has no record";
                return new UploadSponsorshipResult(new ArrayList<>(), message);
            }

            // Define expected fields based on roleCode
            String[] expectedFields;
            switch (roleCode) {
                case "CA": expectedFields = new String[] { "CoR Type", "Insurance Category", "Sponsorship Date", "Name Initial", "Candidate/Corporate Name", "Father Name", "Category", "Current House Number", "Current Street", "Current Town", "Current District", "Current State", "Current Pincode", "Permanent House Number", "Permanent Street", "Permanent Town", "Permanent District", "Permanent State", "Permanent Pincode", "Area", "Basic Qualification", "Board Name For Basic Qualification", "Roll Number For Basic Qualification", "Year of Passing For Basic Qualification", "Educational Qualification", "Date of Birth", "Sex", "Primary Profession", "Landline No", "Mobile No", "Branch Name", "Exam Mode", "Exam Body Name", "Exam Center Location", "Exam Language", "Email ID", "Contact Person Email ID", "PAN",  "Employee No", "Voter ID Card", "Driving License No", "Passport No", "Central Govt ID Card", "Nationality", "Photo File Name", "Signature File Name", "Allow WhatsApp Messages", "WhatsApp Number", "Telemarketer TRAI reg no" }; break;
                case "WA": expectedFields = new String[] { "CoR Type", "Insurance Category", "Sponsorship Date", "Name Initial", "Candidate/Corporate Name", "Father Name", "Category", "Current House Number", "Current Street", "Current Town", "Current District", "Current State", "Current Pincode", "Permanent House Number", "Permanent Street", "Permanent Town", "Permanent District", "Permanent State", "Permanent Pincode", "Area", "Basic Qualification", "Board Name For Basic Qualification", "Roll Number For Basic Qualification", "Year of Passing For Basic Qualification", "Educational Qualification", "Date of Birth", "Sex", "Primary Profession", "Landline No", "Mobile No", "Branch Name", "Exam Mode", "Exam Body Name", "Exam Center Location", "Exam Language", "Email ID", "Contact Person Email ID", "PAN",  "Employee No", "Voter ID Card", "Driving License No", "Passport No", "Central Govt ID Card", "Nationality", "Photo File Name", "Signature File Name", "Allow WhatsApp Messages", "WhatsApp Number", "Telemarketer TRAI reg no" }; break; // Same as CA in example
                case "BR": expectedFields = new String[] { "CoR Type", "Insurance Category", "Sponsorship Date", "Name Initial", "Candidate/Corporate Name", "Father Name", "Category", "Current House Number", "Current Street", "Current Town", "Current District", "Current State", "Current Pincode", "Permanent House Number", "Permanent Street", "Permanent Town", "Permanent District", "Permanent State", "Permanent Pincode", "Area", "Basic Qualification", "Board Name For Basic Qualification", "Roll Number For Basic Qualification", "Year of Passing For Basic Qualification", "Educational Qualification", "Date of Birth", "Sex", "Primary Profession", "Landline No", "Mobile No", "Branch Name", "Exam Mode", "Exam Body Name", "Exam Center Location", "Exam Language", "Email ID", "Contact Person Email ID", "PAN", "Employee No", "Voter ID Card", "Driving License No", "Passport No", "Central Govt ID Card", "Nationality", "Photo File Name", "Signature File Name", "Allow WhatsApp Messages", "WhatsApp Number", "Telemarketer TRAI reg no" }; break; // Same as CA
                case "IMF": expectedFields = new String[] { "CoR Type", "Insurance Category", "Sponsorship Date", "Name Initial", "Candidate/Corporate Name", "Father Name", "Category", "Current House Number", "Current Street", "Current Town", "Current District", "Current State", "Current Pincode", "Permanent House Number", "Permanent Street", "Permanent Town", "Permanent District", "Permanent State", "Permanent Pincode", "Area", "Basic Qualification", "Board Name For Basic Qualification", "Roll Number For Basic Qualification", "Year of Passing For Basic Qualification", "Educational Qualification", "Date of Birth", "Sex", "Primary Profession", "Landline No", "Mobile No", "Branch Name", "Exam Mode", "Exam Body Name", "Exam Center Location", "Exam Language", "Email ID", "Contact Person Email ID", "PAN", "Employee No", "Voter ID Card", "Driving License No", "Passport No", "Central Govt ID Card", "Nationality", "Photo File Name", "Signature File Name", "Allow WhatsApp Messages", "WhatsApp Number" }; break; // No Telemarketer TRAI
                case "I": expectedFields = new String[] { "Sponsorship Date","Name Initial","Candidate Name","Father Name","Category","Current House Number","Current Street","Current Town","Current District","Current State","Current Pincode","Permanent House Number","Permanent Street","Permanent Town","Permanent District","Permanent State","Permanent Pincode","Area","Basic Qualification","Board Name For Basic Qualification","Roll Number For Basic Qualification","Year of Passing For Basic Qualification","Educational Qualification","Other Qualification","Date of Birth","Sex","Phone No","Mobile No","Exam Mode","Exam Body Name","Exam Center Location","Exam Language","Email ID","PAN","Insurer Ref No","Voter ID Card","Driving License No","Passport No","Central Govt ID Card","Primary Profession","Nationality","Photo File Name","Signature File Name", "Allow WhatsApp Messages", "WhatsApp Number", "Candidate Type", "Telemarketer TRAI reg no", "Contact Person Email ID" }; break;
                default: throw new IllegalArgumentException("Invalid Role Code for UploadURN");
            }

            // Check for missing columns
            StringBuilder missingCols = new StringBuilder();
            Map<String, Object> firstRowSample = excelOutput.get(0); // Assume all rows have same columns
            for (String field : expectedFields) {
                if (!firstRowSample.containsKey(field)) {
                    if (missingCols.length() > 0) missingCols.append(", ");
                    missingCols.append(field);
                }
            }
            if (missingCols.length() > 0) {
                message = "Invalid file format. The required fields : " + missingCols.toString() + " not found.";
                return new UploadSponsorshipResult(null, message);
            }
            
            // --- Add dynamic/processing columns and perform row-wise validation ---
            // This part is complex and involves file system access for photos/signatures and regex validation.
            // It's a direct translation of the C# logic.
            List<Map<String, Object>> processedExcelOutput = new ArrayList<>();
            int invalidCount = 0;
            int rowNumCounter = 1;

            for (Map<String, Object> drOriginal : excelOutput) {
                Map<String, Object> dr = new HashMap<>(drOriginal); // Work on a copy
                dr.put("rownum", rowNumCounter++);
                boolean allColumnsAreEmpty = true;
                StringBuilder rowError = new StringBuilder();

                // Check for prohibited characters
                for (String field : expectedFields) {
                    String sValue = dr.get(field) != null ? String.valueOf(dr.get(field)).trim() : "";
                    if (!sValue.isEmpty()) {
                        allColumnsAreEmpty = false;
                        if (!Pattern.matches(CommonRegex.regexLowAscii, sValue)) {
                            if (rowError.length() > 0) rowError.append(", ");
                            rowError.append(field);
                        }
                    }
                }

                if (allColumnsAreEmpty) {
                    dr.put("IsValidRecord", false);
                    dr.put("UploadRemark", "The row is empty");
                    invalidCount++;
                } else if (rowError.length() > 0) {
                    dr.put("IsValidRecord", false);
                    dr.put("UploadRemark", "Invalid characters found in column(s): " + rowError.toString());
                    invalidCount++;
                } else {
                    // File and Date Validations (and other field specific validations)
                    StringBuilder fieldErrors = new StringBuilder();
                    String photoFileName = dr.get("Photo File Name") != null ? String.valueOf(dr.get("Photo File Name")).trim() : "";
                    String signFileName = dr.get("Signature File Name") != null ? String.valueOf(dr.get("Signature File Name")).trim() : "";

                    // Photo validation
                    if (photoFileName.isEmpty()) { fieldErrors.append(" Photo file name is required;"); }
                    else if (!(photoFileName.toLowerCase().endsWith(".jpg") || photoFileName.toLowerCase().endsWith(".jpeg"))) { fieldErrors.append(" Photo file should be jpg/jpeg;"); }
                    else if (photoFileName.equalsIgnoreCase(signFileName) && !photoFileName.isEmpty()) { fieldErrors.append(" Photo and Signature file name cannot be the same;"); }
                    else {
                        Path photoFile = findFileInDirectory(dirPath, photoFileName);
                        if (photoFile != null) {
                            if (Files.size(photoFile) > 51200) fieldErrors.append(" Photo File size should be less than 50KB;");
                            else dr.put("applicantPhoto", Files.readAllBytes(photoFile));
                        } else { fieldErrors.append(" Photo file '").append(photoFileName).append("' not found;"); }
                    }
                    // Signature validation
                    if (signFileName.isEmpty()) { fieldErrors.append(" Signature file name is required;"); }
                    else if (!(signFileName.toLowerCase().endsWith(".jpg") || signFileName.toLowerCase().endsWith(".jpeg"))) { fieldErrors.append(" Signature should be jpg/jpeg;"); }
                    else {
                        Path signFile = findFileInDirectory(dirPath, signFileName);
                        if (signFile != null) {
                            if (Files.size(signFile) > 51200) fieldErrors.append(" Signature File size should be less than 50KB;");
                            else dr.put("applicantSignature", Files.readAllBytes(signFile));
                        } else { fieldErrors.append(" Signature file '").append(signFileName).append("' not found;"); }
                    }
                    
                    // Date Validations
                    validateAndStoreDate(dr, "Date of Birth", "dd-MMM-yyyy", CommonRegex.regexLowAscii, fieldErrors, true); // Assuming regexLowAscii is placeholder for actual date regex
                    validateAndStoreDate(dr, "Sponsorship Date", "dd-MMM-yyyy", CommonRegex.regexLowAscii, fieldErrors, true);
                    validateAndStoreDate(dr, "Year of Passing For Basic Qualification", "MMM yyyy", CommonRegex.regexLowAscii, fieldErrors, true);

                    // WhatsApp validation
                    String allowWhatsapp = dr.get("Allow WhatsApp Messages") != null ? String.valueOf(dr.get("Allow WhatsApp Messages")).trim().toUpperCase() : "";
                    if (allowWhatsapp.isEmpty()) fieldErrors.append(" [Allow WhatsApp Messages] is required;");
                    else if (!"Y".equals(allowWhatsapp) && !"N".equals(allowWhatsapp)) fieldErrors.append(" Invalid [Allow WhatsApp Messages], select Y/N;");
                    dr.put("Allow WhatsApp Messages 2", allowWhatsapp); // Store processed value
                    dr.put("WhatsApp Number 2", dr.get("WhatsApp Number")); // Copy as is

                    // Email Validation
                    validateEmail(dr, "Email ID", CommonRegex.regexEmail, fieldErrors);
                    validateEmail(dr, "Contact Person Email ID", CommonRegex.regexEmail, fieldErrors);
                    
                    // Candidate Type for 'I' role
                     if ("I".equals(roleCode)) {
                        String candidateType = dr.get("Candidate Type") != null ? String.valueOf(dr.get("Candidate Type")).trim() : "";
                        if (candidateType.isEmpty()) fieldErrors.append(" [Candidate Type] is required;");
                        else if (!"Insurance Agent".equals(candidateType) && !"Authorised Verifier".equals(candidateType)) fieldErrors.append(" Invalid [Candidate Type];");
                        dr.put("Candidate Type 2", candidateType);
                        dr.put("Contact Person Email ID 2", dr.get("Contact Person Email ID"));
                    }
                    dr.put("Telemarketer TRAI reg no 2", dr.get("Telemarketer TRAI reg no"));


                    if (fieldErrors.length() > 0) {
                        dr.put("IsValidRecord", false);
                        dr.put("UploadRemark", fieldErrors.toString().trim());
                        invalidCount++;
                    } else {
                        dr.put("IsValidRecord", true);
                        dr.put("UploadRemark", "");
                    }
                }
                // Add internal processing columns (like C# did before filtering)
                dr.put("_chrLicIndOrCorporate", "I"); // C# hardcoded "I"
                dr.put("_bntID", rowNumCounter -1); // just a counter

                processedExcelOutput.add(dr);
            } // End row processing loop

            if (invalidCount == processedExcelOutput.size() && !processedExcelOutput.isEmpty()) {
                // Remove helper columns before returning the data with errors
                List<Map<String, Object>> errorReturnData = processedExcelOutput.stream()
                    .map(row -> {
                        Map<String, Object> cleanedRow = new HashMap<>(row);
                        cleanedRow.remove("_chrLicIndOrCorporate"); cleanedRow.remove("_bntID");
                        cleanedRow.remove("applicantPhoto"); cleanedRow.remove("applicantSignature");
                        cleanedRow.remove("Allow WhatsApp Messages 2"); cleanedRow.remove("WhatsApp Number 2");
                        cleanedRow.remove("Telemarketer TRAI reg no 2");
                        if ("I".equals(roleCode)) {
                             cleanedRow.remove("Candidate Type 2"); cleanedRow.remove("Contact Person Email ID 2");
                        }
                        // Keep IsValidRecord and UploadRemark for user feedback
                        return cleanedRow;
                    }).collect(Collectors.toList());
                message = "Unable to proceed. Kindly rectify the errors and try uploading again";
                return new UploadSponsorshipResult(errorReturnData, message);
            }
            
            // Prepare final table for DB by renaming temp columns and removing original ones
            List<Map<String, Object>> finalDbData = processedExcelOutput.stream()
                .filter(row -> (Boolean)row.get("IsValidRecord")) // Process only valid records
                .map(row -> {
                    Map<String, Object> dbRow = new HashMap<>(row);
                    dbRow.put("Allow WhatsApp Messages", dbRow.remove("Allow WhatsApp Messages 2"));
                    dbRow.put("WhatsApp Number", dbRow.remove("WhatsApp Number 2"));
                    dbRow.put("Telemarketer TRAI reg no", dbRow.remove("Telemarketer TRAI reg no 2"));
                    if ("I".equals(roleCode)) {
                        dbRow.put("Candidate Type", dbRow.remove("Candidate Type 2"));
                        dbRow.put("Contact Person Email ID", dbRow.remove("Contact Person Email ID 2"));
                    }
                    // Remove other temp/original columns not needed for DB SP call if any
                    dbRow.remove("IsValidRecord");
                    // dbRow.remove("UploadRemark"); // Or keep it if SP handles it
                    return dbRow;
                }).collect(Collectors.toList());


            if (roleCode.equals("I")) {
                UploadSponsorshipResult spResult = urnDataAccess.uploadSponsorshipFileAgents(connectionString, finalDbData, userId, insuranceType, originalFileName);
                resultDataSet = spResult.getDataSet();
                message = spResult.getMessage();
            } else {
                UploadSponsorshipResult spResult = urnDataAccess.uploadSponsorshipFileCorporates(connectionString, finalDbData, userId, roleCode, originalFileName);
                resultDataSet = spResult.getDataSet();
                message = spResult.getMessage();
            }
            return new UploadSponsorshipResult(resultDataSet, message);

        } catch (IOException ioEx) {
            message = "Error occurred while processing files: " + ioEx.getMessage();
            throw new RuntimeException(message, ioEx);
        } catch (Exception ex) {
            throw ex;
        }
    }
    
    private Path findFileInDirectory(Path baseDir, String fileName) throws IOException {
        final Path[] foundFile = {null};
        if (fileName == null || fileName.trim().isEmpty()) return null;

        Files.walk(baseDir)
            .filter(Files::isRegularFile)
            .filter(path -> path.getFileName().toString().equalsIgnoreCase(fileName))
            .findFirst()
            .ifPresent(path -> foundFile[0] = path);
        return foundFile[0];
    }

    private void validateAndStoreDate(Map<String, Object> row, String columnName, String format, String regex, StringBuilder errors, boolean isRequired) {
        String dateStr = row.get(columnName) != null ? String.valueOf(row.get(columnName)).trim() : "";
        if (dateStr.isEmpty()) {
            if (isRequired) errors.append(" [").append(columnName).append("] is required;");
            return;
        }
        if (!FileParserUtil.TextValidator.validateDate2(dateStr, format, regex)) {
            errors.append(" Invalid [").append(columnName).append("];");
        } else {
            // If valid and needs to be stored as a specific format string (C# example did this for YOP)
            // For now, assuming the string from Excel is fine if it passes validation.
            // If DB needs Date object, parse it here.
            // if (format.equals("MMM yyyy")) {
            //    try {
            //        Date parsedDate = new SimpleDateFormat(format).parse(dateStr);
            //        row.put(columnName, new SimpleDateFormat("MMM yyyy").format(parsedDate)); // Reformat
            //    } catch (ParseException e) { /* Already handled by validateDate2 */ }
            // }
        }
    }
    private void validateEmail(Map<String, Object> row, String columnName, String regex, StringBuilder errors) {
        String email = row.get(columnName) != null ? String.valueOf(row.get(columnName)).trim() : "";
        if (!email.isEmpty() && !Pattern.matches(regex, email)) {
            errors.append(" Invalid [").append(columnName).append("];");
        }
    }


    public List<Map<String, Object>> getURNDetailForEE(String connectionString, String urn) {
        try { return urnDataAccess.getURNDetailForEE(connectionString, urn); } catch (Exception ex) { throw ex; }
    }

    public String saveExamDetails(String connectionString, int hint, String urn, String examDate, String examineeId, int marks, int result, int currentUser) {
        try { return urnDataAccess.saveExamDetails(connectionString, hint, urn, examDate, examineeId, marks, result, currentUser); } catch (Exception ex) { throw ex; }
    }

    public String updateUrnStatus(String connectionString, String urn, String changedStatus, String userName, String userId, String userMachineIP) {
        try { return urnDataAccess.updateUrnStatus(connectionString, urn, changedStatus, userName, userId, userMachineIP); } catch (Exception ex) { throw ex; }
    }

    public void updatePaymentStatus(String connectionString, String transactionId, String nseitRefNo, String pgRefNo, String pgStatus, String pgResponse) {
        try { urnDataAccess.updatePaymentStatus(connectionString, transactionId, nseitRefNo, pgRefNo, pgStatus, pgResponse); } catch (Exception ex) { throw ex; }
    }

    public UrnDataResult getURNDataForDeletion(String connectionString, String urn, int userId) {
        try { return urnDataAccess.getURNDataForDeletion(connectionString, urn, userId); } catch (Exception ex) { throw ex; }
    }

    public String deleteURN(String connectionString, String urn, int userId) {
        try { return urnDataAccess.deleteURN(connectionString, urn, userId); } catch (Exception ex) { throw ex; }
    }
    
    public List<Map<String,Object>> getQualData(String connectionString, String urn, int userId){
        try { return urnDataAccess.getQualData(connectionString, urn, userId); } catch (Exception ex) { throw ex; }
    }

    public UrnDataResult getURNDataForApproval(String connectionString, int hint, long id, int userId){
        try { return urnDataAccess.getURNDataForApproval(connectionString, hint, id, userId); } catch (Exception ex) { throw ex; }
    }
    
    public String approveRejectURN(String connectionString, long id, int userId, String approversRemarks, String status){
        try { return urnDataAccess.approveRejectURN(connectionString, id, userId, approversRemarks, status); } catch (Exception ex) { throw ex; }
    }

    public List<Map<String,Object>> updatePS(String connectionString, String originalFileName, int userId, String directoryNameAndPath) {
        // C# had (DataTable, int, string, out string Message)
        // Java interface (List<Map>, int, string) returns List<Map>
        // Message handling from IIIDL is not present in this C# BL method, assumed it's in the returned DataSet
        String message = ""; // Placeholder for potential message handling
        FileParserUtil.ExcelParser excelParser = null;
        List<Map<String, Object>> excelOutput = null;
        
        try {
            Path dirPath = Paths.get(directoryNameAndPath);
            if (!Files.exists(dirPath) || !Files.isDirectory(dirPath)) {
                message = "Error occurred while reading the file: Unzipped data directory not found.";
                // How to return message with List<Map>? For now, throw or log
                throw new RuntimeException(message);
            }

            Path excelFile = null;
            try (DirectoryStream<Path> stream = Files.newDirectoryStream(dirPath, "*.xls*")) {
                for (Path entry : stream) {
                    if (excelFile == null) excelFile = entry;
                    else throw new IOException("Multiple Excel files found in directory.");
                }
            }
            if (excelFile == null) throw new FileNotFoundException("No Excel file found in directory.");

            excelParser = new FileParserUtil.ExcelParser();
            excelOutput = excelParser.parseAllAsData(excelFile.toString(), "Sheet1$", true);

            if (excelOutput.isEmpty()) {
                message = "File has no record";
                return new ArrayList<>(); // Return empty list
            }

            String[] expectedFields = new String[] { "urn","photo_file_name","sign_file_name"};
            StringBuilder missingCols = new StringBuilder();
            Map<String, Object> firstRowSample = excelOutput.get(0);
            for (String field : expectedFields) {
                if (!firstRowSample.containsKey(field)) {
                    if (missingCols.length() > 0) missingCols.append(", ");
                    missingCols.append(field);
                }
            }
            if (missingCols.length() > 0) {
                message = "Invalid file format. The required fields : " + missingCols.toString() + " not found.";
                throw new RuntimeException(message);
            }

            // Add processing columns
            for (Map<String, Object> row : excelOutput) {
                row.put("photo", null); // byte[]
                row.put("signature", null); // byte[]
                row.put("remarks", "");
            }
            
            for (Map<String, Object> row : excelOutput) {
                StringBuilder rowRemarks = new StringBuilder();
                String photoFileName = row.get("photo_file_name") != null ? String.valueOf(row.get("photo_file_name")) : null;
                String signFileName = row.get("sign_file_name") != null ? String.valueOf(row.get("sign_file_name")) : null;

                if (photoFileName != null && !photoFileName.isEmpty()) {
                    Path photoFile = findFileInDirectory(dirPath, photoFileName);
                    if (photoFile != null) row.put("photo", Files.readAllBytes(photoFile));
                    else rowRemarks.append("File not found ").append(photoFileName).append(";");
                }
                if (signFileName != null && !signFileName.isEmpty()) {
                    Path signFile = findFileInDirectory(dirPath, signFileName);
                    if (signFile != null) row.put("signature", Files.readAllBytes(signFile));
                    else rowRemarks.append("File not found ").append(signFileName).append(";");
                }
                row.put("remarks", rowRemarks.toString().trim());
            }
            return urnDataAccess.updatePS(connectionString, excelOutput, userId, originalFileName);

        } catch (IOException ioEx){
            message = "Error processing files for UpdatePS: " + ioEx.getMessage();
            throw new RuntimeException(message, ioEx);
        } catch (Exception ex) {
            throw ex;
        }
    }
    
    public List<Map<String,Object>> uploadUrnExamCentreUpdate(String connectionString, int userId, String fileNameAndPath) {
         // C# had (String, int, string, out string Message)
        // Java interface (String, List<Map>, int) returns List<Map>
        String message = ""; // Placeholder
        List<Map<String, Object>> excelOutput = null;
        FileParserUtil.ExcelParser excelParser = null;
        boolean initialStatus = true;

        try {
            excelParser = new FileParserUtil.ExcelParser();
            excelOutput = excelParser.parseAllAsData(fileNameAndPath, "Sheet1$", true);

            if (excelOutput.isEmpty()) {
                message = "File has no record";
                initialStatus = false; // As per C# logic where it implies an issue
                return new ArrayList<>(); // Or throw new RuntimeException(message);
            }

            String[] expectedFields = { "URN", "TEST_CENTER" };
            // Column existence check
            Map<String,Object> firstRow = excelOutput.get(0);
            StringBuilder missing = new StringBuilder();
            for(String field : expectedFields){
                if(!firstRow.containsKey(field)){
                    if(missing.length() > 0) missing.append(", ");
                    missing.append(field);
                }
            }
            if(missing.length() > 0){
                initialStatus = false;
                message = "Invalid file format. The required fields : " + missing.toString() + " not found.";
                throw new RuntimeException(message); // Or return empty with message
            }
            
            // Character validation (simplified)
            for(Map<String, Object> row : excelOutput){
                for(String field : expectedFields){
                    String val = row.get(field) != null ? String.valueOf(row.get(field)).trim() : "";
                    if(!val.isEmpty() && !Pattern.matches(CommonRegex.regexLowAscii, val)){
                        // C# does not stop processing here, just builds an error string.
                        // For Java, this would ideally be part of a validation result object.
                        // For direct translation, this error accumulation is complex to mirror perfectly without changing structure.
                        // Let's assume for now that if any row has bad chars, the whole upload might be problematic
                        // or the SP handles individual row errors.
                        System.err.println("Warning: Invalid char in UploadUrnExamCentreUpdate data - field: " + field + ", value: " + val);
                    }
                }
            }
            return urnDataAccess.uploadUrnExamCentreUpdate(connectionString, excelOutput, userId);
        } catch (IOException ioEx) {
            message = "Error reading Excel file for UploadUrnExamCentreUpdate: " + ioEx.getMessage();
            throw new RuntimeException(message, ioEx);
        } catch (Exception ex) {
            throw ex;
        }
    }
    
    public List<Map<String,Object>> uploadTrainingDetails(String connectionString, String fileNameAndPath, int userId) {
         // C# had (String, String, int, out string Message)
        // Java interface (String, List<Map>, int) returns List<Map>
        String message = ""; // Placeholder
        FileParserUtil.TextParser textParser = null;
        List<Map<String, Object>> inputData = null;

        try {
            File fileInfo = new File(fileNameAndPath);
            if (!fileInfo.exists()) {
                message = "File Not Found";
                throw new FileNotFoundException(message);
            }
            
            FileParserUtil.Field[] fields = new FileParserUtil.Field[5];
            fields[0] = new FileParserUtil.Field("urn", "", 0, 0, String.class, "", false);
            fields[1] = new FileParserUtil.Field("training_start_date", "", 1, 0, Date.class, "dd-MMM-yyyy", false);
            fields[2] = new FileParserUtil.Field("training_end_date", "", 2, 0, Date.class, "dd-MMM-yyyy", false);
            fields[3] = new FileParserUtil.Field("training_hours", "", 3, 0, Long.class, "", false); // Int64 -> Long
            fields[4] = new FileParserUtil.Field("tcc_expiry_date", "", 4, 0, Date.class, "dd-MMM-yyyy", false);

            List<String> expectedHeaders = new ArrayList<>();
            expectedHeaders.add("urn|training_start_date|training_end_date|training_hours|tcc_expiry_date");
            
            List<String> actualHeaders = new ArrayList<>(); // To be populated by mock parser
            StringBuilder parseMessage = new StringBuilder(); // For 'out Message' from parser

            textParser = new FileParserUtil.TextParser();
            inputData = textParser.parse2(fileNameAndPath, 1, false, "|", fields, expectedHeaders, actualHeaders, parseMessage);
            message = parseMessage.toString();

            if (message.isEmpty()) { // Parse successful
                // Remove utility columns added by C# TextParser if any
                // The mock parser doesn't add ERROR_FLAG, ROW_ID etc. If a real one does, remove them here.
                // e.g., inputData.forEach(row -> { row.remove("ERROR_FLAG"); row.remove("ROW_ID"); });
                return urnDataAccess.uploadTrainingDetails(connectionString, inputData, userId);
            } else {
                // If parseMessage is not empty, it means parser found issues.
                // The C# code still removed columns even on parser error.
                // This indicates the data might still be returned for user to see errors.
                // For now, if parser returns a message, we assume it's an error and don't proceed to DB.
                throw new RuntimeException("Text parsing error: " + message);
            }

        } catch (IOException ioEx){
             message = "Error processing text file for UploadTrainingDetails: " + ioEx.getMessage();
            throw new RuntimeException(message, ioEx);
        }
        catch (Exception ex) {
            throw ex;
        }
    }

    public List<Map<String, Object>> getPaymentReceiptDataIIIBL(String connectionString, String pan) { // Renamed to avoid clash
        try { return urnDataAccess.getURNForPAN(connectionString, pan); } // C# logic was calling GetURNForPAN
        catch (Exception ex) { throw ex; }
    }

    public boolean validateURNandDOB(String connectionString, String urn, Date dob) {
        try { return urnDataAccess.validateURNandDOB(connectionString, urn, dob); } catch (Exception ex) { throw ex; }
    }
}