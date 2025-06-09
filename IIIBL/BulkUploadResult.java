package IIIBL;

import java.util.List;
import java.util.Map;

// Helper class to return multiple values from BulkUploadExamRegData
public class BulkUploadResult {
    private List<Map<String, Object>> uploadData; // Equivalent of the returned DataSet/DataTable from DAL call
    private boolean status;
    private String message;

    public BulkUploadResult(List<Map<String, Object>> uploadData, boolean status, String message) {
        this.uploadData = uploadData;
        this.status = status;
        this.message = message;
    }

    public List<Map<String, Object>> getUploadData() {
        return uploadData;
    }

    public boolean isStatus() {
        return status;
    }

    public String getMessage() {
        return message;
    }

    @Override
    public String toString() {
        return "BulkUploadResult{" +
               "status=" + status +
               ", message='" + message + '\'' +
               ", uploadData=" + uploadData +
               '}';
    }
}