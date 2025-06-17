package com.example.controllers;

import com.example.config.PortalApplication;
import com.example.dto.ApiResponse; // If you decide to use ApiResponse for any JSON parts
import com.example.services.BatchMgmtService;
import com.example.services.UrnService;
import com.example.interfaces.BatchMgmtDataAccess; // For nested DTO if used
import com.example.util.ErrorLogger;
import com.example.util.PortalSession;
import com.tpsl.sdk.RequestURL; // CRITICAL: Use your actual TPSL SDK package

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.format.annotation.DateTimeFormat; // For Date param if needed
import org.springframework.http.MediaType;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.servlet.ModelAndView; // For TPSL_PGRequest_T

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpSession;
import java.math.BigDecimal;
import java.text.DecimalFormat;
import java.text.DecimalFormatSymbols;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;
import java.util.Map; // For ErrorLogger

@Controller
@RequestMapping("/Payments") // Assuming this is the base path
public class TPSLController {

    private final BatchMgmtService batchMgmtService;
    private final UrnService urnService;
    // Inject ServletContext if needed for MIME types, not directly used here but common in controllers
    // private final ServletContext servletContext;


    @Autowired
    public PaymentsController(BatchMgmtService batchMgmtService, UrnService urnService /*, ServletContext servletContext*/) {
        this.batchMgmtService = batchMgmtService;
        this.urnService = urnService;
        // this.servletContext = servletContext;
    }

    private String getCurrentClassName() {
        return this.getClass().getSimpleName();
    }

    private String getCurrentMethodName() {
        return Thread.currentThread().getStackTrace()[2].getMethodName();
    }

    @PostMapping("/TPSL_PGRequest_T")
    public ModelAndView tpslPgRequestT(@RequestParam("TransactionId") String transactionId,
                                       @RequestParam("Amount") BigDecimal amount, // Spring handles BigDecimal conversion
                                       HttpServletRequest request) {
        ModelAndView mav = new ModelAndView();
        String viewName = "PGResponse"; // Default to error/response view
        String pgResponseHtml = ""; // To hold the HTML from payment gateway SDK

        try {
            String requestType = "T";
            String key = PortalApplication.getPGKey();
            String iv = PortalApplication.getPGIV();
            String merchantCode = PortalApplication.getPGMerchantCode();
            String currencyCode = PortalApplication.getPGCurrencyCode();
            String returnURL = PortalApplication.getPGReturnURL();

            DecimalFormatSymbols symbols = new DecimalFormatSymbols(Locale.US);
            DecimalFormat df = new DecimalFormat("0.00", symbols);
            String strAmount = df.format(amount);

            // Service call for BatchMgmt
            BatchMgmtDataAccess.UpdatePaymentAttemptResult updateResult = batchMgmtService.updatePaymentAttempt(
                    PortalApplication.getConnectionString(), transactionId, "TPSL");
            String nseitReferenceNumber = updateResult.getNseitReferenceNumber();
            Date paymentDate = updateResult.getPaymentDate();

            com.tpsl.sdk.RequestURL objRequestURL = new com.tpsl.sdk.RequestURL(); // Your TPSL SDK class

            pgResponseHtml = objRequestURL.sendRequest(
                    requestType, merchantCode, nseitReferenceNumber, transactionId,
                    strAmount, currencyCode, "", returnURL, "", "",
                    String.format("Test_%s_0.0", strAmount), // ITC format
                    new SimpleDateFormat("dd-MM-yyyy").format(paymentDate),
                    "", "", "", "", "", "", key, iv);

            String strResponseUpper = (pgResponseHtml != null) ? pgResponseHtml.toUpperCase() : "NULL_RESPONSE_FROM_PG";
            boolean isValidForPgSubmission = false;

            if (strResponseUpper.startsWith("ERROR")) {
                isValidForPgSubmission = false;
                if ("ERROR073".equals(strResponseUpper)) {
                    String currentDateStr = new SimpleDateFormat("dd-MM-yyyy").format(new Date());
                    pgResponseHtml = objRequestURL.sendRequest( // Retry
                            requestType, merchantCode, nseitReferenceNumber, transactionId,
                            strAmount, currencyCode, "", returnURL, "", "",
                            String.format("Test_%s_0.0", strAmount),
                            currentDateStr, // Use current date
                            "", "", "", "", "", "", key, iv);
                    strResponseUpper = (pgResponseHtml != null) ? pgResponseHtml.toUpperCase() : "NULL_RESPONSE_FROM_PG_RETRY";

                    if (strResponseUpper.startsWith("ERROR")) {
                        mav.addObject("Status", "Error Occurred After Retry: " + pgResponseHtml);
                        mav.addObject("ShowTable", false);
                        // viewName remains "PGResponse"
                    } else {
                        isValidForPgSubmission = true; // Retry was successful
                    }
                } else {
                    mav.addObject("Status", "Error Occurred: " + pgResponseHtml);
                    mav.addObject("ShowTable", false);
                    // viewName remains "PGResponse"
                }
            } else {
                isValidForPgSubmission = true;
            }

            if ("T".equals(requestType) && isValidForPgSubmission) {
                HttpSession session = request.getSession();
                session.setAttribute("Merchant_Code", merchantCode);
                session.setAttribute("IsKey", key);
                session.setAttribute("IsIv", iv);

                mav.addObject("Response", pgResponseHtml); // This is the HTML form/script
                viewName = "PG"; // View that renders the HTML to auto-submit
            }
            // If !isValidForPgSubmission, viewName should be "PGResponse" with error attributes set.

        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            mav.addObject("Status", "An unexpected error occurred processing your payment request.");
            mav.addObject("ShowTable", false);
            viewName = "PGResponse";
        }
        mav.setViewName(viewName);
        return mav;
    }

