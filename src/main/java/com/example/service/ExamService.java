package com.example.service;

import com.example.model.Exam;
import java.time.LocalDateTime;
import java.util.List;
import java.util.Optional;

public interface ExamService {
    List<Exam> getAllExams();
    
    Optional<Exam> getExamById(Long id);
    
    Exam createExam(Exam exam);
    
    Exam updateExam(Long id, Exam exam);
    
    void deleteExam(Long id);
    
    List<Exam> getExamsByStatus(String status);
    
    List<Exam> getExamsByType(String examType);
    
    List<Exam> getActiveRegistrations();
    
    List<Exam> getOngoingExams();
    
    List<Exam> getUpcomingExams();
    
    void updateExamStatus(Long id, String status);
    
    boolean validateExamCode(String examCode);
    
    long countActiveRegistrations();
    
    void publishExam(Long id);
    
    void cancelExam(Long id);
    
    void extendRegistrationPeriod(Long id, LocalDateTime newEndDate);
    
    void updateExamSchedule(Long id, LocalDateTime newStartDate, LocalDateTime newEndDate);
    
    void updateExamFee(Long id, double newFee);
    
    void updateMaxCandidates(Long id, int newMaxCandidates);
} 