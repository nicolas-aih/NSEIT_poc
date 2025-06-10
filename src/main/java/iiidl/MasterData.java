// File: iiidl/MasterData.java
package iiidl;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

// This is a STUB/MOCK class for the IIIDL layer for translation purposes.
// In a real application, this class would interact with the database
// using JDBC or an ORM and return actual data.
public class MasterData {

    // Simulate getting states
    public List<Map<String, Object>> GetStates(String connectionString) {
        System.out.println("IIIDL.MasterData.GetStates called with connectionString: " + connectionString);
        // Simulate returning data
        List<Map<String, Object>> states = new ArrayList<>();
        Map<String, Object> state1 = new HashMap<>();
        state1.put("StateId", 1);
        state1.put("StateName", "California");
        states.add(state1);
        Map<String, Object> state2 = new HashMap<>();
        state2.put("StateId", 2);
        state2.put("StateName", "Texas");
        states.add(state2);
        return states;
    }

    // Simulate getting notifications
    public List<Map<String, Object>> GetNotifications(String connectionString, String notificationType, String roleCode) {
        System.out.println("IIIDL.MasterData.GetNotifications called with connectionString: " + connectionString + ", notificationType: " + notificationType + ", roleCode: " + roleCode);
        // Simulate returning data
        List<Map<String, Object>> notifications = new ArrayList<>();
        Map<String, Object> notif1 = new HashMap<>();
        notif1.put("NotificationId", 101);
        notif1.put("Message", "Welcome!");
        notifications.add(notif1);
        return notifications;
    }

    // Simulate getting generic master data
    public List<Map<String, Object>> GetMasterData(String connectionString) {
         System.out.println("IIIDL.MasterData.GetMasterData called with connectionString: " + connectionString);
         // Simulate returning data
         List<Map<String, Object>> masterData = new ArrayList<>();
         Map<String, Object> data1 = new HashMap<>();
         data1.put("Key", "SomeSetting");
         data1.put("Value", "123");
         masterData.add(data1);
         return masterData;
    }

    // Simulate getting center list for district
    public List<Map<String, Object>> GetCenterListForDistrict(String connectionString, int districtId) {
        System.out.println("IIIDL.MasterData.GetCenterListForDistrict called with connectionString: " + connectionString + ", districtId: " + districtId);
        // Simulate returning data
        List<Map<String, Object>> centers = new ArrayList<>();
        Map<String, Object> center1 = new HashMap<>();
        center1.put("CenterId", 50);
        center1.put("CenterName", "Downtown Center");
        centers.add(center1);
        return centers;
    }

    // Simulate getting insurers
    public List<Map<String, Object>> GetInsurers(String connectionString, int insurerId) {
         System.out.println("IIIDL.MasterData.GetInsurers called with connectionString: " + connectionString + ", insurerId: " + insurerId);
         // Simulate returning data
         List<Map<String, Object>> insurers = new ArrayList<>();
         Map<String, Object> insurer1 = new HashMap<>();
         insurer1.put("InsurerId", 10);
         insurer1.put("InsurerName", "Insurer A");
         insurers.add(insurer1);
         return insurers;
    }

    // Simulate getting insurers (version 2)
    public List<Map<String, Object>> GetInsurers2(String connectionString, int insurerId) {
         System.out.println("IIIDL.MasterData.GetInsurers2 called with connectionString: " + connectionString + ", insurerId: " + insurerId);
         // Simulate returning data
         List<Map<String, Object>> insurers = new ArrayList<>();
         Map<String, Object> insurer1 = new HashMap<>();
         insurer1.put("InsurerId", 20);
         insurer1.put("InsurerName", "Insurer B (v2)");
         insurers.add(insurer1);
         return insurers;
    }

    // Simulate getting DP for Insurer
    public List<Map<String, Object>> GetDPForInsurer(String connectionString, int insurerId, int dpId) {
        System.out.println("IIIDL.MasterData.GetDPForInsurer called with connectionString: " + connectionString + ", insurerId: " + insurerId + ", dpId: " + dpId);
        // Simulate returning data
        List<Map<String, Object>> dps = new ArrayList<>();
        Map<String, Object> dp1 = new HashMap<>();
        dp1.put("DPId", 100);
        dp1.put("DPName", "DP Alpha");
        dps.add(dp1);
        return dps;
    }

    // Simulate getting AC for DPs
    public List<Map<String, Object>> GetACForDPs(String connectionString, int insurerId, int dpId, int acId) {
        System.out.println("IIIDL.MasterData.GetACForDPs called with connectionString: " + connectionString + ", insurerId: " + insurerId + ", dpId: " + dpId + ", acId: " + acId);
        // Simulate returning data
        List<Map<String, Object>> acs = new ArrayList<>();
        Map<String, Object> ac1 = new HashMap<>();
        ac1.put("ACId", 200);
        ac1.put("ACName", "AC One");
        acs.add(ac1);
        return acs;
    }

    // Simulate getting Exam Center
    public List<Map<String, Object>> GetExamCenter(String connectionString, short examBodyId, short examCenterId) {
        System.out.println("IIIDL.MasterData.GetExamCenter called with connectionString: " + connectionString + ", examBodyId: " + examBodyId + ", examCenterId: " + examCenterId);
        // Simulate returning data
        List<Map<String, Object>> centers = new ArrayList<>();
        Map<String, Object> center1 = new HashMap<>();
        center1.put("ExamCenterId", 300);
        center1.put("CenterLocation", "Test City");
        centers.add(center1);
        return centers;
    }

    // Simulate getting new DP ID
    public int GetNewDPId(String connectionString, int insurerUserId) {
        System.out.println("IIIDL.MasterData.GetNewDPId called with connectionString: " + connectionString + ", insurerUserId: " + insurerUserId);
        // Simulate returning a new ID
        return 999; // Dummy ID
    }

    // Simulate getting company list for voucher entry
    public List<Map<String, Object>> GetCompanyListForVoucherEntry(String connectionString, String companyType) {
        System.out.println("IIIDL.MasterData.GetCompanyListForVoucherEntry called with connectionString: " + connectionString + ", companyType: " + companyType);
        // Simulate returning data
        List<Map<String, Object>> companies = new ArrayList<>();
        Map<String, Object> company1 = new HashMap<>();
        company1.put("CompanyId", 400);
        company1.put("CompanyName", "Comp A");
        companies.add(company1);
        return companies;
    }
}