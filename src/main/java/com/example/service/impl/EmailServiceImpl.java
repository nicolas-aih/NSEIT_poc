package com.example.service.impl;

import com.example.service.EmailService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.mail.SimpleMailMessage;
import org.springframework.mail.javamail.JavaMailSender;
import org.springframework.stereotype.Service;

@Service
public class EmailServiceImpl implements EmailService {

    private static final Logger logger = LoggerFactory.getLogger(EmailServiceImpl.class);
    private final JavaMailSender mailSender;

    @Autowired
    public EmailServiceImpl(JavaMailSender mailSender) {
        this.mailSender = mailSender;
    }

    private void sendEmail(SimpleMailMessage message) {
        try {
            if (mailSender != null) {
                mailSender.send(message);
                logger.info("Email sent successfully to: {}", message.getTo());
            } else {
                logger.warn("Email not sent - mail sender not configured");
            }
        } catch (Exception e) {
            logger.error("Failed to send email: {}", e.getMessage());
        }
    }

    @Override
    public void sendRegistrationConfirmation(String to, String registrationNumber, String examName) {
        SimpleMailMessage message = new SimpleMailMessage();
        message.setTo(to);
        message.setSubject("Registration Confirmation - " + examName);
        message.setText(String.format(
            "Dear Candidate,\n\n" +
            "Your registration for %s has been received successfully.\n" +
            "Registration Number: %s\n\n" +
            "Please complete the payment and upload required documents to complete your registration.\n\n" +
            "Best regards,\n" +
            "Exam Registration Team",
            examName, registrationNumber
        ));
        sendEmail(message);
    }

    @Override
    public void sendPaymentConfirmation(String to, String registrationNumber, String amount) {
        SimpleMailMessage message = new SimpleMailMessage();
        message.setTo(to);
        message.setSubject("Payment Confirmation");
        message.setText(String.format(
            "Dear Candidate,\n\n" +
            "We have received your payment of %s for registration number %s.\n" +
            "Your registration is now being processed.\n\n" +
            "Best regards,\n" +
            "Exam Registration Team",
            amount, registrationNumber
        ));
        sendEmail(message);
    }

    @Override
    public void sendDocumentVerificationStatus(String to, String registrationNumber, String status, String remarks) {
        SimpleMailMessage message = new SimpleMailMessage();
        message.setTo(to);
        message.setSubject("Document Verification Status");
        message.setText(String.format(
            "Dear Candidate,\n\n" +
            "Your documents for registration number %s have been %s.\n" +
            "Remarks: %s\n\n" +
            "Best regards,\n" +
            "Exam Registration Team",
            registrationNumber, status, remarks
        ));
        sendEmail(message);
    }

    @Override
    public void sendHallTicket(String to, String registrationNumber, String examName) {
        SimpleMailMessage message = new SimpleMailMessage();
        message.setTo(to);
        message.setSubject("Hall Ticket - " + examName);
        message.setText(String.format(
            "Dear Candidate,\n\n" +
            "Your hall ticket for %s (Registration Number: %s) is now available.\n" +
            "Please download it from your dashboard.\n\n" +
            "Best regards,\n" +
            "Exam Registration Team",
            examName, registrationNumber
        ));
        sendEmail(message);
    }

    @Override
    public void sendRegistrationCancellation(String to, String registrationNumber, String examName) {
        SimpleMailMessage message = new SimpleMailMessage();
        message.setTo(to);
        message.setSubject("Registration Cancellation - " + examName);
        message.setText(String.format(
            "Dear Candidate,\n\n" +
            "Your registration for %s (Registration Number: %s) has been cancelled.\n" +
            "If you have already made the payment, the refund process will be initiated.\n\n" +
            "Best regards,\n" +
            "Exam Registration Team",
            examName, registrationNumber
        ));
        sendEmail(message);
    }

    @Override
    public void sendRefundConfirmation(String to, String registrationNumber, String amount) {
        SimpleMailMessage message = new SimpleMailMessage();
        message.setTo(to);
        message.setSubject("Refund Confirmation");
        message.setText(String.format(
            "Dear Candidate,\n\n" +
            "We have processed your refund of %s for registration number %s.\n" +
            "The amount will be credited to your account within 5-7 business days.\n\n" +
            "Best regards,\n" +
            "Exam Registration Team",
            amount, registrationNumber
        ));
        sendEmail(message);
    }

    @Override
    public void sendExamReminder(String to, String registrationNumber, String examName, String examDate) {
        SimpleMailMessage message = new SimpleMailMessage();
        message.setTo(to);
        message.setSubject("Exam Reminder - " + examName);
        message.setText(String.format(
            "Dear Candidate,\n\n" +
            "This is a reminder that your exam %s (Registration Number: %s) is scheduled for %s.\n" +
            "Please ensure you have downloaded your hall ticket and bring it along with a valid ID proof.\n\n" +
            "Best regards,\n" +
            "Exam Registration Team",
            examName, registrationNumber, examDate
        ));
        sendEmail(message);
    }
} 