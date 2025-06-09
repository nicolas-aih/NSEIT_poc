package com.example.model;

import com.example.database.Database;
import com.example.database.ParamLength;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

public class ExamCenters {
    // Add these constants
    private static final int INPUT = 1;
    private static final int OUTPUT = 2;
    private static final int INPUT_OUTPUT = 3;
    
    // Then update all ParameterDirection references to use these constants
    // For example:
    public ResultSet findNearestExamCenter(String connectionString, int pincode, int examBodyId) throws Exception {
        Database objDatabase = null;
        ResultSet objResultSet = null;
        int procReturnValue = 0;
        List<Object> allParameters = new ArrayList<>();
        
        try {
            String procedureName = "STP_CMN_FindNearestExamCenter2";
            String[] params = new String[] { "@intPINCode", "@tntExamBodyID" };
            int[] paramTypes = new int[] { java.sql.Types.INTEGER, java.sql.Types.TINYINT };
            ParamLength[] paramLengths = new ParamLength[] { 
                new ParamLength(4, 10, 0), 
                new ParamLength(1, 3, 0) 
            };
            // Change this line:
            int[] paramDirections = new int[] { INPUT, INPUT };
            // ... rest of your code ...
            Object[] values = new Object[] { pincode, examBodyId };
            
            objDatabase = new Database();
            ResultSet[] resultSetWrapper = new ResultSet[1];
            
            procReturnValue = objDatabase.execProcedure(
                connectionString, procedureName, params, paramTypes, paramLengths, 
                paramDirections, values, allParameters, resultSetWrapper, true);
            
            objResultSet = resultSetWrapper[0];
        } catch (Exception ex) {
            throw ex;
        } finally {
            objDatabase = null;
        }
        
        return objResultSet;
    }
    
    public ResultSet getExamCenters(String connectionString, int pincode, int examBodyId) throws Exception {
        Database objDatabase = null;
        ResultSet objResultSet = null;
        int procReturnValue = 0;
        List<Object> allParameters = new ArrayList<>();
        
        try {
            String procedureName = "STP_CMN_FindNearestExamCenter3";
            String[] params = new String[] { "@intPINCode", "@tntExamBodyID" };
            int[] paramTypes = new int[] { java.sql.Types.INTEGER, java.sql.Types.TINYINT };
            ParamLength[] paramLengths = new ParamLength[] { 
                new ParamLength(4, 10, 0), 
                new ParamLength(1, 3, 0) 
            };
            int[] paramDirections = new int[] { ParameterDirection.INPUT, ParameterDirection.INPUT };
            Object[] values = new Object[] { pincode, examBodyId };
            
            objDatabase = new Database();
            ResultSet[] resultSetWrapper = new ResultSet[1];
            
            procReturnValue = objDatabase.execProcedure(
                connectionString, procedureName, params, paramTypes, paramLengths, 
                paramDirections, values, allParameters, resultSetWrapper, true);
            
            objResultSet = resultSetWrapper[0];
        } catch (Exception ex) {
            throw ex;
        } finally {
            objDatabase = null;
        }
        
        return objResultSet;
    }
    
    public ResultSet examCentersForState(String connectionString, int stateId, boolean isTbxCenter) throws Exception {
        Database objDatabase = null;
        ResultSet objResultSet = null;
        int procReturnValue = 0;
        List<Object> allParameters = new ArrayList<>();
        
        try {
            String procedureName = "sp_get_examcenters";
            String[] params = new String[] { "@stateid", "@IsTBXCenter" };
            int[] paramTypes = new int[] { java.sql.Types.INTEGER, java.sql.Types.VARCHAR };
            ParamLength[] paramLengths = new ParamLength[] { 
                new ParamLength(4, 10, 0), 
                new ParamLength(1, 0, 0) 
            };
            int[] paramDirections = new int[] { ParameterDirection.INPUT, ParameterDirection.INPUT };
            Object[] values = new Object[] { stateId, isTbxCenter ? "Y" : "N" };
            
            objDatabase = new Database();
            ResultSet[] resultSetWrapper = new ResultSet[1];
            
            procReturnValue = objDatabase.execProcedure(
                connectionString, procedureName, params, paramTypes, paramLengths, 
                paramDirections, values, allParameters, resultSetWrapper, true);
            
            objResultSet = resultSetWrapper[0];
        } catch (Exception ex) {
            throw ex;
        } finally {
            objDatabase = null;
        }
        
        return objResultSet;
    }
    
