package com.example.repository;

import com.example.model.ExamRegistration;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.time.LocalDateTime;
import java.util.List;

@Repository
public interface ExamRegistrationRepository extends JpaRepository<ExamRegistration, Long> {
    
    List<ExamRegistration> findByUserId(Long userId);
    
    List<ExamRegistration> findByExamId(Long examId);
    
    List<ExamRegistration> findByExamCenterId(Long examCenterId);
    
    List<ExamRegistration> findByStatus(String status);
    
    List<ExamRegistration> findByPaymentStatus(String paymentStatus);
    
    List<ExamRegistration> findByDocumentStatus(String documentStatus);
    
    @Query("SELECT er FROM ExamRegistration er WHERE er.exam.id = :examId AND er.examCenter.id = :examCenterId AND er.examDate = :examDate AND er.examTimeSlot = :timeSlot")
    List<ExamRegistration> findByExamCenterAndTimeSlot(
        @Param("examId") Long examId,
        @Param("examCenterId") Long examCenterId,
        @Param("examDate") LocalDateTime examDate,
        @Param("timeSlot") String timeSlot
    );
    
    @Query("SELECT COUNT(er) FROM ExamRegistration er WHERE er.exam.id = :examId AND er.status = 'APPROVED'")
    long countApprovedRegistrations(@Param("examId") Long examId);
    
    @Query("SELECT er FROM ExamRegistration er WHERE er.registrationDate BETWEEN :startDate AND :endDate")
    List<ExamRegistration> findByRegistrationDateRange(
        @Param("startDate") LocalDateTime startDate,
        @Param("endDate") LocalDateTime endDate
    );
    
    boolean existsByRegistrationNumber(String registrationNumber);
    
    @Query("SELECT er FROM ExamRegistration er WHERE er.exam.id = :examId AND er.user.id = :userId")
    List<ExamRegistration> findByExamAndUser(
        @Param("examId") Long examId,
        @Param("userId") Long userId
    );
} 