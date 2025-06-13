package com.example.controller;

import com.example.model.PaymentRequest;
import com.example.model.PaymentResponse;
import com.example.service.PaymentService;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;

import java.math.BigDecimal;
import java.util.Map;

@RestController
@RequestMapping("/api/payments")
public class PaymentController {

    private final PaymentService paymentService;

    public PaymentController(PaymentService paymentService) {
        this.paymentService = paymentService;
    }

    @GetMapping("/exam-fees")
    public ResponseEntity<Map<String, Object>> examFeesPayment() {
        return ResponseEntity.ok(Map.of(
            "response", "",
            "className", "col-sm-6"
        ));
    }

    @PostMapping("/process")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<PaymentResponse> processPayment(
            @RequestParam String transactionId,
            @RequestParam BigDecimal amount,
            @RequestParam String paymentGateway) {
        
        // Check if payment was already made
        if (paymentService.checkPreviousTransactionRequest(transactionId, amount)) {
            PaymentResponse response = new PaymentResponse();
            response.setStatus("Payment is already done");
            response.setShowTable(true);
            response.setSuccess(true);
            return ResponseEntity.ok(response);
        }

        // Create payment request
        PaymentRequest request = new PaymentRequest();
        request.setTransactionId(transactionId);
        request.setAmount(amount);
        request.setPaymentGateway(paymentGateway);

        // Process payment based on gateway
        PaymentResponse response;
        if ("TPSL".equals(paymentGateway)) {
            response = paymentService.processTPSLPayment(request);
        } else if ("PAYTM".equals(paymentGateway)) {
            response = paymentService.processPaytmPayment(request);
        } else {
            throw new IllegalArgumentException("Invalid payment gateway: " + paymentGateway);
        }

        return ResponseEntity.ok(response);
    }

    @PostMapping("/tpsl/response")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<PaymentResponse> tpslResponse(@RequestParam String msg) {
        // This would typically involve:
        // 1. Decrypting the response using TPSL key/IV
        // 2. Parsing the response
        // 3. Updating the payment status
        // For now, we'll just return a simulated response
        
        PaymentResponse response = new PaymentResponse();
        response.setStatus("Payment Successful");
        response.setShowTable(true);
        response.setSuccess(true);
        return ResponseEntity.ok(response);
    }

    @PostMapping("/paytm/response")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<PaymentResponse> paytmResponse(@RequestBody Map<String, String> responseParams) {
        // This would typically involve:
        // 1. Validating the Paytm checksum
        // 2. Processing the response
        // 3. Updating the payment status
        // For now, we'll just return a simulated response
        
        PaymentResponse response = new PaymentResponse();
        response.setStatus("Payment Successful");
        response.setShowTable(true);
        response.setSuccess(true);
        return ResponseEntity.ok(response);
    }

    @GetMapping("/pg2")
    @PreAuthorize("hasRole('USER')")
    public ResponseEntity<Map<String, Object>> pg2() {
        return ResponseEntity.ok(Map.of(
            "isLoggedOn", true,
            "className", "col-sm-9"
        ));
    }
} 