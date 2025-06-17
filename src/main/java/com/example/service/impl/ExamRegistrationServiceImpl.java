package com.example.service.impl;

import com.example.dto.ExamRegistrationDto;
import com.example.model.Exam;
import com.example.model.ExamCenter;
import com.example.model.ExamRegistration;
import com.example.model.User;
import com.example.repository.ExamRegistrationRepository;
import com.example.repository.ExamRepository;
import com.example.repository.ExamCenterRepository;
import com.example.repository.UserRepository;
import com.example.service.ExamRegistrationService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;
import java.util.UUID;

@Service
@Transactional
public class ExamRegistrationServiceImpl implements ExamRegistrationService {

    private final ExamRegistrationRepository registrationRepository;
    private final ExamRepository examRepository;
    private final ExamCenterRepository examCenterRepository;
    private final UserRepository userRepository;

    @Autowired
    public ExamRegistrationServiceImpl(
            ExamRegistrationRepository registrationRepository,
            ExamRepository examRepository,
            ExamCenterRepository examCenterRepository,
            UserRepository userRepository) {
        this.registrationRepository = registrationRepository;
        this.examRepository = examRepository;
        this.examCenterRepository = examCenterRepository;
        this.userRepository = userRepository;
    }

    @Override
    public ExamRegistration registerForExam(ExamRegistrationDto registrationDto, Long userId) {
        // Implementation needed
        throw new UnsupportedOperationException("Method not implemented");
    }

    @Override
    public ExamRegistration getRegistrationById(Long id) {
        // Implementation needed
        throw new UnsupportedOperationException("Method not implemented");
    }

    @Override
    public List<ExamRegistration> getRegistrationsByExamId(Long examId) {
        // Implementation needed
        throw new UnsupportedOperationException("Method not implemented");
    }

    @Override
    public List<ExamRegistration> getRegistrationsByUserId(Long userId) {
        // Implementation needed
        throw new UnsupportedOperationException("Method not implemented");
    }

    @Override
    public ExamRegistration updateRegistration(Long id, ExamRegistrationDto registrationDto) {
        // Implementation needed
        throw new UnsupportedOperationException("Method not implemented");
    }

    @Override
    public void cancelRegistration(Long id) {
        // Implementation needed
        throw new UnsupportedOperationException("Method not implemented");
    }

    @Override
    public void confirmPayment(Long id, String paymentReference) {
        // Implementation needed
        throw new UnsupportedOperationException("Method not implemented");
    }

    @Override
    public Map<String, Object> getRegistrationStatistics(Long examId) {
        // Implementation needed
        throw new UnsupportedOperationException("Method not implemented");
    }

    @Override
    public byte[] exportRegistrations(Long examId) {
        // Implementation needed
        throw new UnsupportedOperationException("Method not implemented");
    }

    @Override
    public List<ExamRegistration> getRegistrationsByExamCenterId(Long examCenterId) {
        return registrationRepository.findByExamCenterId(examCenterId);
    }

    @Override
    public List<ExamRegistration> getRegistrationsByStatus(String status) {
        return registrationRepository.findByStatus(status);
    }

    @Override
    public void updateRegistrationStatus(Long id, String status) {
        ExamRegistration registration = getRegistrationById(id);
        registration.setStatus(status);
        registrationRepository.save(registration);
    }

    @Override
    public void updatePaymentStatus(Long id, String paymentStatus, String transactionId) {
        ExamRegistration registration = getRegistrationById(id);
        registration.setPaymentStatus(paymentStatus);
        registration.setPaymentTransactionId(transactionId);
        registration.setPaymentDate(LocalDateTime.now());
        registrationRepository.save(registration);
    }

    @Override
    public void updateDocumentStatus(Long id, String documentStatus, String remarks) {
        ExamRegistration registration = getRegistrationById(id);
        registration.setDocumentStatus(documentStatus);
        registration.setDocumentRemarks(remarks);
        registrationRepository.save(registration);
    }

