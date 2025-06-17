package com.example.controller;

import java.util.HashMap;
import java.util.Map;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.ModelAttribute;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.multipart.MultipartFile;

import com.example.model.ExamRegistration;
import com.example.dto.ExamRegistrationDto;
import com.example.service.ExamRegistrationService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDateTime;
import java.util.List;

@Controller
public class ExamRegistrationController {

    private final ExamRegistrationService registrationService;

    @Autowired
    public ExamRegistrationController(ExamRegistrationService registrationService) {
        this.registrationService = registrationService;
    }

    @GetMapping("/register-exam")
    public String showRegistrationForm(Model model) {
        model.addAttribute("examRegistration", new ExamRegistration());
        return "register-exam";
    }

    @PostMapping("/register-exam")
    public String submitRegistration(@ModelAttribute ExamRegistration examRegistration, Model model) {
        // Validate required fields
        if (examRegistration.getName() == null || examRegistration.getName().trim().isEmpty()) {
            model.addAttribute("error", "Full Name is required.");
            return "register-exam";
        }
        if (examRegistration.getAddress() == null || examRegistration.getAddress().trim().isEmpty()) {
            model.addAttribute("error", "Address is required.");
            return "register-exam";
        }
        if (examRegistration.getTraiRegNo() == null || examRegistration.getTraiRegNo().trim().isEmpty()) {
            model.addAttribute("error", "TRAI Reg. No. is required.");
            return "register-exam";
        }
        if (examRegistration.getIsActive() == null || examRegistration.getIsActive().trim().isEmpty()) {
            model.addAttribute("error", "Is Active? is required.");
            return "register-exam";
        }
        if (examRegistration.getDpName() == null || examRegistration.getDpName().trim().isEmpty()) {
            model.addAttribute("error", "Designated Person's Name is required.");
            return "register-exam";
        }
        if (examRegistration.getDpEmailId() == null || examRegistration.getDpEmailId().trim().isEmpty()) {
            model.addAttribute("error", "Designated Person's Email Id is required.");
            return "register-exam";
        }
        if (examRegistration.getDpContactNo() == null || examRegistration.getDpContactNo().trim().isEmpty()) {
            model.addAttribute("error", "Designated Person's Mobile No. is required.");
            return "register-exam";
        }
        if (examRegistration.getCpName() == null || examRegistration.getCpName().trim().isEmpty()) {
            model.addAttribute("error", "Contact Person's Name is required.");
            return "register-exam";
        }
        if (examRegistration.getCpEmailId() == null || examRegistration.getCpEmailId().trim().isEmpty()) {
            model.addAttribute("error", "Contact Person's Email Id is required.");
            return "register-exam";
        }
        if (examRegistration.getCpContactNo() == null || examRegistration.getCpContactNo().trim().isEmpty()) {
            model.addAttribute("error", "Contact Person's Mobile No. is required.");
            return "register-exam";
        }

        // TODO: Save registration to database
        model.addAttribute("message", "Registration successful!");
        return "register-exam";
    }

    @GetMapping("/urn-creation")
    public String showURNCreationForm(Model model) {
        return "urn-creation";
    }

    @PostMapping("/urn-creation")
    @ResponseBody
    public Map<String, Object> createURN(@RequestParam String dummy) {
        Map<String, Object> response = new HashMap<>();
        // TODO: Implement URN creation logic
        response.put("status", true);
        response.put("message", "URN created successfully");
        return response;
    }

    @GetMapping("/exam-details")
    public String showExamDetailsForm(Model model) {
        return "exam-details";
    }

    @PostMapping("/exam-details")
    @ResponseBody
    public Map<String, Object> getExamDetails(@RequestParam String urn) {
        Map<String, Object> response = new HashMap<>();
        // TODO: Implement exam details retrieval logic
        response.put("status", true);
        response.put("message", "Exam details retrieved successfully");
        return response;
    }

    @PostMapping("/save-exam-details")
    @ResponseBody
    public Map<String, Object> saveExamDetails(@RequestParam MultipartFile file) {
        Map<String, Object> response = new HashMap<>();
        // TODO: Implement exam details saving logic
        response.put("status", true);
        response.put("message", "Exam details saved successfully");
        return response;
    }

    @PostMapping("/api/registrations")
    public ResponseEntity<ExamRegistration> registerForExam(
            @RequestBody ExamRegistrationDto registrationDto,
            @RequestParam Long userId) {
        return ResponseEntity.ok(registrationService.registerForExam(registrationDto, userId));
    }

    @GetMapping("/api/registrations/{id}")
    public ResponseEntity<ExamRegistration> getRegistrationById(@PathVariable Long id) {
        return ResponseEntity.ok(registrationService.getRegistrationById(id));
    }

    @GetMapping("/api/registrations/user/{userId}")
    public ResponseEntity<List<ExamRegistration>> getRegistrationsByUserId(@PathVariable Long userId) {
        return ResponseEntity.ok(registrationService.getRegistrationsByUserId(userId));
    }

    @GetMapping("/api/registrations/exam/{examId}")
    public ResponseEntity<List<ExamRegistration>> getRegistrationsByExamId(@PathVariable Long examId) {
        return ResponseEntity.ok(registrationService.getRegistrationsByExamId(examId));
    }

