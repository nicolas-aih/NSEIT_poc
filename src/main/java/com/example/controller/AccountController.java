package com.example.controller;

import com.example.model.CreditBalance;
import com.example.model.ApiResponse;
import com.example.service.AccountService;
import com.example.config.PortalSession;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.math.BigDecimal;
import java.text.NumberFormat;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.*;

@Controller
@RequestMapping("/accounts")
public class AccountController {

    @Autowired
    private AccountService accountService;

    @Autowired
    private PortalSession portalSession;

    private static final String[] CREDIT_BALANCE_ROLES = {"BR", "CA", "WA", "IMF", "I"};

    @GetMapping("/credit-balance")
    @PreAuthorize("isAuthenticated()")
    public String getCreditBalance(Model model) {
        model.addAttribute("isLoggedOn", portalSession.getUserId() != 0);
        model.addAttribute("className", "col-sm-9");

        if (Arrays.asList(CREDIT_BALANCE_ROLES).contains(portalSession.getRoleCode())) {
            try {
                NumberFormat nf = NumberFormat.getNumberInstance();
                nf.setMinimumFractionDigits(2);
                nf.setMaximumFractionDigits(2);
                nf.setGroupingUsed(true);

                String output;
                if ("CSS".equals(portalSession.getIntegrationMode())) {
                    BigDecimal amount = accountService.getCurrentBalance(portalSession.getInsurerUserId());
                    output = formatBalanceMessage(amount, nf);
                } else {
                    if (accountService.validateCreditMode(portalSession.getRoleCode(), portalSession.getInsurerUserId())) {
                        BigDecimal amount = accountService.getCurrentBalanceOAIMS(portalSession.getTopUserLoginId());
                        output = formatBalanceMessage(amount, nf);
                    } else {
                        output = "You have not opted for Credit Balance facility.";
                    }
                }
                model.addAttribute("output", output);
            } catch (Exception e) {
                model.addAttribute("output", "An error occurred while processing your request.");
            }
        }
        return "credit-balance";
    }

    @PostMapping("/credit-balance")
    @PreAuthorize("isAuthenticated()")
    @ResponseBody
    public ResponseEntity<ApiResponse> getCreditBalance2(
            @RequestParam String companyType,
            @RequestParam Integer companyCode) {
        try {
            NumberFormat nf = NumberFormat.getNumberInstance();
            nf.setMinimumFractionDigits(2);
            nf.setMaximumFractionDigits(2);
            nf.setGroupingUsed(true);

            String output;
            if ("CSS".equals(portalSession.getIntegrationMode())) {
                BigDecimal amount = accountService.getCurrentBalance(companyCode);
                output = formatBalanceMessage(amount, nf);
            } else {
                String topLevelCompanyCode = accountService.validateCreditModeAndGetTopLevel(companyType, companyCode);
                if (topLevelCompanyCode != null) {
                    BigDecimal amount = accountService.getCurrentBalanceOAIMS(topLevelCompanyCode);
                    output = formatBalanceMessage(amount, nf);
                } else {
                    output = "You have not opted for Credit Balance facility.";
                }
            }
            return ResponseEntity.ok(new ApiResponse(true, output));
        } catch (Exception e) {
            return ResponseEntity.ok(new ApiResponse(false, "An error occurred while processing your request."));
        }
    }

    @GetMapping("/new-entry")
    @PreAuthorize("isAuthenticated()")
    public String newEntry(Model model) {
        model.addAttribute("isLoggedOn", portalSession.getUserId() != 0);
        model.addAttribute("className", "col-sm-9");
        return "new-entry";
    }

    @PostMapping("/voucher")
    @PreAuthorize("isAuthenticated()")
    @ResponseBody
    public ResponseEntity<ApiResponse> addVoucher(
            @RequestParam String instructionType,
            @RequestParam String referenceNo,
            @RequestParam @DateTimeFormat(pattern = "yyyy-MM-dd") LocalDateTime dateOfPayment,
            @RequestParam BigDecimal amount,
            @RequestParam String modeOfPayment,
            @RequestParam(required = false) Integer companyCode,
            @RequestParam(required = false) String remarks,
            @RequestParam(required = false) String narration) {
        
        try {
            // Validate instruction type
            if (!("C".equalsIgnoreCase(instructionType) || "D".equalsIgnoreCase(instructionType))) {
                return ResponseEntity.ok(new ApiResponse(false, "Invalid instruction type"));
            }

            // Validate mode of payment
            List<String> validModes = Arrays.asList("NEFT", "RTGS", "IMPS", "CHEQUE", "DD");
            if (!validModes.contains(modeOfPayment.toUpperCase())) {
                return ResponseEntity.ok(new ApiResponse(false, "Invalid mode of payment"));
            }

            // Handle company code based on role
            if ("BO".equals(portalSession.getRoleCode())) {
                if (companyCode == null) {
                    return ResponseEntity.ok(new ApiResponse(false, "Company code is required"));
                }
            } else if (Arrays.asList("I", "CA", "WA", "IMF", "BR").contains(portalSession.getRoleCode())) {
                companyCode = portalSession.getInsurerUserId();
            }

            String message = accountService.saveCreditBalanceEntry(
                companyCode,
                instructionType.toUpperCase(),
                referenceNo,
                amount,
                modeOfPayment.toUpperCase(),
                dateOfPayment,
                remarks,
                portalSession.getUserId(),
                narration
            );

            if (message.isEmpty()) {
                return ResponseEntity.ok(new ApiResponse(true, "Data saved successfully"));
            } else {
                return ResponseEntity.ok(new ApiResponse(false, message));
            }
        } catch (Exception e) {
            return ResponseEntity.ok(new ApiResponse(false, "An error occurred while processing your request."));
        }
    }

    @GetMapping("/approval")
    @PreAuthorize("isAuthenticated()")
    public String approvalRejection(Model model) {
        model.addAttribute("isLoggedOn", portalSession.getUserId() != 0);
        model.addAttribute("className", "col-sm-9");
        return "approval-rejection";
    }

    @GetMapping("/ledger")
    @PreAuthorize("isAuthenticated()")
    public String getLedgerReport(Model model) {
        model.addAttribute("isLoggedOn", portalSession.getUserId() != 0);
        model.addAttribute("className", "col-sm-9");
        return "ledger-report";
    }

    private String formatBalanceMessage(BigDecimal amount, NumberFormat nf) {
        return String.format("Credit Balance as on %s is Rupees %s",
            LocalDateTime.now().format(DateTimeFormatter.ofPattern("dd-MMM-yyyy hh:mm:ss a")),
            nf.format(amount));
    }
} 