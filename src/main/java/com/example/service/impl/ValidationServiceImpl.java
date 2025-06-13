package com.example.service.impl;

import com.example.service.ValidationService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDate;
import java.time.Period;

@Service
@Transactional
public class ValidationServiceImpl implements ValidationService {

    private final JdbcTemplate jdbcTemplate;

    @Autowired
    public ValidationServiceImpl(JdbcTemplate jdbcTemplate) {
        this.jdbcTemplate = jdbcTemplate;
    }

    @Override
    public boolean validateDateOfBirth(LocalDate dob) {
        if (dob == null) {
            return false;
        }
        
        LocalDate now = LocalDate.now();
        Period age = Period.between(dob, now);
        return age.getYears() >= 18;
    }

    @Override
    public boolean validateYearOfPassing(LocalDate yop) {
        if (yop == null) {
            return false;
        }
        return !yop.isAfter(LocalDate.now());
    }

    @Override
    public String validatePAN(String pan, Long applicantId) {
        if (pan == null || pan.trim().isEmpty()) {
            return "PAN number is required";
        }

        if (!pan.matches("[A-Z]{5}[0-9]{4}[A-Z]{1}")) {
            return "Invalid PAN format";
        }

        // Check if PAN already exists for another applicant
        String sql = "SELECT COUNT(*) FROM applicants WHERE pan = ? AND applicant_id != ?";
        int count = jdbcTemplate.queryForObject(sql, Integer.class, pan, applicantId);
        
        return count > 0 ? "PAN already exists" : "";
    }

    @Override
    public String validateEmailForCorporates(String email, Long applicantId, String pan) {
        if (email == null || email.trim().isEmpty()) {
            return "Email is required";
        }

        if (!email.matches("^[A-Za-z0-9+_.-]+@(.+)$")) {
            return "Invalid email format";
        }

        // Check if email already exists for another corporate
        String sql = "SELECT COUNT(*) FROM corporate_applicants WHERE email = ? AND applicant_id != ? AND pan != ?";
        int count = jdbcTemplate.queryForObject(sql, Integer.class, email, applicantId, pan);
        
        return count > 0 ? "Email already exists" : "";
    }

    @Override
    public String validateMobileForCorporates(String mobile, Long applicantId, String pan) {
        if (mobile == null || mobile.trim().isEmpty()) {
            return "Mobile number is required";
        }

        if (!mobile.matches("^[0-9]{10}$")) {
            return "Invalid mobile number format";
        }

        // Check if mobile already exists for another corporate
        String sql = "SELECT COUNT(*) FROM corporate_applicants WHERE mobile = ? AND applicant_id != ? AND pan != ?";
        int count = jdbcTemplate.queryForObject(sql, Integer.class, mobile, applicantId, pan);
        
        return count > 0 ? "Mobile number already exists" : "";
    }

    @Override
    public String validateMobileForCorporatesApp(String mobile, Long applicantDataId, String pan) {
        if (mobile == null || mobile.trim().isEmpty()) {
            return "Mobile number is required";
        }

        if (!mobile.matches("^[0-9]{10}$")) {
            return "Invalid mobile number format";
        }

        // Check if mobile already exists for another corporate application
        String sql = "SELECT COUNT(*) FROM corporate_applications WHERE mobile = ? AND applicant_data_id != ? AND pan != ?";
        int count = jdbcTemplate.queryForObject(sql, Integer.class, mobile, applicantDataId, pan);
        
        return count > 0 ? "Mobile number already exists" : "";
    }
} 