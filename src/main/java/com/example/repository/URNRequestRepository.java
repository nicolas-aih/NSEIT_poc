package com.example.repository;

import com.example.model.URNRequest;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface URNRequestRepository extends JpaRepository<URNRequest, Long> {
    
    // Find URN request by URN
    Optional<URNRequest> findByUrn(String urn);
    
    // Find URN requests by status
    List<URNRequest> findByStatus(String status);
    
    // Find URN requests by applicant name
    List<URNRequest> findByApplicantNameContainingIgnoreCase(String applicantName);
    
    // Find URN requests by email
    Optional<URNRequest> findByEmail(String email);
    
    // Find URN requests by phone number
    Optional<URNRequest> findByPhoneNumber(String phoneNumber);
    
    // Find URN requests by status and date range
    List<URNRequest> findByStatusAndCreatedAtBetween(
        String status, 
        java.time.LocalDateTime startDate, 
        java.time.LocalDateTime endDate
    );
    
    // Find pending URN requests
    List<URNRequest> findByStatusOrderByCreatedAtDesc(String status);
    
    // Check if URN exists
    boolean existsByUrn(String urn);
} 