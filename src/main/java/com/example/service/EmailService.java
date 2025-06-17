package com.example.service;

public interface EmailService {
    void sendRegistrationConfirmation(String to, String registrationNumber, String examName);
    
    void sendPaymentConfirmation(String to, String registrationNumber, String amount);
    
    void sendDocumentVerificationStatus(String to, String registrationNumber, String status, String remarks);
    
    void sendHallTicket(String to, String registrationNumber, String examName);
    
    void sendRegistrationCancellation(String to, String registrationNumber, String examName);
    
    void sendRefundConfirmation(String to, String registrationNumber, String amount);
    
    void sendExamReminder(String to, String registrationNumber, String examName, String examDate);
} 