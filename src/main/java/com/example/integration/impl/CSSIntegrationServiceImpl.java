package com.example.integration.impl;

import com.example.integration.CSSIntegrationService;
import com.example.model.ExamDetail;
import com.example.model.URNRequest;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpMethod;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;
import org.springframework.web.util.UriComponentsBuilder;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Service
public class CSSIntegrationServiceImpl implements CSSIntegrationService {

    private final RestTemplate restTemplate;
    
    @Value("${css.api.base-url}")
    private String cssBaseUrl;
    
    @Value("${css.api.key}")
    private String cssApiKey;
    
    public CSSIntegrationServiceImpl(RestTemplate restTemplate) {
        this.restTemplate = restTemplate;
    }
    
    @Override
    public void syncExamDetails(List<ExamDetail> examDetails) {
        String url = cssBaseUrl + "/api/exam-details/sync";
        HttpHeaders headers = createHeaders();
        HttpEntity<List<ExamDetail>> request = new HttpEntity<>(examDetails, headers);
        restTemplate.exchange(url, HttpMethod.POST, request, Void.class);
    }
    
    @Override
    public void syncURNRequests(List<URNRequest> urnRequests) {
        String url = cssBaseUrl + "/api/urn-requests/sync";
        HttpHeaders headers = createHeaders();
        HttpEntity<List<URNRequest>> request = new HttpEntity<>(urnRequests, headers);
        restTemplate.exchange(url, HttpMethod.POST, request, Void.class);
    }
    
    @Override
    public Map<String, Object> getExamCenterDetails(Long centerId) {
        String url = UriComponentsBuilder.fromHttpUrl(cssBaseUrl)
            .path("/api/exam-centers/{centerId}")
            .buildAndExpand(centerId)
            .toUriString();
            
        HttpHeaders headers = createHeaders();
        HttpEntity<?> request = new HttpEntity<>(headers);
        ResponseEntity<Map> response = restTemplate.exchange(
            url, HttpMethod.GET, request, Map.class);
            
        return response.getBody();
    }
    
    @Override
    public Map<String, Object> getCandidateDetails(String urn) {
        String url = UriComponentsBuilder.fromHttpUrl(cssBaseUrl)
            .path("/api/candidates/{urn}")
            .buildAndExpand(urn)
            .toUriString();
            
        HttpHeaders headers = createHeaders();
        HttpEntity<?> request = new HttpEntity<>(headers);
        ResponseEntity<Map> response = restTemplate.exchange(
            url, HttpMethod.GET, request, Map.class);
            
        return response.getBody();
    }
    
    @Override
    public void updateExamResults(String urn, Integer marks, String result) {
        String url = cssBaseUrl + "/api/exam-results/update";
        HttpHeaders headers = createHeaders();
        
        Map<String, Object> requestBody = new HashMap<>();
        requestBody.put("urn", urn);
        requestBody.put("marks", marks);
        requestBody.put("result", result);
        
        HttpEntity<Map<String, Object>> request = new HttpEntity<>(requestBody, headers);
        restTemplate.exchange(url, HttpMethod.POST, request, Void.class);
    }
    
    @Override
    public boolean validateURN(String urn) {
        String url = UriComponentsBuilder.fromHttpUrl(cssBaseUrl)
            .path("/api/urn/validate/{urn}")
            .buildAndExpand(urn)
            .toUriString();
            
        HttpHeaders headers = createHeaders();
        HttpEntity<?> request = new HttpEntity<>(headers);
        ResponseEntity<Boolean> response = restTemplate.exchange(
            url, HttpMethod.GET, request, Boolean.class);
            
        return response.getBody();
    }
    
    @Override
    public Map<String, Object> getSystemStatus() {
        String url = cssBaseUrl + "/api/system/status";
        HttpHeaders headers = createHeaders();
        HttpEntity<?> request = new HttpEntity<>(headers);
        ResponseEntity<Map> response = restTemplate.exchange(
            url, HttpMethod.GET, request, Map.class);
            
        return response.getBody();
    }
    
    @Override
    public void retryFailedSyncs() {
        String url = cssBaseUrl + "/api/sync/retry";
        HttpHeaders headers = createHeaders();
        HttpEntity<?> request = new HttpEntity<>(headers);
        restTemplate.exchange(url, HttpMethod.POST, request, Void.class);
    }
    
    private HttpHeaders createHeaders() {
        HttpHeaders headers = new HttpHeaders();
        headers.set("X-API-Key", cssApiKey);
        headers.set("Content-Type", "application/json");
        return headers;
    }
} 