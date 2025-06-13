package com.example.service;

import com.example.model.PaymentRequest;
import com.example.model.PaymentResponse;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.Map;

@Service
public class PaymentService {
    
    private final JdbcTemplate jdbcTemplate;

    @Value("${portal.connection-string}")
    private String connectionString;

    @Value("${payment.tpsl.key}")
    private String tpslKey;

    @Value("${payment.tpsl.iv}")
    private String tpslIv;

    @Value("${payment.tpsl.merchant-code}")
    private String tpslMerchantCode;

    @Value("${payment.tpsl.currency-code}")
    private String tpslCurrencyCode;

    @Value("${payment.tpsl.return-url}")
    private String tpslReturnUrl;

    @Value("${payment.paytm.merchant-id}")
    private String paytmMerchantId;

    @Value("${payment.paytm.merchant-key}")
    private String paytmMerchantKey;

    @Value("${payment.paytm.website}")
    private String paytmWebsite;

    @Value("${payment.paytm.industry-type}")
    private String paytmIndustryType;

    public PaymentService(JdbcTemplate jdbcTemplate) {
        this.jdbcTemplate = jdbcTemplate;
    }

    public boolean checkPreviousTransactionRequest(String transactionId, BigDecimal amount) {
        String sql = "EXEC sp_GetPreviousPaymentAttemptDetails @TransactionId = ?";
        Map<String, Object> result = jdbcTemplate.queryForMap(sql, transactionId);
        
        String nseitRefNumber = (String) result.get("NSEITReferenceNumber");
        if (nseitRefNumber == null || nseitRefNumber.trim().isEmpty()) {
            return false;
        }

        String paymentGateway = (String) result.get("PaymentGateway");
        LocalDateTime paymentDate = (LocalDateTime) result.get("PaymentDate");

        if ("TPSL".equals(paymentGateway)) {
            return checkTPSLPaymentStatus(transactionId, amount, nseitRefNumber, paymentDate);
        } else if ("PAYTM".equals(paymentGateway)) {
            return checkPaytmPaymentStatus(transactionId, amount, nseitRefNumber, paymentDate);
        }

        return false;
    }

    @Transactional
    public PaymentResponse processTPSLPayment(PaymentRequest request) {
        // Update payment attempt
        String sql = "EXEC sp_UpdatePaymentAttempt @TransactionId = ?, @PaymentGateway = ?";
        Map<String, Object> result = jdbcTemplate.queryForMap(sql, 
            request.getTransactionId(), "TPSL");

        String nseitRefNumber = (String) result.get("NSEITReferenceNumber");
        
        // Call TPSL gateway and process payment
        // This would typically involve calling the TPSL API
        // For now, we'll just return a simulated response
        
        PaymentResponse response = new PaymentResponse();
        response.setNseitTxnId(nseitRefNumber);
        response.setPgTxnId("TPSL_" + System.currentTimeMillis());
        response.setAmount(request.getAmount());
        response.setStatusCode("0300");
        response.setStatus("Payment Successful");
        response.setPaymentGateway("TPSL");
        response.setPaymentDate(LocalDateTime.now());
        response.setSuccess(true);
        response.setShowTable(true);

        // Update payment status in database
        updatePaymentStatus(request.getTransactionId(), nseitRefNumber, 
            response.getPgTxnId(), response.getStatusCode(), "TPSL Response");

        return response;
    }

    @Transactional
    public PaymentResponse processPaytmPayment(PaymentRequest request) {
        // Update payment attempt
        String sql = "EXEC sp_UpdatePaymentAttempt @TransactionId = ?, @PaymentGateway = ?";
        Map<String, Object> result = jdbcTemplate.queryForMap(sql, 
            request.getTransactionId(), "PAYTM");

        String nseitRefNumber = (String) result.get("NSEITReferenceNumber");
        
        // Call Paytm gateway and process payment
        // This would typically involve calling the Paytm API
        // For now, we'll just return a simulated response
        
        PaymentResponse response = new PaymentResponse();
        response.setNseitTxnId(nseitRefNumber);
        response.setPgTxnId("PAYTM_" + System.currentTimeMillis());
        response.setAmount(request.getAmount());
        response.setStatusCode("TXN_SUCCESS");
        response.setStatus("Payment Successful");
        response.setPaymentGateway("PAYTM");
        response.setPaymentDate(LocalDateTime.now());
        response.setSuccess(true);
        response.setShowTable(true);

        // Update payment status in database
        updatePaymentStatus(request.getTransactionId(), nseitRefNumber, 
            response.getPgTxnId(), response.getStatusCode(), "PAYTM Response");

        return response;
    }

    private boolean checkTPSLPaymentStatus(String transactionId, BigDecimal amount, 
            String nseitRefNumber, LocalDateTime paymentDate) {
        // Call TPSL API to check payment status
        // This would typically involve calling the TPSL API
        // For now, we'll just return a simulated response
        return true;
    }

    private boolean checkPaytmPaymentStatus(String transactionId, BigDecimal amount, 
            String nseitRefNumber, LocalDateTime paymentDate) {
        // Call Paytm API to check payment status
        // This would typically involve calling the Paytm API
        // For now, we'll just return a simulated response
        return true;
    }

    private void updatePaymentStatus(String transactionId, String nseitRefNumber, 
            String pgTxnId, String status, String response) {
        String sql = "EXEC sp_UpdatePaymentStatus @TransactionId = ?, @NSEITReferenceNumber = ?, " +
                    "@PGTransactionId = ?, @Status = ?, @Response = ?";
        jdbcTemplate.update(sql, transactionId, nseitRefNumber, pgTxnId, status, response);
    }
} 