    public ResultSet examCentersForState(String connectionString, int stateId, int centerId) throws Exception {
        Database objDatabase = null;
        ResultSet objResultSet = null;
        int procReturnValue = 0;
        List<Object> allParameters = new ArrayList<>();
        
        try {
            String procedureName = "sp_get_examcenters2";
            String[] params = new String[] { "@stateid", "@centerid" };
            int[] paramTypes = new int[] { java.sql.Types.INTEGER, java.sql.Types.INTEGER };
            ParamLength[] paramLengths = new ParamLength[] { 
                new ParamLength(4, 10, 0), 
                new ParamLength(4, 10, 0) 
            };
            int[] paramDirections = new int[] { ParameterDirection.INPUT, ParameterDirection.INPUT };
            Object[] values = new Object[] { 
                stateId <= 0 ? null : stateId, 
                centerId <= 0 ? null : centerId 
            };
            
            objDatabase = new Database();
            ResultSet[] resultSetWrapper = new ResultSet[1];
            
            procReturnValue = objDatabase.execProcedure(
                connectionString, procedureName, params, paramTypes, paramLengths, 
                paramDirections, values, allParameters, resultSetWrapper, true);
            
            objResultSet = resultSetWrapper[0];
        } catch (Exception ex) {
            throw ex;
        } finally {
            objDatabase = null;
        }
        
        return objResultSet;
    }
    
    public ResultSet examCentersForStateEx(String connectionString, int stateId, int centerId) throws Exception {
        Database objDatabase = null;
        ResultSet objResultSet = null;
        int procReturnValue = 0;
        List<Object> allParameters = new ArrayList<>();
        
        try {
            String procedureName = "sp_get_examcenters5";
            String[] params = new String[] { "@stateid", "@centerid" };
            int[] paramTypes = new int[] { java.sql.Types.INTEGER, java.sql.Types.INTEGER };
            ParamLength[] paramLengths = new ParamLength[] { 
                new ParamLength(4, 10, 0), 
                new ParamLength(4, 10, 0) 
            };
            int[] paramDirections = new int[] { ParameterDirection.INPUT, ParameterDirection.INPUT };
            Object[] values = new Object[] { 
                stateId <= 0 ? null : stateId, 
                centerId <= 0 ? null : centerId 
            };
            
            objDatabase = new Database();
            ResultSet[] resultSetWrapper = new ResultSet[1];
            
            procReturnValue = objDatabase.execProcedure(
                connectionString, procedureName, params, paramTypes, paramLengths, 
                paramDirections, values, allParameters, resultSetWrapper, true);
            
            objResultSet = resultSetWrapper[0];
        } catch (Exception ex) {
            throw ex;
        } finally {
            objDatabase = null;
        }
        
        return objResultSet;
    }
    
    public ResultSet examCentersForDownload(String connectionString) throws Exception {
        Database objDatabase = null;
        ResultSet objResultSet = null;
        int procReturnValue = 0;
        List<Object> allParameters = new ArrayList<>();
        
        try {
            String procedureName = "sp_get_examcenters4";
            String[] params = new String[] {};
            int[] paramTypes = new int[] {};
            ParamLength[] paramLengths = new ParamLength[] {};
            int[] paramDirections = new int[] {};
            Object[] values = new Object[] {};
            
            objDatabase = new Database();
            ResultSet[] resultSetWrapper = new ResultSet[1];
            
            procReturnValue = objDatabase.execProcedure(
                connectionString, procedureName, params, paramTypes, paramLengths, 
                paramDirections, values, allParameters, resultSetWrapper, true);
            
            objResultSet = resultSetWrapper[0];
        } catch (Exception ex) {
            throw ex;
        } finally {
            objDatabase = null;
        }
        
        return objResultSet;
    }
    
