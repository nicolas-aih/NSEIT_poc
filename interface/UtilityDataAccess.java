package com.example.interfaces;

import java.util.List;
import java.util.Map;

// This interface corresponds to the methods in IIIDL.Utility.cs
// The concrete implementation of this interface will handle actual database calls.
public interface UtilityDataAccess {

    // --- Nested DTOs for return types ---
    class CompanyDetails { // Renamed from CompanyDetailsResult for brevity
        private List<Map<String, Object>> data; // Represents DataSet's first table
        private String message;

        public CompanyDetails(List<Map<String, Object>> data, String message) {
            this.data = data;
            this.message = message;
        }
        public List<Map<String, Object>> getData() { return data; }
        public String getMessage() { return message; }
        public void setData(List<Map<String,Object>> data) { this.data = data; }
        public void setMessage(String message) { this.message = message; }
    }

    class UserPassword { // Renamed from UserPasswordResult
        private String password; // The 'out String Password'
        private String message;  // The String returned by the C# method

        public UserPassword(String password, String message) {
            this.password = password;
            this.message = message;
        }
        public String getPassword() { return password; }
        public String getMessage() { return message; }
        public void setPassword(String password) { this.password = password; }
        public void setMessage(String message) { this.message = message; }
    }


    // --- Interface Methods ---

    /**
     * Corresponds to IIIDL.Utility.SaveCandidateProfile
     * The implementation will execute "STP_Details_update".
     * @return The 'Message' out parameter from the stored procedure.
     */
    String saveCandidateProfile(String connectionString, String updatedValue, String urn, int userId, String updateAction);

    /**
     * Corresponds to IIIDL.Utility.GetCompanyDetails
     * The implementation will execute "Sp_LoadCompanyDetails".
     * @return A CompanyDetails object containing the DataSet (as List<Map<String, Object>>) and 'Message' out parameter.
     */
    CompanyDetails getCompanyDetails(String connectionString, String companyName);

    /**
     * Corresponds to IIIDL.Utility.GetUserPassword
     * The implementation will execute "Sp_GetUserPassword".
     * @return A UserPassword object containing the main returned message String and the 'Password' out parameter.
     */
    UserPassword getUserPassword(String connectionString, String userLoginId);
}