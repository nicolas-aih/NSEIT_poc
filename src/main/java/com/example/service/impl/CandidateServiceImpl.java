package com.example.service.impl;

import com.example.model.Candidate;
import com.example.model.ExamDetail;
import com.example.model.URNRequest;
import com.example.repository.CandidateRepository;
import com.example.repository.ExamDetailRepository;
import com.example.repository.URNRequestRepository;
import com.example.service.CandidateService;
import com.example.util.ExcelHelper;
import com.example.util.PDFGenerator;
import org.springframework.core.io.Resource;
import org.springframework.core.io.UrlResource;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.multipart.MultipartFile;

import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import com.example.dto.CandidateRegistrationDto;

@Service
public class CandidateServiceImpl implements CandidateService {

    private final CandidateRepository candidateRepository;
    private final URNRequestRepository urnRequestRepository;
    private final ExamDetailRepository examDetailRepository;
    private final JdbcTemplate jdbcTemplate;
    private final PDFGenerator pdfGenerator;
    private final ExcelHelper excelHelper;
    private final Path fileStorageLocation;

    public CandidateServiceImpl(
            CandidateRepository candidateRepository,
            URNRequestRepository urnRequestRepository,
            ExamDetailRepository examDetailRepository,
            JdbcTemplate jdbcTemplate,
            PDFGenerator pdfGenerator,
            ExcelHelper excelHelper,
            String fileStorageLocation) {
        this.candidateRepository = candidateRepository;
        this.urnRequestRepository = urnRequestRepository;
        this.examDetailRepository = examDetailRepository;
        this.jdbcTemplate = jdbcTemplate;
        this.pdfGenerator = pdfGenerator;
        this.excelHelper = excelHelper;
        this.fileStorageLocation = Paths.get(fileStorageLocation);
    }

    @Override
    public Resource generateScorecard(String urn, LocalDate dob) {
        try {
            Candidate candidate = candidateRepository.findByUrnAndDateOfBirth(urn, dob)
                .orElseThrow(() -> new RuntimeException("Candidate not found"));

            ExamDetail examDetail = examDetailRepository.findByCandidateAndStatus(candidate, "COMPLETED")
                .orElseThrow(() -> new RuntimeException("Exam details not found"));

            Path scorecardPath = pdfGenerator.generateScorecard(candidate, examDetail);
            Resource resource = new UrlResource(scorecardPath.toUri());

            if (resource.exists()) {
                return resource;
            } else {
                throw new RuntimeException("Could not generate scorecard");
            }
        } catch (Exception e) {
            throw new RuntimeException("Error generating scorecard", e);
        }
    }

    @Override
    public Candidate getCandidateByURN(String urn) {
        return candidateRepository.findByUrn(urn)
            .orElseThrow(() -> new RuntimeException("Candidate not found"));
    }

    @Override
    @Transactional
    public Map<String, Object> modifyURN(URNRequest request) {
        request.setStatus("PENDING");
        request.setRequestedBy(SecurityContextHolder.getContext().getAuthentication().getName());
        URNRequest savedRequest = urnRequestRepository.save(request);

        Map<String, Object> response = new HashMap<>();
        response.put("success", true);
        response.put("message", "URN modification request submitted successfully");
        response.put("requestId", savedRequest.getId());
        return response;
    }

    @Override
    public List<URNRequest> getPendingURNRequests(String status) {
        return urnRequestRepository.findByStatus(status);
    }

    @Override
    @Transactional
    public Map<String, Object> approveURNRequest(Long id, String remarks) {
        URNRequest request = urnRequestRepository.findById(id)
            .orElseThrow(() -> new RuntimeException("URN request not found"));

        request.setStatus("APPROVED");
        request.setRemarks(remarks);
        request.setProcessedBy(SecurityContextHolder.getContext().getAuthentication().getName());

        // Apply the changes to the candidate
        Candidate candidate = request.getCandidate();
        applyURNChanges(candidate, request);
        candidateRepository.save(candidate);

        Map<String, Object> response = new HashMap<>();
        response.put("success", true);
        response.put("message", "URN request approved successfully");
        return response;
    }

