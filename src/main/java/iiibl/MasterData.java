// File: iiibl/MasterData.java
package iiibl;

// import IIDL.*; // Import the stub DAL
// import IIIDL.MasterData; // Import the stub DAL
import java.util.List;
import java.util.Map;
import java.util.ArrayList; // Needed for the main method's dummy data example
import java.util.HashMap;   // Needed for the main method's dummy data example

// Equivalent of your C# IIIBL.MasterData class
public class MasterData {

    // Method equivalent to C# GetStates
    // DataSet is translated to List<Map<String, Object>>
    public List<Map<String, Object>> getStates(String connectionString) {
        iiidl.MasterData objMasterData = null;
        List<Map<String, Object>> objDataSet = null;
        try {
            objMasterData = new iiidl.MasterData(); // Instantiate the DAL stub
            objDataSet = objMasterData.GetStates(connectionString); // Call the DAL method
        } catch (Exception ex) {
            // In Java, simply 'throw ex;' re-throws the exception preserving the stack trace
            throw ex;
        } finally {
            // Setting to null is not strictly necessary in Java due to garbage collection,
            // but kept for direct translation fidelity.
            objMasterData = null;
        }
        return objDataSet;
    }

    // Method equivalent to C# GetNotifications
    public List<Map<String, Object>> getNotifications(String connectionString, String notificationType /*T-icker or N-otification panel*/, String roleCode) {
        iiidl.MasterData objMasterData = null;
        List<Map<String, Object>> objDataSet = null;
        try {
            objMasterData = new iiidl.MasterData();
            objDataSet = objMasterData.GetNotifications(connectionString, notificationType, roleCode);
        } catch (Exception ex) {
            throw ex;
        } finally {
            objMasterData = null;
        }
        return objDataSet;
    }

    // Method equivalent to C# GetMasterData
    public List<Map<String, Object>> getMasterData(String connectionString) {
        iiidl.MasterData objMasterData = null;
        List<Map<String, Object>> objDataSet = null;
        try {
            objMasterData = new iiidl.MasterData();
            objDataSet = objMasterData.GetMasterData(connectionString);
        } catch (Exception ex) {
            throw ex;
        } finally {
            objMasterData = null;
        }
        return objDataSet;
    }

    // Method equivalent to C# GetCenterListForDistrict
    public List<Map<String, Object>> getCenterListForDistrict(String connectionString, int districtId) {
        iiidl.MasterData objMasterData = null;
        List<Map<String, Object>> objDataSet = null;
        try {
            objMasterData = new iiidl.MasterData();
            objDataSet = objMasterData.GetCenterListForDistrict(connectionString, districtId);
        } catch (Exception ex) {
            throw ex;
        } finally {
            objMasterData = null;
        }
        return objDataSet;
    }

    // Method equivalent to C# GetInsurers
    public List<Map<String, Object>> getInsurers(String connectionString, int insurerId) {
        iiidl.MasterData objMasterData = null;
        List<Map<String, Object>> objDataSet = null;
        try {
            objMasterData = new iiidl.MasterData();
            objDataSet = objMasterData.GetInsurers(connectionString, insurerId);
        } catch (Exception ex) {
            throw ex;
        } finally {
            objMasterData = null;
        }
        return objDataSet;
    }

    // Method equivalent to C# GetInsurers2
    public List<Map<String, Object>> getInsurers2(String connectionString, int insurerId) {
        iiidl.MasterData objMasterData = null;
        List<Map<String, Object>> objDataSet = null;
        try {
            objMasterData = new iiidl.MasterData();
            objDataSet = objMasterData.GetInsurers2(connectionString, insurerId);
        } catch (Exception ex) {
            throw ex;
        } finally {
            objMasterData = null;
        }
        return objDataSet;
    }

    // Method equivalent to C# GetDPForInsurer
    public List<Map<String, Object>> getDPForInsurer(String connectionString, int insurerId, int dpId) {
        iiidl.MasterData objMasterData = null;
        List<Map<String, Object>> objDataSet = null;
        try {
            objMasterData = new iiidl.MasterData();
            objDataSet = objMasterData.GetDPForInsurer(connectionString, insurerId, dpId);
        } catch (Exception ex) {
            throw ex;
        } finally {
            objMasterData = null;
        }
        return objDataSet;
    }

