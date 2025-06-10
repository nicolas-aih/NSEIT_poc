package databases.sqlserver;

import java.util.List;
import java.util.Map;

// Represents the combined result from a stored procedure call
public class ProcedureResult {
    private int returnValue; // Scalar return value from the procedure
    private Object[] allParameters; // Array containing input and output parameters (output values populated)
    private List<Map<String, Object>> resultSetTable; // Represents the first result set (equivalent to DataTable 0)

    // Constructor when a result set and output parameters are expected
    public ProcedureResult(int returnValue, Object[] allParameters, List<Map<String, Object>> resultSetTable) {
        this.returnValue = returnValue;
        this.allParameters = allParameters;
        this.resultSetTable = resultSetTable;
    }

    // Constructor when only output parameters (no main result set) are expected
    public ProcedureResult(int returnValue, Object[] allParameters) {
        this(returnValue, allParameters, null);
    }

    public int getReturnValue() {
        return returnValue;
    }

    // This array will contain the final values of all parameters, including outputs
    public Object[] getAllParameters() {
        return allParameters;
    }

    public List<Map<String, Object>> getResultSetTable() {
        return resultSetTable;
    }
}