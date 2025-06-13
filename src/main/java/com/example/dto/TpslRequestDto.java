package com.example.dto;

import java.math.BigDecimal;

public class TpslRequestDto {
    private String transactionId;
    private BigDecimal amount;

    public TpslRequestDto() {}

    public String getTransactionId() { return transactionId; }
    public void setTransactionId(String transactionId) { this.transactionId = transactionId; }
    public BigDecimal getAmount() { return amount; }
    public void setAmount(BigDecimal amount) { this.amount = amount; }
} 