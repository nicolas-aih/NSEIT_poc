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

@Controller
public class ExamRegistrationController {

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
} 