    // Private helper method from C#. Not directly an endpoint.
    // The Model parameter is for consistency if it were to set attributes, but not used in original return.
    private boolean tpslPgRequestO(String transactionId, BigDecimal amount, String nseitReferenceNumber, 
                                   Date paymentDate, Model model, HttpServletRequest request) { // Added request for logging
        boolean isAlreadyPaid = true; // Original C# default
        String requestType = "O";

        try {
            String key = PortalApplication.getPGKey();
            String iv = PortalApplication.getPGIV();
            String merchantCode = PortalApplication.getPGMerchantCode();
            String currencyCode = PortalApplication.getPGCurrencyCode();
            String returnURL = PortalApplication.getPGReturnURL();

            DecimalFormatSymbols symbols = new DecimalFormatSymbols(Locale.US);
            DecimalFormat df = new DecimalFormat("0.00", symbols);
            String strAmount = df.format(amount);

            com.tpsl.sdk.RequestURL objRequestURL = new com.tpsl.sdk.RequestURL();
            String response = "";

            // C# had: if (RequestType.ToUpper() == "T" || "S" || "O" || "R")
            // This method specifically uses "O"
            response = objRequestURL.sendRequest(
                    requestType, merchantCode, nseitReferenceNumber, transactionId,
                    strAmount, currencyCode, "", returnURL, "", "",
                    String.format("Test_%s_0.0", strAmount),
                    new SimpleDateFormat("dd-MM-yyyy").format(paymentDate),
                    "", "", "", "", "", "", key, iv);

            String strResponseRaw = response; // For DB logging
            String strResponseUpper = (response != null) ? response.toUpperCase() : "";
            String[] strSplitDecryptedResponse = strResponseUpper.split("\\|");

            String txnStatus = "", clntTxnRef = "", tpslTxnId = "", txnAmt = "";

            for (String part : strSplitDecryptedResponse) {
                String[] keyValuePair = part.split("=", 2);
                if (keyValuePair.length == 2) {
                    String K = keyValuePair[0].trim().toUpperCase();
                    String V = keyValuePair[1].trim();
                    switch (K) {
                        case "TXN_STATUS": txnStatus = V; break;
                        case "CLNT_TXN_REF": clntTxnRef = V; break;
                        case "TPSL_TXN_ID": tpslTxnId = V; break;
                        case "TXN_AMT": txnAmt = V; break;
                    }
                }
            }

            isAlreadyPaid = "0300".equals(txnStatus);
            
            // The C# original set ViewBag properties. If these are needed in a calling method's view,
            // the calling method would have to manage passing them or this helper would need to return a richer object.
            // For now, just logging them if they were for debugging.
            // System.out.println("TPSL_PGRequest_O - CLNT_TXN_REF: " + clntTxnRef + ", TPSL_TXN_ID: " + tpslTxnId + ", Amount: " + txnAmt + ", Status: " + txnStatus);
            if (model != null) { // Pass model attributes back if necessary
                 model.addAttribute("NSEITTxnsId", clntTxnRef);
                 model.addAttribute("PGTxnsId", tpslTxnId);
                 model.addAttribute("Amount", txnAmt);
                 model.addAttribute("StatusCode", txnStatus);
            }


            urnService.updatePaymentStatus(PortalApplication.getConnectionString(), transactionId, 
                                           nseitReferenceNumber, tpslTxnId, txnStatus, strResponseRaw);

        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), "tpslPgRequestO", ex, (request != null ? request.getParameterMap() : null));
            isAlreadyPaid = false; // Assume not paid or status unknown on error
        }
        return isAlreadyPaid;
    }

    @PostMapping("/TPSL_PGResponse")
    public String tpslPgResponsePost(HttpServletRequest request, Model model) {
        String viewName = "PGResponse"; // Default view
        try {
            String strPGResponse = request.getParameter("msg");

            if (strPGResponse != null && !strPGResponse.isEmpty()) {
                com.tpsl.sdk.RequestURL objRequestURL = new com.tpsl.sdk.RequestURL();
                String strDecryptedVal;

                String pgKey = PortalApplication.getPGKey(); // Get from config, not session
                String pgIV = PortalApplication.getPGIV();

                strDecryptedVal = objRequestURL.verifyPGResponse(strPGResponse, pgKey, pgIV);
                ErrorLogger.logInfo(this.getClass().getSimpleName(), "TPSL_PGResponse Decrypted", strDecryptedVal);

                if (strDecryptedVal != null && strDecryptedVal.toUpperCase().startsWith("ERROR")) {
                    model.addAttribute("ShowTable", false);
                    model.addAttribute("Status", strDecryptedVal);
                } else if (strDecryptedVal != null) {
                    String[] strSplitDecryptedResponse = strDecryptedVal.split("\\|");
                    String txnStatus = "", clntTxnRef = "", tpslTxnId = "", txnAmt = "", clientRequestMetaTransactionId = "";

                    for (String part : strSplitDecryptedResponse) {
                        String[] keyValuePair = part.split("=", 2);
                        if (keyValuePair.length == 2) {
                            String K = keyValuePair[0].trim().toUpperCase();
                            String V = keyValuePair[1].trim();
                            switch (K) {
                                case "TXN_STATUS": txnStatus = V; break;
                                case "CLNT_TXN_REF": clntTxnRef = V; break;
                                case "TPSL_TXN_ID": tpslTxnId = V; break;
                                case "TXN_AMT": txnAmt = V; break;
                                case "CLNT_RQST_META":
                                    String meta = V;
                                    if (meta.startsWith("{itc:") && meta.endsWith("}")) {
                                        clientRequestMetaTransactionId = meta.substring(5, meta.length() - 1);
                                    } else {
                                        clientRequestMetaTransactionId = meta;
                                    }
                                    break;
                            }
                        }
                    }
                    
                    String finalTransactionIdForDB = clientRequestMetaTransactionId;
                    if (finalTransactionIdForDB == null || finalTransactionIdForDB.isEmpty()) {
                         ErrorLogger.logError(getCurrentClassName(), "TPSL_PGResponse", 
                            new Exception("CLNT_RQST_META (TransactionId) missing/invalid in PG response: " + strDecryptedVal), 
                            request.getParameterMap());
                        model.addAttribute("Status", "Critical Error: Original transaction ID not found in response.");
                        model.addAttribute("ShowTable", false);
                        return viewName;
                    }

                    urnService.updatePaymentStatus(PortalApplication.getConnectionString(), 
                                                   finalTransactionIdForDB, clntTxnRef, tpslTxnId, 
                                                   txnStatus, strDecryptedVal);

                    model.addAttribute("NSEITTxnsId", clntTxnRef);
                    model.addAttribute("PGTxnsId", tpslTxnId);
                    model.addAttribute("Amount", txnAmt);
                    model.addAttribute("StatusCode", txnStatus);
                    if ("0300".equals(txnStatus)) {
                        model.addAttribute("Status", "Payment Successful.");
                        model.addAttribute("ShowTable", true);
                    } else {
                        model.addAttribute("Status", "Transaction Failed. Status: " + txnStatus);
                        model.addAttribute("ShowTable", false);
                    }
                } else {
                     model.addAttribute("ShowTable", false);
                     model.addAttribute("Status", "Unable to decrypt or verify payment gateway response.");
                }
            } else {
                model.addAttribute("ShowTable", false);
                model.addAttribute("Status", "No response received from payment gateway.");
            }
        } catch (Exception ex) {
            ErrorLogger.logError(getCurrentClassName(), getCurrentMethodName(), ex, request.getParameterMap());
            model.addAttribute("ShowTable", false);
            model.addAttribute("Status", "An error occurred while processing the payment response.");
        }
        return viewName;
    }

    @GetMapping("/PG2")
    public String pg2View(Model model, HttpServletRequest request) {
        model.addAttribute("IsLoggedOn", PortalSession.getUserID(request) != 0);
        model.addAttribute("ClassName", "col-sm-9");
        return "PG2"; // View name
    }

    // --- Include Paytm methods if this controller is intended to handle both ---
    // If PaytmPGRequest, PaytmPGResponse, PaytmPaymentStatusCheck are part of the same controller,
    // their converted Java versions (from previous responses) would go here.
    // For clarity, I'm keeping this focused on the TPSL methods you just provided.
}