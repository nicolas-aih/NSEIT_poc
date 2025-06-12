package com.example.controller;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.ModelAttribute;
import org.springframework.web.bind.annotation.PostMapping;

import com.example.model.TelemarketerRegistration;

@Controller
public class TelemarketerRegistrationController {

    @GetMapping("/register-telemarketer")
    public String showRegistrationForm(Model model) {
        model.addAttribute("telemarketerRegistration", new TelemarketerRegistration());
        return "register-telemarketer";
    }

    @PostMapping("/register-telemarketer")
    public String submitRegistration(@ModelAttribute TelemarketerRegistration telemarketerRegistration, Model model) {
        // TODO: Save registration to database
        model.addAttribute("message", "Registration successful!");
        return "register-telemarketer";
    }
} 