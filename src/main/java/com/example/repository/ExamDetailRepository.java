package com.example.repository;

import com.example.model.ExamDetail;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface ExamDetailRepository extends JpaRepository<ExamDetail, Long> {
    
    // Find exam details by URN
    Optional<ExamDetail> findByUrn(String urn);
    
    // Find exam details by exam date
    List<ExamDetail> findByExamDate(java.time.LocalDateTime examDate);
    
    // Find exam details by exam center ID
    List<ExamDetail> findByExamCenterId(Long examCenterId);
    
    // Find exam details by status
    List<ExamDetail> findByStatus(String status);
    
    // Find exam details by URN and status
    Optional<ExamDetail> findByUrnAndStatus(String urn, String status);
    
    // Find exam details by exam date range
    List<ExamDetail> findByExamDateBetween(java.time.LocalDateTime startDate, java.time.LocalDateTime endDate);
} 