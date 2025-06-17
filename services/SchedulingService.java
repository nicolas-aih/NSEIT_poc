package com.example.services;

import com.example.interfaces.SchedulingDataAccess;
// Import nested DTOs
import com.example.interfaces.SchedulingDataAccess.*;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.Date;
import java.util.List;
import java.util.Map;

// Corresponds to IIIBL.Scheduling.cs
@Service
public class SchedulingService {

    private final SchedulingDataAccess schedulingDataAccess;

    @Autowired
    public SchedulingService(SchedulingDataAccess schedulingDataAccess) {
        this.schedulingDataAccess = schedulingDataAccess;
    }

    public CandidateDetailsResult getCandidateDetails(String connectionString, String urn) {
        try {
            return schedulingDataAccess.getCandidateDetails(connectionString, urn);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public CandidateDetailsResult getCandidateDetailsRC(String connectionString, String urn) {
        try {
            return schedulingDataAccess.getCandidateDetailsRC(connectionString, urn);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public List<Map<String, Object>> getCandidateDetailsRCB(String connectionString, int centerId, Date examDateTime) {
        try {
            return schedulingDataAccess.getCandidateDetailsRCB(connectionString, centerId, examDateTime);
        } catch (Exception ex) {
            throw ex;
        }
    }

    /*
    // Corresponds to commented out IIIDL.Scheduling.GetDatesForCenter
    public DatesForCenterResult getDatesForCenter(String connectionString, int clientId, int centerId, int examDuration) {
        try {
            return schedulingDataAccess.getDatesForCenter(connectionString, clientId, centerId, examDuration);
        } catch (Exception ex) {
            throw ex;
        }
    }

    // Corresponds to commented out IIIDL.Scheduling.GetBatchesForCenterDate
    public BatchesForCenterDateResult getBatchesForCenterDate(String connectionString, int clientId, Date preferredDate, int centerId, int examDuration) {
        try {
            return schedulingDataAccess.getBatchesForCenterDate(connectionString, clientId, preferredDate, centerId, examDuration);
        } catch (Exception ex) {
            throw ex;
        }
    }
    */

    public List<Map<String, Object>> getScheduledBatchesForCenterDate(String connectionString, int hint, Date examDate, int centerId, String slot) {
        try {
            return schedulingDataAccess.getScheduledBatchesForCenterDate(connectionString, hint, examDate, centerId, slot);
        } catch (Exception ex) {
            throw ex;
        }
    }

    /*
    // Corresponds to commented out IIIDL.Scheduling.BookSeat
    public BookSeatResult bookSeat(String connectionString, int clientId, long clientSideIdentifier, String salutation,
                                   String candidateName, String candidateEmail, String candidatePhone, int cssCenterId,
                                   Date testDate, String fromTime, int examDuration) {
        // Note: IIIBL method had clientSideIdentifier as String, but it was passed as long to IIIDL
        // Assuming the IIIDL interface would take String clientSideIdentifier as per its original usage
        try {
            return schedulingDataAccess.bookSeat(connectionString, clientId, String.valueOf(clientSideIdentifier), salutation,
                                                candidateName, candidateEmail, candidatePhone, cssCenterId, testDate,
                                                fromTime, examDuration);
        } catch (Exception ex) {
            throw ex;
        }
    }

    // Corresponds to commented out IIIDL.Scheduling.RescheduleSeat
    public BulkOperationResult rescheduleSeat(String connectionString, int clientId, List<Map<String, Object>> candidateData,
                                           String remarks, String source) {
        try {
            return schedulingDataAccess.rescheduleSeat(connectionString, clientId, candidateData, remarks, source);
        } catch (Exception ex) {
            throw ex;
        }
    }
    
    // Corresponds to commented out IIIDL.Scheduling.CancelSeat
    public BulkOperationResult cancelSeat(String connectionString, int clientId, List<Map<String, Object>> candidateData,
                                       String remarks, String source) {
        try {
            return schedulingDataAccess.cancelSeat(connectionString, clientId, candidateData, remarks, source);
        } catch (Exception ex) {
            throw ex;
        }
    }
    */

    public void updateBookingStatus(String connectionString, String urn, long clientSideIdentifier, String cssReferenceNumber,
                                    Date testDate, String fromTime, int cssCenterId, int languageId) {
        try {
            schedulingDataAccess.updateBookingStatus(connectionString, urn, clientSideIdentifier, cssReferenceNumber,
                                                    testDate, fromTime, cssCenterId, languageId);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public List<Map<String, Object>> updateBookingStatusBulk(String connectionString, List<Map<String, Object>> data) {
        try {
            return schedulingDataAccess.updateBookingStatusBulk(connectionString, data);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public List<Map<String, Object>> updateCancellationStatusBulk(String connectionString, List<Map<String, Object>> data) {
        try {
            return schedulingDataAccess.updateCancellationStatusBulk(connectionString, data);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public List<Map<String, Object>> getScheduleReport(String connectionString, int hint, Date fromDate, Date toDate) {
        try {
            return schedulingDataAccess.getScheduleReport(connectionString, hint, fromDate, toDate);
        } catch (Exception ex) {
            throw ex;
        }
    }

    /*
    // Corresponds to commented out IIIDL.Scheduling.GetReconcileBoookingDetails
    public List<Map<String, Object>> getReconcileBoookingDetails(String connectionString, Date reconDate, int clientId) {
        try {
            return schedulingDataAccess.getReconcileBoookingDetails(connectionString, reconDate, clientId);
        } catch (Exception ex) {
            throw ex;
        }
    }
    */

    public List<Map<String, Object>> reconcileBoookings(String connectionString, List<Map<String, Object>> dataTable) {
        try {
            return schedulingDataAccess.reconcileBoookings(connectionString, dataTable);
        } catch (Exception ex) {
            throw ex;
        }
    }
}