package com.example.controller;

import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import com.example.service.URNService;

@RestController
@RequestMapping("/urn")
public class URNController {
    @Autowired
    private URNService urnService;

    @PostMapping("/generate")
    public String generateURN(@RequestBody Map<String, Object> applicantData,
                              @RequestParam String roleCode,
                              @RequestParam int userId) {
        return urnService.generateURN(applicantData, roleCode, userId);
    }

    @GetMapping("/for-pan")
    public String getURNForPAN(@RequestParam String pan) {
        return urnService.getURNForPAN(pan);
    }

    @PostMapping("/unarchive")
    public String unarchiveURN(@RequestParam String urn) {
        return urnService.unarchiveURN(urn);
    }

    @GetMapping("/details")
    public String getURNDetails(@RequestParam String urn, @RequestParam int userId) {
        return urnService.getURNDetails(urn, userId);
    }

    @PostMapping("/save-or-update")
    public String saveOrUpdateURNDetails(@RequestBody Map<String, Object> applicantData,
                                         @RequestParam int userId) {
        return urnService.saveOrUpdateURNDetails(applicantData, userId);
    }

    @PostMapping("/delete")
    public String deleteURN(@RequestParam String urn, @RequestParam int userId) {
        return urnService.deleteURN(urn, userId);
    }

    @GetMapping("/qual-data")
    public String getQualData(@RequestParam String urn, @RequestParam int userId) {
        return urnService.getQualData(urn, userId);
    }

    @PostMapping("/upload-exam-center-update")
    public String uploadExamCenterUpdate(@RequestParam String filePath, @RequestParam int userId) {
        return urnService.uploadExamCenterUpdate(filePath, userId);
    }

    @PostMapping("/generate-duplicate")
    public String generateDuplicateURN(@RequestBody Map<String, Object> applicantData,
                                       @RequestParam String roleCode,
                                       @RequestParam int userId) {
        return urnService.generateDuplicateURN(applicantData, roleCode, userId);
    }

    @GetMapping("/validate-format")
    public boolean validateURNFormat(@RequestParam String urn) {
        return urnService.validateURNFormat(urn);
    }
} 