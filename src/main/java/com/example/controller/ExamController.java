package com.example.controller;

import com.example.model.Exam;
import com.example.service.ExamService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDateTime;
import java.util.List;

@RestController
@RequestMapping("/api/exams")
public class ExamController {

    private final ExamService examService;

    @Autowired
    public ExamController(ExamService examService) {
        this.examService = examService;
    }

    @GetMapping
    public ResponseEntity<List<Exam>> getAllExams() {
        return ResponseEntity.ok(examService.getAllExams());
    }

    @GetMapping("/{id}")
    public ResponseEntity<Exam> getExamById(@PathVariable Long id) {
        return examService.getExamById(id)
                .map(ResponseEntity::ok)
                .orElse(ResponseEntity.notFound().build());
    }

    @PostMapping
    public ResponseEntity<Exam> createExam(@RequestBody Exam exam) {
        return ResponseEntity.ok(examService.createExam(exam));
    }

    @PutMapping("/{id}")
    public ResponseEntity<Exam> updateExam(@PathVariable Long id, @RequestBody Exam exam) {
        return ResponseEntity.ok(examService.updateExam(id, exam));
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteExam(@PathVariable Long id) {
        examService.deleteExam(id);
        return ResponseEntity.ok().build();
    }

    @GetMapping("/status/{status}")
    public ResponseEntity<List<Exam>> getExamsByStatus(@PathVariable String status) {
        return ResponseEntity.ok(examService.getExamsByStatus(status));
    }

    @GetMapping("/type/{examType}")
    public ResponseEntity<List<Exam>> getExamsByType(@PathVariable String examType) {
        return ResponseEntity.ok(examService.getExamsByType(examType));
    }

    @GetMapping("/active")
    public ResponseEntity<List<Exam>> getActiveRegistrations() {
        return ResponseEntity.ok(examService.getActiveRegistrations());
    }

    @GetMapping("/ongoing")
    public ResponseEntity<List<Exam>> getOngoingExams() {
        return ResponseEntity.ok(examService.getOngoingExams());
    }

    @GetMapping("/upcoming")
    public ResponseEntity<List<Exam>> getUpcomingExams() {
        return ResponseEntity.ok(examService.getUpcomingExams());
    }

    @PutMapping("/{id}/status")
    public ResponseEntity<Void> updateExamStatus(@PathVariable Long id, @RequestParam String status) {
        examService.updateExamStatus(id, status);
        return ResponseEntity.ok().build();
    }

    @GetMapping("/validate-code")
    public ResponseEntity<Boolean> validateExamCode(@RequestParam String examCode) {
        return ResponseEntity.ok(examService.validateExamCode(examCode));
    }

    @GetMapping("/active-count")
    public ResponseEntity<Long> countActiveRegistrations() {
        return ResponseEntity.ok(examService.countActiveRegistrations());
    }

    @PutMapping("/{id}/publish")
    public ResponseEntity<Void> publishExam(@PathVariable Long id) {
        examService.publishExam(id);
        return ResponseEntity.ok().build();
    }

    @PutMapping("/{id}/cancel")
    public ResponseEntity<Void> cancelExam(@PathVariable Long id) {
        examService.cancelExam(id);
        return ResponseEntity.ok().build();
    }

    @PutMapping("/{id}/extend-registration")
    public ResponseEntity<Void> extendRegistrationPeriod(
            @PathVariable Long id,
            @RequestParam LocalDateTime newEndDate) {
        examService.extendRegistrationPeriod(id, newEndDate);
        return ResponseEntity.ok().build();
    }

    @PutMapping("/{id}/schedule")
    public ResponseEntity<Void> updateExamSchedule(
            @PathVariable Long id,
            @RequestParam LocalDateTime newStartDate,
            @RequestParam LocalDateTime newEndDate) {
        examService.updateExamSchedule(id, newStartDate, newEndDate);
        return ResponseEntity.ok().build();
    }

    @PutMapping("/{id}/fee")
    public ResponseEntity<Void> updateExamFee(
            @PathVariable Long id,
            @RequestParam double newFee) {
        examService.updateExamFee(id, newFee);
        return ResponseEntity.ok().build();
    }

    @PutMapping("/{id}/max-candidates")
    public ResponseEntity<Void> updateMaxCandidates(
            @PathVariable Long id,
            @RequestParam int newMaxCandidates) {
        examService.updateMaxCandidates(id, newMaxCandidates);
        return ResponseEntity.ok().build();
    }
} 