    // Method equivalent to C# GetACForDPs
    public List<Map<String, Object>> getACForDPs(String connectionString, int insurerId, int dpId, int acId) {
        iiidl.MasterData objMasterData = null;
        List<Map<String, Object>> objDataSet = null;
        try {
            objMasterData = new iiidl.MasterData();
            objDataSet = objMasterData.GetACForDPs(connectionString, insurerId, dpId, acId);
        } catch (Exception ex) {
            throw ex;
        } finally {
            objMasterData = null;
        }
        return objDataSet;
    }

    // Method equivalent to C# GetExamCenter
    public List<Map<String, Object>> getExamCenter(String connectionString, short examBodyId, short examCenterId) {
        iiidl.MasterData objMasterData = null;
        List<Map<String, Object>> objDataSet = null;
        try {
            objMasterData = new iiidl.MasterData();
            objDataSet = objMasterData.GetExamCenter(connectionString, examBodyId, examCenterId);
        } catch (Exception ex) {
            throw ex;
        } finally {
            objMasterData = null;
        }
        return objDataSet;
    }

    // Method equivalent to C# GetNewDPId
    public int getNewDPId(String connectionString, int insurerUserId) {
        iiidl.MasterData objMasterData = null;
        int dpId = -1;
        try {
            objMasterData = new iiidl.MasterData();
            dpId = objMasterData.GetNewDPId(connectionString, insurerUserId);
        } catch (Exception ex) {
            throw ex;
        } finally {
            objMasterData = null;
        }
        return dpId;
    }

    // Method equivalent to C# GetCompanyListForVoucherEntry
    public List<Map<String, Object>> getCompanyListForVoucherEntry(String connectionString, String companyType) {
        iiidl.MasterData objMasterData = null;
        List<Map<String, Object>> objDataSet = null;
        try {
            objMasterData = new iiidl.MasterData();
            objDataSet = objMasterData.GetCompanyListForVoucherEntry(connectionString, companyType);
        } catch (Exception ex) {
            throw ex;
        } finally {
            objMasterData = null;
        }
        return objDataSet;
    }

    // --- Main function for testing ---
    public static void main(String[] args) {
        System.out.println("--- Testing iiibl.MasterData ---");

        MasterData bll = new MasterData();
        String testConnectionString = "jdbc:sqlserver://localhost;database=MyDb;user=sa;password=pwd"; // Dummy connection string

        // Test GetStates
        System.out.println("\nTesting getStates...");
        try {
            List<Map<String, Object>> states = bll.getStates(testConnectionString);
            System.out.println("States received: " + states);
        } catch (Exception e) {
            System.err.println("Error getting states: " + e.getMessage());
            e.printStackTrace();
        }

        // Test GetNotifications
        System.out.println("\nTesting getNotifications...");
        try {
            List<Map<String, Object>> notifications = bll.getNotifications(testConnectionString, "T", "ADMIN");
            System.out.println("Notifications received: " + notifications);
        } catch (Exception e) {
            System.err.println("Error getting notifications: " + e.getMessage());
            e.printStackTrace();
        }

        // Test GetNewDPId
        System.out.println("\nTesting getNewDPId...");
         try {
             int newDpId = bll.getNewDPId(testConnectionString, 123); // Dummy user ID
             System.out.println("New DP ID received: " + newDpId);
         } catch (Exception e) {
             System.err.println("Error getting new DP ID: " + e.getMessage());
             e.printStackTrace();
         }

         // Add tests for other methods similarly...
         System.out.println("\nTesting getCenterListForDistrict...");
         try {
             List<Map<String, Object>> centers = bll.getCenterListForDistrict(testConnectionString, 5); // Dummy district ID
             System.out.println("Centers received: " + centers);
         } catch (Exception e) {
             System.err.println("Error getting centers: " + e.getMessage());
             e.printStackTrace();
         }

         System.out.println("\nTesting getInsurers...");
         try {
             List<Map<String, Object>> insurers = bll.getInsurers(testConnectionString, 0); // Dummy insurer ID (0 might mean all?)
             System.out.println("Insurers received: " + insurers);
         } catch (Exception e) {
             System.err.println("Error getting insurers: " + e.getMessage());
             e.printStackTrace();
         }

        System.out.println("\n--- Testing Complete ---");
    }
}