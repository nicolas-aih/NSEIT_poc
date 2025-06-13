package com.example.service;

import java.time.LocalDate;

public interface ValidationService {
    boolean validateDateOfBirth(LocalDate dob);
    boolean validateYearOfPassing(LocalDate yop);
    String validatePAN(String pan, Long applicantId);
    String validateEmailForCorporates(String email, Long applicantId, String pan);
    String validateMobileForCorporates(String mobile, Long applicantId, String pan);
    String validateMobileForCorporatesApp(String mobile, Long applicantDataId, String pan);
} 