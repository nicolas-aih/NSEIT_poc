package com.example.service;

import com.example.model.Candidate;
import com.example.model.ExamDetail;
import com.example.model.URNRequest;
import com.example.dto.CandidateRegistrationDto;
import org.springframework.core.io.Resource;
import org.springframework.web.multipart.MultipartFile;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;

public interface CandidateService {
    // Scorecard Management
    Resource generateScorecard(String urn, LocalDate dob);

    // URN Management
    Candidate getCandidateByURN(String urn);
    Map<String, Object> modifyURN(URNRequest request);
    List<URNRequest> getPendingURNRequests(String status);
    Map<String, Object> approveURNRequest(Long id, String remarks);
    Map<String, Object> rejectURNRequest(Long id, String remarks);

    // Exam Management
    ExamDetail getExamDetails(String urn);
    Map<String, Object> saveExamDetails(ExamDetail examDetail);
    Map<String, Object> uploadExamResults(MultipartFile file, LocalDateTime fromDate, LocalDateTime toDate);

    // PAN Lookup
    List<Candidate> lookupByPAN(String panNumber);

    // Bulk Operations
    Map<String, Object> bulkUploadTrainingDetails(MultipartFile file);

    Map<String, Object> registerCandidate(CandidateRegistrationDto dto);
} 