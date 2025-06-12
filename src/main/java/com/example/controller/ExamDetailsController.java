package com.example.controller;

import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;

import com.example.service.ExamDetailsService;

@Controller
@RequestMapping("/exam-details")
public class ExamDetailsController {
    @Autowired
    private ExamDetailsService examDetailsService;

    @PostMapping("/save")
    public String saveExamDetails(@RequestParam String urn,
                                  @RequestParam String examDate,
                                  @RequestParam String examineeId,
                                  @RequestParam int marks,
                                  @RequestParam int result,
                                  @RequestParam int userId) {
        return examDetailsService.saveExamDetails(urn, examDate, examineeId, marks, result, userId);
    }

    @GetMapping("/get")
    public ResponseEntity<Map<String, Object>> getExamDetails(@RequestParam String urn) {
        Map<String, Object> result = examDetailsService.getExamDetails(urn);
        return ResponseEntity.ok(result);
    }
} 