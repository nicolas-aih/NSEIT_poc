package com.example.controller;

import com.example.dto.TpslRequestDto;
import com.example.service.TpslService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/tpsl")
public class TpslController {

    @Autowired
    private TpslService tpslService;

    @PostMapping("/pg-request-t")
    public ResponseEntity<?> pgRequestT(@RequestBody TpslRequestDto requestDto) {
        return ResponseEntity.ok(tpslService.pgRequestT(requestDto));
    }

    @PostMapping("/pg-response")
    public ResponseEntity<?> pgResponse() {
        return ResponseEntity.ok(tpslService.pgResponse());
    }

    @GetMapping("/pg2")
    public ResponseEntity<?> pg2() {
        return ResponseEntity.ok("PG2 View Stub");
    }
} 