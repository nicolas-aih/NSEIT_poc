package com.example.interfaces;

import java.util.List;
import java.util.Map;

// Corresponds to IIIDL.Telemarketer.cs
public interface TelemarketerDataAccess {

    /**
     * Corresponds to IIIDL.Telemarketer.RegisterTelemarketer
     * The implementation will execute "sp_insert_telemarketeer".
     * @return The 'Message' (error or success) from the stored procedure.
     */
    String registerTelemarketer(String connectionString, int insurerUserId, int createdByUserId,
                                String traiRegNo, String name, String address, String cpName,
                                String cpEmailId, String cpContactNo, String dpName,
                                String dpEmailId, String dpContactNo, String isActive);

    /**
     * Corresponds to IIIDL.Telemarketer.UpdateTelemarketer
     * The implementation will execute "sp_update_telemarketeer".
     * @return The 'Message' (error or success) from the stored procedure.
     */
    String updateTelemarketer(String connectionString, int insurerUserId, int createdByUserId,
                              long tmId, String traiRegNo, String name, String address, String cpName,
                              String cpEmailId, String cpContactNo, String dpName,
                              String dpEmailId, String dpContactNo, String isActive);

    /**
     * Corresponds to IIIDL.Telemarketer.DeleteTelemarketer
     * The implementation will execute "sp_delete_telemarketeer".
     * @return The 'Message' (error or success) from the stored procedure.
     */
    String deleteTelemarketer(String connectionString, int insurerUserId, int createdByUserId, long tmId);

    /**
     * Corresponds to IIIDL.Telemarketer.GetTelemarketerData
     * The implementation will execute "sp_get_telemarketeer".
     * @return List of Maps representing the DataSet (expected to have one DataTable).
     *         Returns null or an empty list if operation fails or no data.
     */
    List<Map<String, Object>> getTelemarketerData(String connectionString, int insurerUserId, long tmId, int isActive);
}