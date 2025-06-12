package com.example.controller;

import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import com.example.service.DuplicateURNService;

@RestController
@RequestMapping("/duplicate-urn")
public class DuplicateURNController {
    @Autowired
    private DuplicateURNService duplicateURNService;

    @PostMapping("/generate")
    public String generateDuplicateURN(@RequestBody Map<String, Object> applicantData,
                                       @RequestParam String roleCode,
                                       @RequestParam int userId) {
        return duplicateURNService.generateDuplicateURN(applicantData, roleCode, userId);
    }
} 