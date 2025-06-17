package com.example.service;

import com.example.dto.ExamRegistrationDto;
import com.example.model.ExamRegistration;
import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;

public interface ExamRegistrationService {
    ExamRegistration registerForExam(ExamRegistrationDto registrationDto, Long userId);
    
    ExamRegistration getRegistrationById(Long id);
    
    List<ExamRegistration> getRegistrationsByUserId(Long userId);
    
    List<ExamRegistration> getRegistrationsByExamId(Long examId);
    
    List<ExamRegistration> getRegistrationsByExamCenterId(Long examCenterId);
    
    List<ExamRegistration> getRegistrationsByStatus(String status);
    
    void updateRegistrationStatus(Long id, String status);
    
    void updatePaymentStatus(Long id, String paymentStatus, String transactionId);
    
    void updateDocumentStatus(Long id, String documentStatus, String remarks);
    
    void assignExamCenter(Long id, Long examCenterId, LocalDateTime examDate, String timeSlot);
    
    void generateRollNumber(Long id);
    
    void generateSeatNumber(Long id);
    
    boolean validateRegistration(Long id);
    
    void cancelRegistration(Long id);
    
    List<ExamRegistration> getRegistrationsByDateRange(LocalDateTime startDate, LocalDateTime endDate);
    
    long countApprovedRegistrations(Long examId);
    
    boolean isRegistrationOpen(Long examId);
    
    boolean isExamCenterAvailable(Long examId, Long examCenterId, LocalDateTime examDate, String timeSlot);
    
    void verifyDocuments(Long id);
    
    void processRefund(Long id);
    
    void sendConfirmationEmail(Long id);
    
    void generateHallTicket(Long id);
    
    ExamRegistration updateRegistration(Long id, ExamRegistrationDto registrationDto);
    
    void confirmPayment(Long id, String paymentReference);
    
    Map<String, Object> getRegistrationStatistics(Long examId);
    
    byte[] exportRegistrations(Long examId);
} 