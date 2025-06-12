package com.example.service;

import java.util.Map;
import java.util.UUID;

import org.springframework.stereotype.Service;

@Service
public class DuplicateURNService {
    public String generateDuplicateURN(Map<String, Object> applicantData, String roleCode, int userId) {
        // TODO: Implement logic to generate a duplicate URN
        return UUID.randomUUID().toString();
    }
} 