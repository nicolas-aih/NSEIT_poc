package com.example.repository;

import com.example.model.Exam;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.time.LocalDateTime;
import java.util.List;

@Repository
public interface ExamRepository extends JpaRepository<Exam, Long> {
    
    List<Exam> findByStatus(String status);
    
    List<Exam> findByExamType(String examType);
    
    @Query("SELECT e FROM Exam e WHERE e.registrationStartDate <= :currentDate AND e.registrationEndDate >= :currentDate")
    List<Exam> findActiveRegistrations(@Param("currentDate") LocalDateTime currentDate);
    
    @Query("SELECT e FROM Exam e WHERE e.startDate <= :currentDate AND e.endDate >= :currentDate")
    List<Exam> findOngoingExams(@Param("currentDate") LocalDateTime currentDate);
    
    @Query("SELECT e FROM Exam e WHERE e.startDate > :currentDate")
    List<Exam> findUpcomingExams(@Param("currentDate") LocalDateTime currentDate);
    
    boolean existsByExamCode(String examCode);
    
    @Query("SELECT COUNT(e) FROM Exam e WHERE e.status = 'PUBLISHED' AND e.registrationStartDate <= :currentDate AND e.registrationEndDate >= :currentDate")
    long countActiveRegistrations(@Param("currentDate") LocalDateTime currentDate);
} 