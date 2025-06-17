package com.example.services;

import com.example.interfaces.UtilityDataAccess;
// Import the nested DTOs specifically
import com.example.interfaces.UtilityDataAccess.CompanyDetails;
import com.example.interfaces.UtilityDataAccess.UserPassword;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

// Renamed from Utility to UtilityService
@Service
public class UtilityService {

    private final UtilityDataAccess utilityDataAccess;

    @Autowired
    public UtilityService(UtilityDataAccess utilityDataAccess) {
        this.utilityDataAccess = utilityDataAccess;
    }

    /**
     * Saves candidate profile details.
     * The 'message' out parameter from C# is directly returned by the data access layer method.
     */
    public String saveCandidateProfile(String connectionString, String updatedValue, String urn, int userId, String updateAction) {
        try {
            return utilityDataAccess.saveCandidateProfile(connectionString, updatedValue, urn, userId, updateAction);
        } catch (Exception ex) {
            // Original C# code: throw (ex);
            throw ex;
        }
    }

    /**
     * Gets company details.
     * The data access layer returns a CompanyDetails DTO containing both DataSet and message.
     */
    public CompanyDetails getCompanyDetails(String connectionString, String companyName) {
        try {
            return utilityDataAccess.getCompanyDetails(connectionString, companyName);
        } catch (Exception ex) {
            throw ex;
        }
    }

    /**
     * Gets user password.
     * The data access layer returns a UserPassword DTO containing both the message and password.
     */
    public UserPassword getUserPassword(String connectionString, String userLoginId) {
        try {
            return utilityDataAccess.getUserPassword(connectionString, userLoginId);
        } catch (Exception ex) {
            throw ex;
        }
    }
}