    @Override
    public void assignExamCenter(Long id, Long examCenterId, LocalDateTime examDate, String timeSlot) {
        ExamRegistration registration = getRegistrationById(id);
        registration.setExamCenter(examCenterRepository.findById(examCenterId)
                .orElseThrow(() -> new IllegalArgumentException("Exam center not found")));
        registration.setExamDate(examDate);
        registration.setExamTimeSlot(timeSlot);
        registrationRepository.save(registration);
    }

    @Override
    public void generateRollNumber(Long id) {
        ExamRegistration registration = getRegistrationById(id);
        registration.setRollNumber(generateUniqueRollNumber(registration));
        registrationRepository.save(registration);
    }

    @Override
    public void generateSeatNumber(Long id) {
        ExamRegistration registration = getRegistrationById(id);
        registration.setSeatNumber(generateUniqueSeatNumber(registration));
        registrationRepository.save(registration);
    }

    @Override
    public boolean validateRegistration(Long id) {
        ExamRegistration registration = getRegistrationById(id);
        return registration.getPaymentStatus().equals("PAID") &&
               registration.getDocumentStatus().equals("VERIFIED");
    }

    @Override
    public List<ExamRegistration> getRegistrationsByDateRange(LocalDateTime startDate, LocalDateTime endDate) {
        return registrationRepository.findByRegistrationDateRange(startDate, endDate);
    }

    @Override
    public long countApprovedRegistrations(Long examId) {
        return registrationRepository.countApprovedRegistrations(examId);
    }

    @Override
    public boolean isRegistrationOpen(Long examId) {
        Exam exam = examRepository.findById(examId)
                .orElseThrow(() -> new IllegalArgumentException("Exam not found"));
        LocalDateTime now = LocalDateTime.now();
        return now.isAfter(exam.getRegistrationStartDate()) &&
               now.isBefore(exam.getRegistrationEndDate()) &&
               exam.getStatus().equals("PUBLISHED");
    }

    @Override
    public boolean isExamCenterAvailable(Long examId, Long examCenterId, LocalDateTime examDate, String timeSlot) {
        List<ExamRegistration> existingRegistrations = registrationRepository.findByExamCenterAndTimeSlot(
                examId, examCenterId, examDate, timeSlot);
        Exam exam = examRepository.findById(examId)
                .orElseThrow(() -> new IllegalArgumentException("Exam not found"));
        ExamCenter examCenter = examCenterRepository.findById(examCenterId)
                .orElseThrow(() -> new IllegalArgumentException("Exam center not found"));
        
        return existingRegistrations.size() < examCenter.getCapacity();
    }

    @Override
    public void verifyDocuments(Long id) {
        ExamRegistration registration = getRegistrationById(id);
        registration.setDocumentStatus("VERIFIED");
        registrationRepository.save(registration);
    }

    @Override
    public void processRefund(Long id) {
        ExamRegistration registration = getRegistrationById(id);
        registration.setPaymentStatus("REFUNDED");
        registrationRepository.save(registration);
    }

    @Override
    public void sendConfirmationEmail(Long id) {
        // TODO: Implement email service
        ExamRegistration registration = getRegistrationById(id);
        // Send email with registration details
    }

    @Override
    public void generateHallTicket(Long id) {
        // Implementation needed
        throw new UnsupportedOperationException("Method not implemented");
    }

    private String generateRegistrationNumber() {
        return "REG" + UUID.randomUUID().toString().substring(0, 8).toUpperCase();
    }

    private String generateUniqueRollNumber(ExamRegistration registration) {
        return registration.getExam().getExamCode() + 
               String.format("%04d", registration.getId());
    }

    private String generateUniqueSeatNumber(ExamRegistration registration) {
        return registration.getExamCenter().getCenterCode() + 
               String.format("%03d", registration.getId());
    }
} 