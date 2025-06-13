package com.example.controller;

import com.example.config.PortalSession;
import com.example.model.ApiResponse;
import com.example.model.Notification;
import com.example.service.HomeService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@Controller
@RequestMapping("/")
public class HomeController {

    @Autowired
    private HomeService homeService;

    @Autowired
    private PortalSession portalSession;

    @GetMapping("/reset")
    public String reset() {
        // Clear session and logout
        portalSession.clear();
        return "redirect:/";
    }

    @GetMapping
    public String index(Model model) {
        try {
            List<Notification> notifications = homeService.getNotifications("N", portalSession.getRoleCode());
            model.addAttribute("notifications", notifications);
        } catch (Exception e) {
            // Log error but don't expose to user
        }

        model.addAttribute("isLoggedOn", portalSession.getUserId() != 0);
        model.addAttribute("className", "col-sm-6");
        return "index";
    }

    @GetMapping("/error")
    public String error(Model model) {
        model.addAttribute("isLoggedOn", portalSession.getUserId() != 0);
        return "error";
    }

    @GetMapping("/syllabus")
    public String syllabus(Model model) {
        model.addAttribute("isLoggedOn", portalSession.getUserId() != 0);
        model.addAttribute("className", "col-sm-6");
        return "syllabus";
    }

    @GetMapping("/exam-centers")
    public String examCenters(Model model) {
        model.addAttribute("isLoggedOn", portalSession.getUserId() != 0);
        model.addAttribute("className", "col-sm-6");
        return "exam-centers";
    }

    @GetMapping("/dashboard")
    @PreAuthorize("isAuthenticated()")
    public String dashboard(Model model) {
        model.addAttribute("isLoggedOn", portalSession.getUserId() != 0);
        model.addAttribute("className", "col-sm-9");
        return "dashboard";
    }

    @GetMapping("/unauthorized")
    public String unauthorizedAccess(Model model) {
        model.addAttribute("isLoggedOn", portalSession.getUserId() != 0);
        return "unauthorized";
    }

    @GetMapping("/relogin")
    public String relogin() {
        return "redirect:/login";
    }

    @GetMapping("/ticker")
    @ResponseBody
    public String getTicker() {
        return homeService.getTicker();
    }
} 