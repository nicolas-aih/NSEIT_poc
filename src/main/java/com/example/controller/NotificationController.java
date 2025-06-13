package com.example.controller;

import com.example.config.PortalSession;
import com.example.model.ApiResponse;
import com.example.model.Notification;
import com.example.service.NotificationService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

@Controller
@RequestMapping("/notifications")
public class NotificationController {

    @Autowired
    private NotificationService notificationService;

    @Autowired
    private PortalSession portalSession;

    @GetMapping
    @PreAuthorize("isAuthenticated()")
    public String getNotificationsPage(Model model) {
        model.addAttribute("isLoggedOn", portalSession.getUserId() != 0);
        model.addAttribute("className", "col-sm-9");
        return "notifications";
    }

    @PostMapping
    @PreAuthorize("isAuthenticated()")
    @ResponseBody
    public ResponseEntity<ApiResponse> saveNotification(@RequestBody Notification notification) {
        try {
            String message = notificationService.saveNotification(
                notification.getTitle(),
                notification.getDescription(),
                notification.getValidFrom(),
                notification.getValidTo(),
                notification.getNotificationType(),
                portalSession.getUserId()
            );

            if (message.isEmpty()) {
                return ResponseEntity.ok(new ApiResponse(true, "Notification saved successfully"));
            } else {
                return ResponseEntity.ok(new ApiResponse(false, message));
            }
        } catch (Exception e) {
            return ResponseEntity.ok(new ApiResponse(false, "Error saving notification"));
        }
    }
} 