package com.example.database;

import java.sql.*;
import java.util.ArrayList;
import java.util.List;
import javax.sql.DataSource;
import java.sql.CallableStatement;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Types;

public class Database {
    public int execProcedure(String connectionString, String procedureName, 
                           String[] params, int[] paramTypes, ParamLength[] paramLengths, 
                           int[] paramDirections, Object[] values, List<Object> allParameters, 
                           ResultSet[] resultSet, boolean returnDataSet) throws SQLException {
        
        Connection connection = null;
        CallableStatement callableStatement = null;
        ResultSet rs = null;
        int returnValue = 0;

        try {
            // Establish connection
            connection = DriverManager.getConnection(connectionString);
            
            // Prepare the stored procedure call
            String call = "{call " + procedureName + "(";
            for (int i = 0; i < params.length; i++) {
                if (i > 0) call += ", ";
                call += "?";
            }
            call += ")}";
            
            callableStatement = connection.prepareCall(call);
            
            // Set parameters
            for (int i = 0; i < params.length; i++) {
                int direction = paramDirections[i];
                int type = paramTypes[i];
                Object value = values[i];
                
                if (direction == ParameterDirection.INPUT) {
                    if (value == null) {
                        callableStatement.setNull(params[i], type);
                    } else {
                        callableStatement.setObject(params[i], value, type);
                    }
                } else if (direction == ParameterDirection.OUTPUT) {
                    callableStatement.registerOutParameter(params[i], type);
                } else if (direction == ParameterDirection.INPUT_OUTPUT) {
                    callableStatement.setObject(params[i], value, type);
                    callableStatement.registerOutParameter(params[i], type);
                }
            }
            
            // Execute the stored procedure
            boolean hasResults = callableStatement.execute();
            
            // Handle output parameters
            if (allParameters != null) {
                allParameters.clear();
                for (int i = 0; i < params.length; i++) {
                    if (paramDirections[i] != ParameterDirection.INPUT) {
                        allParameters.add(callableStatement.getObject(params[i]));
                    } else {
                        allParameters.add(values[i]);
                    }
                }
            }
            
            // Handle result set if needed
            if (returnDataSet && hasResults) {
                rs = callableStatement.getResultSet();
                resultSet[0] = rs;
            }
            
            returnValue = callableStatement.getUpdateCount();
            
        } finally {
            // Note: We don't close the ResultSet here if we're returning it
            if (!returnDataSet && rs != null) {
                try { rs.close(); } catch (SQLException e) { /* ignore */ }
            }
            if (callableStatement != null) {
                try { callableStatement.close(); } catch (SQLException e) { /* ignore */ }
            }
            if (connection != null) {
                try { connection.close(); } catch (SQLException e) { /* ignore */ }
            }
        }
        
        return returnValue;
    }
}

class ParameterDirection {
    public static final int INPUT = 1;
    public static final int OUTPUT = 2;
    public static final int INPUT_OUTPUT = 3;
}