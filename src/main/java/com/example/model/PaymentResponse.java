package com.example.model;

import lombok.Data;
import java.math.BigDecimal;
import java.time.LocalDateTime;

@Data
public class PaymentResponse {
    private String nseitTxnId;
    private String pgTxnId;
    private BigDecimal amount;
    private String statusCode;
    private String status;
    private String paymentGateway;
    private LocalDateTime paymentDate;
    private String message;
    private boolean success;
    private boolean showTable;
} 