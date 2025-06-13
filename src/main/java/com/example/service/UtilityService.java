package com.example.service;

import java.util.Map;

public interface UtilityService {
    Map<String, Object> saveCandidateProfile(Map<String, Object> profileData);
    Map<String, Object> loadCompanyDetails(Map<String, Object> requestData);
    Map<String, Object> getUserPassword(Map<String, Object> requestData);
} 