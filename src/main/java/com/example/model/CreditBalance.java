package com.example.model;

import jakarta.persistence.*;
import lombok.Data;
import java.math.BigDecimal;
import java.time.LocalDateTime;

@Data
@Entity
@Table(name = "credit_balances")
public class CreditBalance {
    
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    
    @Column(name = "user_id", nullable = false)
    private Long userId;
    
    @Column(name = "balance", nullable = false, precision = 10, scale = 2)
    private BigDecimal balance;
    
    @Column(name = "last_transaction_date")
    private LocalDateTime lastTransactionDate;
    
    @Column(name = "last_transaction_type")
    private String lastTransactionType;
    
    @Column(name = "last_transaction_amount", precision = 10, scale = 2)
    private BigDecimal lastTransactionAmount;
    
    @Column(name = "status", nullable = false)
    private String status;
    
    @Column(name = "created_at")
    private LocalDateTime createdAt;
    
    @Column(name = "updated_at")
    private LocalDateTime updatedAt;
    
    @Column(name = "created_by")
    private String createdBy;
    
    @Column(name = "updated_by")
    private String updatedBy;
    
    @PrePersist
    protected void onCreate() {
        createdAt = LocalDateTime.now();
        updatedAt = LocalDateTime.now();
        if (status == null) {
            status = "ACTIVE";
        }
    }
    
    @PreUpdate
    protected void onUpdate() {
        updatedAt = LocalDateTime.now();
    }
} 