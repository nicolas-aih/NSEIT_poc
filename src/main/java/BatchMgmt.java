import java.sql.*;
import java.util.*;

public class BatchMgmt {
    public static void main(String[] args) {
        String connString =  "jdbc:sqlserver://" + "LAPTOP-AMFV5IUN" + ";databaseName=" + "AgencyLicensingPortal" + 
                              ";user=" + "sa" + 
                              ";password=" + "sa123" + 
                              ";encrypt=true;trustServerCertificate=true;";
        
        // Example: Verify Batch
        BatchMgmt batchMgmt = new BatchMgmt();
        Map<String, Object> outParams = new HashMap<>();
        try {
            batchMgmt.verifyBatch(connString, "TXN12345", outParams);
            System.out.println("Payment Mode: " + outParams.get("PaymentMode"));
            System.out.println("Total Amount: " + outParams.get("TotalAmount"));
            System.out.println("Can Proceed: " + outParams.get("CanProceed"));
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    public void verifyBatch(String connectionString, String transactionId,
                           Map<String, Object> outParams) throws SQLException {
        String sql = "{call VerifyBatchProcedure(?, ?, ?, ?, ?)}";
        try (Connection conn = DriverManager.getConnection(connectionString);
             CallableStatement cstmt = conn.prepareCall(sql)) {
            
            cstmt.setString(1, transactionId);
            cstmt.registerOutParameter(2, Types.VARCHAR); // PaymentMode
            cstmt.registerOutParameter(3, Types.DECIMAL); // TotalAmount
            cstmt.registerOutParameter(4, Types.VARCHAR); // Message
            cstmt.registerOutParameter(5, Types.BOOLEAN); // CanProceed
            
            cstmt.execute();
            
            outParams.put("PaymentMode", cstmt.getString(2));
            outParams.put("TotalAmount", cstmt.getBigDecimal(3));
            outParams.put("Message", cstmt.getString(4));
            outParams.put("CanProceed", cstmt.getBoolean(5));
        }
    }

    public List<Map<String, Object>> getTransactionList(String connectionString, int hint, 
                                                       String transactionId, java.util.Date fromDate, 
                                                       java.util.Date toDate, int status, int userId) throws SQLException {
        String sql = "{call GetTransactionListProcedure(?, ?, ?, ?, ?, ?)}";
        try (Connection conn = DriverManager.getConnection(connectionString);
             CallableStatement cstmt = conn.prepareCall(sql)) {
            
            cstmt.setInt(1, hint);
            cstmt.setString(2, transactionId);
            cstmt.setDate(3, new java.sql.Date(fromDate.getTime()));
            cstmt.setDate(4, new java.sql.Date(toDate.getTime()));
            cstmt.setInt(5, status);
            cstmt.setInt(6, userId);
            
            return processResultSet(cstmt);
        }
    }

    // Add other methods following the same pattern...

    private List<Map<String, Object>> processResultSet(CallableStatement cstmt) throws SQLException {
        List<Map<String, Object>> results = new ArrayList<>();
        boolean hasResults = cstmt.execute();
        
        while (hasResults) {
            try (ResultSet rs = cstmt.getResultSet()) {
                ResultSetMetaData meta = rs.getMetaData();
                while (rs.next()) {
                    Map<String, Object> row = new HashMap<>();
                    for (int i = 1; i <= meta.getColumnCount(); i++) {
                        row.put(meta.getColumnName(i), rs.getObject(i));
                    }
                    results.add(row);
                }
            }
            hasResults = cstmt.getMoreResults();
        }
        return results;
    }
}

// Main class removed as we moved the main method to BatchMgmt
