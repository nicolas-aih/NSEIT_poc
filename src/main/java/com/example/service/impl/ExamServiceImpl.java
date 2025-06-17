package com.example.service.impl;

import com.example.model.Exam;
import com.example.repository.ExamRepository;
import com.example.service.ExamService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDateTime;
import java.util.List;
import java.util.Optional;

@Service
@Transactional
public class ExamServiceImpl implements ExamService {

    private final ExamRepository examRepository;

    @Autowired
    public ExamServiceImpl(ExamRepository examRepository) {
        this.examRepository = examRepository;
    }

    @Override
    public List<Exam> getAllExams() {
        return examRepository.findAll();
    }

    @Override
    public Optional<Exam> getExamById(Long id) {
        return examRepository.findById(id);
    }

    @Override
    public Exam createExam(Exam exam) {
        if (examRepository.existsByExamCode(exam.getExamCode())) {
            throw new IllegalArgumentException("Exam code already exists");
        }
        return examRepository.save(exam);
    }

    @Override
    public Exam updateExam(Long id, Exam exam) {
        if (!examRepository.existsById(id)) {
            throw new IllegalArgumentException("Exam not found");
        }
        exam.setId(id);
        return examRepository.save(exam);
    }

    @Override
    public void deleteExam(Long id) {
        examRepository.deleteById(id);
    }

    @Override
    public List<Exam> getExamsByStatus(String status) {
        return examRepository.findByStatus(status);
    }

    @Override
    public List<Exam> getExamsByType(String examType) {
        return examRepository.findByExamType(examType);
    }

    @Override
    public List<Exam> getActiveRegistrations() {
        return examRepository.findActiveRegistrations(LocalDateTime.now());
    }

    @Override
    public List<Exam> getOngoingExams() {
        return examRepository.findOngoingExams(LocalDateTime.now());
    }

    @Override
    public List<Exam> getUpcomingExams() {
        return examRepository.findUpcomingExams(LocalDateTime.now());
    }

    @Override
    public void updateExamStatus(Long id, String status) {
        Optional<Exam> examOpt = examRepository.findById(id);
        if (examOpt.isPresent()) {
            Exam exam = examOpt.get();
            exam.setStatus(status);
            examRepository.save(exam);
        } else {
            throw new IllegalArgumentException("Exam not found");
        }
    }

    @Override
    public boolean validateExamCode(String examCode) {
        return !examRepository.existsByExamCode(examCode);
    }

    @Override
    public long countActiveRegistrations() {
        return examRepository.countActiveRegistrations(LocalDateTime.now());
    }

    @Override
    public void publishExam(Long id) {
        updateExamStatus(id, "PUBLISHED");
    }

    @Override
    public void cancelExam(Long id) {
        updateExamStatus(id, "CANCELLED");
    }

    @Override
    public void extendRegistrationPeriod(Long id, LocalDateTime newEndDate) {
        Optional<Exam> examOpt = examRepository.findById(id);
        if (examOpt.isPresent()) {
            Exam exam = examOpt.get();
            exam.setRegistrationEndDate(newEndDate);
            examRepository.save(exam);
        } else {
            throw new IllegalArgumentException("Exam not found");
        }
    }

    @Override
    public void updateExamSchedule(Long id, LocalDateTime newStartDate, LocalDateTime newEndDate) {
        Optional<Exam> examOpt = examRepository.findById(id);
        if (examOpt.isPresent()) {
            Exam exam = examOpt.get();
            exam.setStartDate(newStartDate);
            exam.setEndDate(newEndDate);
            examRepository.save(exam);
        } else {
            throw new IllegalArgumentException("Exam not found");
        }
    }

    @Override
    public void updateExamFee(Long id, double newFee) {
        Optional<Exam> examOpt = examRepository.findById(id);
        if (examOpt.isPresent()) {
            Exam exam = examOpt.get();
            exam.setFee(java.math.BigDecimal.valueOf(newFee));
            examRepository.save(exam);
        } else {
            throw new IllegalArgumentException("Exam not found");
        }
    }

    @Override
    public void updateMaxCandidates(Long id, int newMaxCandidates) {
        Optional<Exam> examOpt = examRepository.findById(id);
        if (examOpt.isPresent()) {
            Exam exam = examOpt.get();
            exam.setMaxCandidates(newMaxCandidates);
            examRepository.save(exam);
        } else {
            throw new IllegalArgumentException("Exam not found");
        }
    }
} 