    @Override
    @Transactional
    public Map<String, Object> rejectURNRequest(Long id, String remarks) {
        URNRequest request = urnRequestRepository.findById(id)
            .orElseThrow(() -> new RuntimeException("URN request not found"));

        request.setStatus("REJECTED");
        request.setRemarks(remarks);
        request.setProcessedBy(SecurityContextHolder.getContext().getAuthentication().getName());
        urnRequestRepository.save(request);

        Map<String, Object> response = new HashMap<>();
        response.put("success", true);
        response.put("message", "URN request rejected successfully");
        return response;
    }

    @Override
    public ExamDetail getExamDetails(String urn) {
        Candidate candidate = getCandidateByURN(urn);
        return examDetailRepository.findFirstByCandidateOrderByExamDateDesc(candidate)
            .orElseThrow(() -> new RuntimeException("Exam details not found"));
    }

    @Override
    @Transactional
    public Map<String, Object> saveExamDetails(ExamDetail examDetail) {
        examDetail.setUpdatedBy(SecurityContextHolder.getContext().getAuthentication().getName());
        ExamDetail savedDetail = examDetailRepository.save(examDetail);

        Map<String, Object> response = new HashMap<>();
        response.put("success", true);
        response.put("message", "Exam details saved successfully");
        response.put("examDetailId", savedDetail.getId());
        return response;
    }

    @Override
    @Transactional
    public Map<String, Object> uploadExamResults(MultipartFile file, LocalDateTime fromDate, LocalDateTime toDate) {
        try {
            List<ExamDetail> examDetails = excelHelper.parseExamResults(file.getInputStream());
            examDetailRepository.saveAll(examDetails);

            Map<String, Object> response = new HashMap<>();
            response.put("success", true);
            response.put("message", "Exam results uploaded successfully");
            response.put("recordsProcessed", examDetails.size());
            return response;
        } catch (Exception e) {
            throw new RuntimeException("Error uploading exam results", e);
        }
    }

    @Override
    public List<Candidate> lookupByPAN(String panNumber) {
        return candidateRepository.findByPanNumber(panNumber);
    }

    @Override
    @Transactional
    public Map<String, Object> bulkUploadTrainingDetails(MultipartFile file) {
        try {
            List<Candidate> candidates = excelHelper.parseTrainingDetails(file.getInputStream());
            candidateRepository.saveAll(candidates);

            Map<String, Object> response = new HashMap<>();
            response.put("success", true);
            response.put("message", "Training details uploaded successfully");
            response.put("recordsProcessed", candidates.size());
            return response;
        } catch (Exception e) {
            throw new RuntimeException("Error uploading training details", e);
        }
    }

    @Override
    @Transactional
    public Map<String, Object> registerCandidate(CandidateRegistrationDto dto) {
        try {
            Candidate candidate = new Candidate();
            candidate.setName(dto.getName());
            candidate.setDateOfBirth(dto.getDob());
            candidate.setEmail(dto.getEmail());
            candidate.setMobileNumber(dto.getMobile());
            candidate.setCurrentHouseNumber(dto.getAddress()); // Simplified, adjust as needed
            candidate.setPanNumber(dto.getPan());
            candidate.setGender(dto.getGender());
            candidate.setCurrentState(dto.getStateId() != null ? dto.getStateId().toString() : null);
            candidate.setCurrentDistrict(dto.getCenterId() != null ? dto.getCenterId().toString() : null);
            candidate.setBasicQualification(dto.getQualification());
            candidate.setYearOfPassing(dto.getYearOfPassing() != null ? java.time.LocalDate.of(dto.getYearOfPassing(), 1, 1) : null);
            // Set other fields as needed
            candidateRepository.save(candidate);
            return Map.of("success", true, "message", "Candidate registered successfully");
        } catch (Exception e) {
            return Map.of("success", false, "message", "Registration failed: " + e.getMessage());
        }
    }

    private void applyURNChanges(Candidate candidate, URNRequest request) {
        // Parse the modified fields and apply changes
        String[] fields = request.getModifiedFields().split(",");
        String[] newValues = request.getNewValues().split(",");

        for (int i = 0; i < fields.length; i++) {
            String field = fields[i];
            String value = newValues[i];

            switch (field) {
                case "name":
                    candidate.setName(value);
                    break;
                case "fatherName":
                    candidate.setFatherName(value);
                    break;
                case "email":
                    candidate.setEmail(value);
                    break;
                case "mobileNumber":
                    candidate.setMobileNumber(value);
                    break;
                // Add more fields as needed
            }
        }
    }
} 