    public ResultSet similarExamCenters(String connectionString, int centerId) throws Exception {
        Database objDatabase = null;
        ResultSet objResultSet = null;
        int procReturnValue = 0;
        List<Object> allParameters = new ArrayList<>();
        
        try {
            String procedureName = "sp_get_examcenters3";
            String[] params = new String[] { "@centerid" };
            int[] paramTypes = new int[] { java.sql.Types.INTEGER };
            ParamLength[] paramLengths = new ParamLength[] { new ParamLength(4, 10, 0) };
            int[] paramDirections = new int[] { ParameterDirection.INPUT };
            Object[] values = new Object[] { centerId };
            
            objDatabase = new Database();
            ResultSet[] resultSetWrapper = new ResultSet[1];
            
            procReturnValue = objDatabase.execProcedure(
                connectionString, procedureName, params, paramTypes, paramLengths, 
                paramDirections, values, allParameters, resultSetWrapper, true);
            
            objResultSet = resultSetWrapper[0];
        } catch (Exception ex) {
            throw ex;
        } finally {
            objDatabase = null;
        }
        
        return objResultSet;
    }
    
    public String saveCenterDetails(String connectionString, int centerId, String centerName, 
                                  String centerCode, int cssCode, int examBody, 
                                  String addressLine1, String addressLine2, String addressLine3, 
                                  int districtId, int pincode, boolean isActive, 
                                  String centerType, int currentUser) throws Exception {
        
        Database objDatabase = null;
        int procReturnValue = 0;
        List<Object> allParameters = new ArrayList<>();
        String message = "";

        try {
            String procedureName = "STP_ADM_SaveExamCenterDetails_New";
            String[] params = new String[] { 
                "@sntExamCenterID", "@varExamCenterName", "@varExamCenterCode", 
                "@tntExamBodyID", "@varHouseNo", "@varStreet", "@varTown", 
                "@sntDistrictID", "@intPinCode", "@btIsActive", "@CurrentUser", 
                "@css_id", "@center_type", "@message" 
            };
            
            int[] paramTypes = new int[] { 
                java.sql.Types.SMALLINT, java.sql.Types.VARCHAR, java.sql.Types.VARCHAR, 
                java.sql.Types.TINYINT, java.sql.Types.VARCHAR, java.sql.Types.VARCHAR, 
                java.sql.Types.VARCHAR, java.sql.Types.SMALLINT, java.sql.Types.INTEGER, 
                java.sql.Types.INTEGER, java.sql.Types.INTEGER, java.sql.Types.INTEGER, 
                java.sql.Types.VARCHAR, java.sql.Types.VARCHAR 
            };
            
            ParamLength[] paramLengths = new ParamLength[] { 
                new ParamLength(2, 5, 0), new ParamLength(256, 0, 0), new ParamLength(5, 0, 0), 
                new ParamLength(1, 3, 0), new ParamLength(256, 0, 0), new ParamLength(256, 0, 0), 
                new ParamLength(256, 0, 0), new ParamLength(2, 5, 0), new ParamLength(4, 10, 0), 
                new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), new ParamLength(4, 10, 0), 
                new ParamLength(20, 0, 0), new ParamLength(255, 0, 0) 
            };
            
            int[] paramDirections = new int[] { 
                ParameterDirection.INPUT, ParameterDirection.INPUT, ParameterDirection.INPUT, 
                ParameterDirection.INPUT, ParameterDirection.INPUT, ParameterDirection.INPUT, 
                ParameterDirection.INPUT, ParameterDirection.INPUT, ParameterDirection.INPUT, 
                ParameterDirection.INPUT, ParameterDirection.INPUT, ParameterDirection.INPUT, 
                ParameterDirection.INPUT, ParameterDirection.OUTPUT 
            };
            
            Object[] values = new Object[] { 
                centerId, centerName, centerCode, examBody, addressLine1, addressLine2, 
                addressLine3, districtId, pincode, isActive ? 1 : 0, currentUser, 
                cssCode, centerType, null 
            };
            
            objDatabase = new Database();
            ResultSet[] resultSetWrapper = new ResultSet[1];
            
            procReturnValue = objDatabase.execProcedure(
                connectionString, procedureName, params, paramTypes, paramLengths, 
                paramDirections, values, allParameters, resultSetWrapper, false);
            
            message = (String) allParameters.get(13);
            
        } catch (Exception ex) {
            throw ex;
        } finally {
            objDatabase = null;
        }
        
        return message;
    }
}