    @GetMapping("/api/registrations/center/{examCenterId}")
    public ResponseEntity<List<ExamRegistration>> getRegistrationsByExamCenterId(@PathVariable Long examCenterId) {
        return ResponseEntity.ok(registrationService.getRegistrationsByExamCenterId(examCenterId));
    }

    @GetMapping("/api/registrations/status/{status}")
    public ResponseEntity<List<ExamRegistration>> getRegistrationsByStatus(@PathVariable String status) {
        return ResponseEntity.ok(registrationService.getRegistrationsByStatus(status));
    }

    @PutMapping("/api/registrations/{id}/status")
    public ResponseEntity<Void> updateRegistrationStatus(
            @PathVariable Long id,
            @RequestParam String status) {
        registrationService.updateRegistrationStatus(id, status);
        return ResponseEntity.ok().build();
    }

    @PutMapping("/api/registrations/{id}/payment")
    public ResponseEntity<Void> updatePaymentStatus(
            @PathVariable Long id,
            @RequestParam String paymentStatus,
            @RequestParam String transactionId) {
        registrationService.updatePaymentStatus(id, paymentStatus, transactionId);
        return ResponseEntity.ok().build();
    }

    @PutMapping("/api/registrations/{id}/documents")
    public ResponseEntity<Void> updateDocumentStatus(
            @PathVariable Long id,
            @RequestParam String documentStatus,
            @RequestParam(required = false) String remarks) {
        registrationService.updateDocumentStatus(id, documentStatus, remarks);
        return ResponseEntity.ok().build();
    }

    @PutMapping("/api/registrations/{id}/assign-center")
    public ResponseEntity<Void> assignExamCenter(
            @PathVariable Long id,
            @RequestParam Long examCenterId,
            @RequestParam LocalDateTime examDate,
            @RequestParam String timeSlot) {
        registrationService.assignExamCenter(id, examCenterId, examDate, timeSlot);
        return ResponseEntity.ok().build();
    }

    @PutMapping("/api/registrations/{id}/generate-roll")
    public ResponseEntity<Void> generateRollNumber(@PathVariable Long id) {
        registrationService.generateRollNumber(id);
        return ResponseEntity.ok().build();
    }

    @PutMapping("/api/registrations/{id}/generate-seat")
    public ResponseEntity<Void> generateSeatNumber(@PathVariable Long id) {
        registrationService.generateSeatNumber(id);
        return ResponseEntity.ok().build();
    }

    @GetMapping("/api/registrations/{id}/validate")
    public ResponseEntity<Boolean> validateRegistration(@PathVariable Long id) {
        return ResponseEntity.ok(registrationService.validateRegistration(id));
    }

    @PutMapping("/api/registrations/{id}/cancel")
    public ResponseEntity<Void> cancelRegistration(@PathVariable Long id) {
        registrationService.cancelRegistration(id);
        return ResponseEntity.ok().build();
    }

    @GetMapping("/api/registrations/date-range")
    public ResponseEntity<List<ExamRegistration>> getRegistrationsByDateRange(
            @RequestParam LocalDateTime startDate,
            @RequestParam LocalDateTime endDate) {
        return ResponseEntity.ok(registrationService.getRegistrationsByDateRange(startDate, endDate));
    }

    @GetMapping("/api/registrations/exam/{examId}/approved-count")
    public ResponseEntity<Long> countApprovedRegistrations(@PathVariable Long examId) {
        return ResponseEntity.ok(registrationService.countApprovedRegistrations(examId));
    }

    @GetMapping("/api/registrations/exam/{examId}/registration-open")
    public ResponseEntity<Boolean> isRegistrationOpen(@PathVariable Long examId) {
        return ResponseEntity.ok(registrationService.isRegistrationOpen(examId));
    }

    @GetMapping("/api/registrations/check-availability")
    public ResponseEntity<Boolean> isExamCenterAvailable(
            @RequestParam Long examId,
            @RequestParam Long examCenterId,
            @RequestParam LocalDateTime examDate,
            @RequestParam String timeSlot) {
        return ResponseEntity.ok(registrationService.isExamCenterAvailable(
                examId, examCenterId, examDate, timeSlot));
    }

    @PutMapping("/api/registrations/{id}/verify-documents")
    public ResponseEntity<Void> verifyDocuments(@PathVariable Long id) {
        registrationService.verifyDocuments(id);
        return ResponseEntity.ok().build();
    }

    @PutMapping("/api/registrations/{id}/process-refund")
    public ResponseEntity<Void> processRefund(@PathVariable Long id) {
        registrationService.processRefund(id);
        return ResponseEntity.ok().build();
    }

    @PostMapping("/api/registrations/{id}/send-confirmation")
    public ResponseEntity<Void> sendConfirmationEmail(@PathVariable Long id) {
        registrationService.sendConfirmationEmail(id);
        return ResponseEntity.ok().build();
    }

    @PostMapping("/api/registrations/{id}/generate-hall-ticket")
    public ResponseEntity<Void> generateHallTicket(@PathVariable Long id) {
        registrationService.generateHallTicket(id);
        return ResponseEntity.ok().build();
    }
} 