package com.example.interfaces;

import java.util.Date;
import java.util.List;
import java.util.Map;

// Corresponds to IIIDL.Scheduling.cs
public interface SchedulingDataAccess {

    // --- Nested DTOs for methods with 'out' parameters ---
    class CandidateDetailsResult {
        private List<Map<String, Object>> dataSet; // Represents DataSet (can have multiple tables)
        private String message; // Represents 'out String Message'

        public CandidateDetailsResult(List<Map<String, Object>> dataSet, String message) {
            this.dataSet = dataSet;
            this.message = message;
        }
        // If DataSet always contains a single table for these methods,
        // then dataSet could be List<Map<String, Object>> for that single table.
        // For now, assuming List<Map> to represent the first table if DataSet usually has one.
        // If multiple tables are in DataSet, then List<List<Map<String,Object>>> or Map<String, List<Map<String,Object>>>
        public List<Map<String, Object>> getDataSet() { return dataSet; }
        public String getMessage() { return message; }
        public void setDataSet(List<Map<String,Object>> dataSet) { this.dataSet = dataSet; }
        public void setMessage(String message) { this.message = message; }
    }

    /*
    // DTO for GetDatesForCenter (if uncommented)
    class DatesForCenterResult {
        private List<Map<String, Object>> dataSet;
        private String message;
        public DatesForCenterResult(List<Map<String, Object>> dataSet, String message) { this.dataSet = dataSet; this.message = message; }
        public List<Map<String, Object>> getDataSet() { return dataSet; }
        public String getMessage() { return message; }
    }

    // DTO for GetBatchesForCenterDate (if uncommented)
    class BatchesForCenterDateResult {
        private List<Map<String, Object>> dataSet;
        private String message;
        public BatchesForCenterDateResult(List<Map<String, Object>> dataSet, String message) { this.dataSet = dataSet; this.message = message; }
        public List<Map<String, Object>> getDataSet() { return dataSet; }
        public String getMessage() { return message; }
    }

    // DTO for BookSeat (if uncommented)
    class BookSeatResult {
        private String status;
        private String message;
        private String cssReferenceNumber;
        public BookSeatResult(String status, String message, String cssReferenceNumber) {
            this.status = status; this.message = message; this.cssReferenceNumber = cssReferenceNumber;
        }
        public String getStatus() { return status; }
        public String getMessage() { return message; }
        public String getCssReferenceNumber() { return cssReferenceNumber; }
    }
    
    // DTO for RescheduleSeat and CancelSeat (if uncommented)
    class BulkOperationResult { // Can be used for RescheduleSeat & CancelSeat
        private List<Map<String, Object>> dataSet; // Resulting dataset
        private String status;
        private String message;
        public BulkOperationResult(List<Map<String, Object>> dataSet, String status, String message) {
            this.dataSet = dataSet; this.status = status; this.message = message;
        }
        public List<Map<String, Object>> getDataSet() { return dataSet; }
        public String getStatus() { return status; }
        public String getMessage() { return message; }
    }
    */


    // --- Interface Methods ---
    CandidateDetailsResult getCandidateDetails(String connectionString, String urn);

    CandidateDetailsResult getCandidateDetailsRC(String connectionString, String urn);

    List<Map<String, Object>> getCandidateDetailsRCB(String connectionString, int centerId, Date examDateTime);

    /*
    // Commented out methods from IIIDL.Scheduling.cs that interact with Oracle
    DatesForCenterResult getDatesForCenter(String connectionString, int clientId, int centerId, int examDuration);

    BatchesForCenterDateResult getBatchesForCenterDate(String connectionString, int clientId, Date preferredDate, int centerId, int examDuration);
    */

    List<Map<String, Object>> getScheduledBatchesForCenterDate(String connectionString, int hint, Date examDate, int centerId, String slot);

    /*
    BookSeatResult bookSeat(String connectionString, int clientId, String clientSideIdentifier, String salutation,
                            String candidateName, String candidateEmail, String candidatePhone, int centerId,
                            Date testDate, String fromTime, int examDuration);

    BulkOperationResult rescheduleSeat(String connectionString, int clientId, List<Map<String, Object>> candidateData,
                                     String remarks, String source);
                                     
    BulkOperationResult cancelSeat(String connectionString, int clientId, List<Map<String, Object>> candidateData,
                                 String remarks, String source);
    */

    void updateBookingStatus(String connectionString, String urn, long clientReferenceNumber, String cssReferenceNumber,
                             Date testDate, String testTime, int centerId, int languageId);

    List<Map<String, Object>> updateBookingStatusBulk(String connectionString, List<Map<String, Object>> data);

    List<Map<String, Object>> updateCancellationStatusBulk(String connectionString, List<Map<String, Object>> data);

    List<Map<String, Object>> getScheduleReport(String connectionString, int hint, Date fromDate, Date toDate);

    /*
    // Commented out method for Oracle DB
    List<Map<String, Object>> getReconcileBoookingDetails(String connectionString, Date reconDate, int clientId);
    */

    List<Map<String, Object>> reconcileBoookings(String connectionString, List<Map<String, Object>> dataTable);
}