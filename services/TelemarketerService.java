package com.example.services;

import com.example.interfaces.TelemarketerDataAccess;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Map;

// Corresponds to IIIBL.Telemarketer.cs
@Service
public class TelemarketerService {

    private final TelemarketerDataAccess telemarketerDataAccess;

    @Autowired
    public TelemarketerService(TelemarketerDataAccess telemarketerDataAccess) {
        this.telemarketerDataAccess = telemarketerDataAccess;
    }

    public String registerTelemarketer(String connectionString, int insurerUserId, int createdByUserId,
                                       String traiRegNo, String name, String address, String cpName,
                                       String cpEmailId, String cpContactNo, String dpName,
                                       String dpEmailId, String dpContactNo, String isActive) {
        try {
            return telemarketerDataAccess.registerTelemarketer(connectionString, insurerUserId, createdByUserId,
                                                              traiRegNo, name, address, cpName, cpEmailId,
                                                              cpContactNo, dpName, dpEmailId, dpContactNo, isActive);
        } catch (Exception ex) {
            throw ex; // Propagate exception as per original C#
        }
    }

    public List<Map<String, Object>> getTelemarketerData(String connectionString, int insurerUserId, long tmId, int isActive) {
        try {
            return telemarketerDataAccess.getTelemarketerData(connectionString, insurerUserId, tmId, isActive);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public String updateTelemarketer(String connectionString, int insurerUserId, int createdByUserId,
                                     long tmId, String traiRegNo, String name, String address, String cpName,
                                     String cpEmailId, String cpContactNo, String dpName,
                                     String dpEmailId, String dpContactNo, String isActive) {
        try {
            return telemarketerDataAccess.updateTelemarketer(connectionString, insurerUserId, createdByUserId,
                                                            tmId, traiRegNo, name, address, cpName, cpEmailId,
                                                            cpContactNo, dpName, dpEmailId, dpContactNo, isActive);
        } catch (Exception ex) {
            throw ex;
        }
    }

    public String deleteTelemarketer(String connectionString, int insurerUserId, int createdByUserId, long tmId) {
        // C# BL method name was DeleteTelemarkiter (typo), interface has DeleteTelemarketer
        // Assuming the interface name is correct.
        try {
            return telemarketerDataAccess.deleteTelemarketer(connectionString, insurerUserId, createdByUserId, tmId);
        } catch (Exception ex) {
            throw ex;
        }
    }
}