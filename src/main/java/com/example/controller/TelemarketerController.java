package com.example.controller;

import com.example.dto.TelemarketerDto;
import com.example.service.TelemarketerService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/telemarketer")
public class TelemarketerController {

    @Autowired
    private TelemarketerService telemarketerService;

    @GetMapping("/register")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<?> registerTelemarketerForm() {
        // In a real app, this would return a view or form data. Here, just a stub.
        return ResponseEntity.ok("Register Telemarketer Form Stub");
    }

    @PostMapping("/register")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<?> registerTelemarketer(@RequestBody TelemarketerDto telemarketerDto) {
        return ResponseEntity.ok(telemarketerService.registerTelemarketer(telemarketerDto));
    }

    @GetMapping("/manage")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<?> manageTelemarketerForm() {
        // In a real app, this would return a view or form data. Here, just a stub.
        return ResponseEntity.ok("Manage Telemarketer Form Stub");
    }

    @PostMapping("/edit")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<?> editTelemarketer(@RequestBody TelemarketerDto telemarketerDto) {
        return ResponseEntity.ok(telemarketerService.editTelemarketer(telemarketerDto));
    }

    @PostMapping("/load")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<?> loadTelemarketerData() {
        return ResponseEntity.ok(telemarketerService.loadTelemarketerData());
    }

    @PostMapping("/delete")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<?> deleteTelemarketer(@RequestParam Long tmId) {
        return ResponseEntity.ok(telemarketerService.deleteTelemarketer(tmId));
    }

    @PostMapping("/download")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<?> downloadData() {
        return ResponseEntity.ok(telemarketerService.downloadData());
    }
} 