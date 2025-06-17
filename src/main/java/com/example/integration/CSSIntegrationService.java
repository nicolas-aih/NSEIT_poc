package com.example.integration;

import com.example.model.ExamDetail;
import com.example.model.URNRequest;
import java.util.List;
import java.util.Map;

public interface CSSIntegrationService {
    
    // Synchronize exam details with CSS
    void syncExamDetails(List<ExamDetail> examDetails);
    
    // Synchronize URN requests with CSS
    void syncURNRequests(List<URNRequest> urnRequests);
    
    // Get exam center details from CSS
    Map<String, Object> getExamCenterDetails(Long centerId);
    
    // Get candidate details from CSS
    Map<String, Object> getCandidateDetails(String urn);
    
    // Update exam results in CSS
    void updateExamResults(String urn, Integer marks, String result);
    
    // Validate URN in CSS
    boolean validateURN(String urn);
    
    // Get CSS system status
    Map<String, Object> getSystemStatus();
    
    // Retry failed synchronizations
    void retryFailedSyncs();
} 