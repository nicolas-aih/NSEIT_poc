package com.example.model;

import lombok.Data;
import java.math.BigDecimal;

@Data
public class PaymentRequest {
    private String transactionId;
    private BigDecimal amount;
    private String paymentGateway; // TPSL or PAYTM
    private String batchNo;
    private String merchantCode;
    private String currencyCode;
    private String returnUrl;
} 