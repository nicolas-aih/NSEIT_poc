package com.example.controller;

import com.example.config.PortalSession;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;

@Controller
@RequestMapping("/dp-range")
public class DPRangeController {

    @Autowired
    private PortalSession portalSession;

    @GetMapping
    @PreAuthorize("isAuthenticated()")
    public String getDPRange(Model model) {
        model.addAttribute("isLoggedOn", portalSession.getUserId() != 0);
        model.addAttribute("className", "col-sm-9");
        return "dp-range";
    }
} 