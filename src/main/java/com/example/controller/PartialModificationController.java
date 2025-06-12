package com.example.controller;

import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;

import com.example.service.PartialModificationService;

@Controller
@RequestMapping("/partial-modification")
public class PartialModificationController {
    
    @Autowired
    private PartialModificationService partialModificationService;
    
    @PostMapping("/update")
    public ResponseEntity<Map<String, Object>> updatePartialDetails(@RequestBody Map<String, Object> request) {
        Map<String, Object> result = partialModificationService.processPartialModification(request);
        return ResponseEntity.ok(result);
    }
} 