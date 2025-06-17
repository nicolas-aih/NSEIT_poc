package com.example.service.impl;

import com.example.config.PortalSession;
import com.example.dto.ChangePasswordDto;
import com.example.dto.LoginRequestDto;
import com.example.dto.ResetPasswordDto;
import com.example.model.User;
import com.example.repository.UserRepository;
import com.example.service.UserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDateTime;
import java.util.HashMap;
import java.util.Map;
import java.util.Optional;

@Service
public class UserServiceImpl implements UserService {

    private final UserRepository userRepository;
    private final PasswordEncoder passwordEncoder;
    private final PortalSession portalSession;

    @Autowired
    public UserServiceImpl(UserRepository userRepository, 
                          PasswordEncoder passwordEncoder,
                          PortalSession portalSession) {
        this.userRepository = userRepository;
        this.passwordEncoder = passwordEncoder;
        this.portalSession = portalSession;
    }

    @Override
    @Transactional
    public Map<String, Object> login(LoginRequestDto loginRequest) {
        Map<String, Object> response = new HashMap<>();
        
        Optional<User> userOpt = userRepository.findByEmail(loginRequest.getEmail());
        if (userOpt.isEmpty()) {
            response.put("success", false);
            response.put("message", "Invalid credentials");
            return response;
        }

        User user = userOpt.get();
        if (!passwordEncoder.matches(loginRequest.getPassword(), user.getPassword())) {
            response.put("success", false);
            response.put("message", "Invalid credentials");
            return response;
        }

        if (!"ACTIVE".equals(user.getStatus())) {
            response.put("success", false);
            response.put("message", "Account is not active");
            return response;
        }

        // Update last login
        user.setLastLoginDate(LocalDateTime.now());
        userRepository.save(user);

        // Set session
        portalSession.setUserId(user.getId());
        portalSession.setUserRole(user.getRole());

        response.put("success", true);
        response.put("user", user);
        return response;
    }

    @Override
    public void logout() {
        portalSession.clear();
    }

    @Override
    @Transactional
    public void changePassword(ChangePasswordDto changePasswordDto) {
        Long userId = portalSession.getUserId();
        User user = userRepository.findById(userId)
            .orElseThrow(() -> new RuntimeException("User not found"));

        if (!passwordEncoder.matches(changePasswordDto.getCurrentPassword(), user.getPassword())) {
            throw new RuntimeException("Current password is incorrect");
        }

        if (!changePasswordDto.getNewPassword().equals(changePasswordDto.getConfirmPassword())) {
            throw new RuntimeException("New passwords do not match");
        }

        user.setPassword(passwordEncoder.encode(changePasswordDto.getNewPassword()));
        user.setPasswordChangedDate(LocalDateTime.now());
        userRepository.save(user);
    }

    @Override
    @Transactional
    public void resetPassword(ResetPasswordDto resetPasswordDto) {
        User user = userRepository.findByEmail(resetPasswordDto.getEmail())
            .orElseThrow(() -> new RuntimeException("User not found"));

        // Validate reset token
        if (!isValidResetToken(user, resetPasswordDto.getToken())) {
            throw new RuntimeException("Invalid or expired reset token");
        }

        if (!resetPasswordDto.getNewPassword().equals(resetPasswordDto.getConfirmPassword())) {
            throw new RuntimeException("New passwords do not match");
        }

        user.setPassword(passwordEncoder.encode(resetPasswordDto.getNewPassword()));
        user.setPasswordChangedDate(LocalDateTime.now());
        user.setResetToken(null);
        user.setResetTokenExpiry(null);
        userRepository.save(user);
    }

    @Override
    public Map<String, Object> myProfile() {
        Long userId = portalSession.getUserId();
        User user = userRepository.findById(userId)
            .orElseThrow(() -> new RuntimeException("User not found"));

        Map<String, Object> profile = new HashMap<>();
        profile.put("id", user.getId());
        profile.put("email", user.getEmail());
        profile.put("name", user.getName());
        profile.put("role", user.getRole());
        profile.put("status", user.getStatus());
        profile.put("lastLoginDate", user.getLastLoginDate());
        return profile;
    }

    @Override
    public Map<String, Object> getProfile() {
        return myProfile();
    }

    @Override
    @Transactional
    public void saveProfile(Map<String, Object> profileData) {
        Long userId = portalSession.getUserId();
        User user = userRepository.findById(userId)
            .orElseThrow(() -> new RuntimeException("User not found"));

        user.setName((String) profileData.get("name"));
        user.setUpdatedAt(LocalDateTime.now());
        user.setUpdatedBy(userId.toString());
        userRepository.save(user);
    }

    @Override
    @Transactional
    public void updateProfile2(Map<String, Object> profileData) {
        saveProfile(profileData);
    }

    @Override
    public Map<String, Object> getUserById(Long userId) {
        User user = userRepository.findById(userId)
            .orElseThrow(() -> new RuntimeException("User not found"));

        Map<String, Object> userData = new HashMap<>();
        userData.put("id", user.getId());
        userData.put("email", user.getEmail());
        userData.put("name", user.getName());
        userData.put("role", user.getRole());
        userData.put("status", user.getStatus());
        return userData;
    }

    @Override
    public Map<String, Object> getUserByEmail(String email) {
        User user = userRepository.findByEmail(email)
            .orElseThrow(() -> new RuntimeException("User not found"));

        Map<String, Object> userData = new HashMap<>();
        userData.put("id", user.getId());
        userData.put("email", user.getEmail());
        userData.put("name", user.getName());
        userData.put("role", user.getRole());
        userData.put("status", user.getStatus());
        return userData;
    }

    @Override
    @Transactional
    public void updateUserStatus(Long userId, String status) {
        User user = userRepository.findById(userId)
            .orElseThrow(() -> new RuntimeException("User not found"));

        user.setStatus(status);
        user.setUpdatedAt(LocalDateTime.now());
        user.setUpdatedBy(portalSession.getUserId().toString());
        userRepository.save(user);
    }

    @Override
    @Transactional
    public void updateUserRole(Long userId, String role) {
        User user = userRepository.findById(userId)
            .orElseThrow(() -> new RuntimeException("User not found"));

        user.setRole(role);
        user.setUpdatedAt(LocalDateTime.now());
        user.setUpdatedBy(portalSession.getUserId().toString());
        userRepository.save(user);
    }

    @Override
    public boolean validateSession(String sessionId) {
        return portalSession.isValid();
    }

    @Override
    public void invalidateSession(String sessionId) {
        portalSession.clear();
    }

    @Override
    public boolean hasPermission(String permission) {
        // Implement permission checking logic
        return true; // Placeholder
    }

    @Override
    public boolean hasRole(String role) {
        return role.equals(portalSession.getUserRole());
    }

    private boolean isValidResetToken(User user, String token) {
        if (user.getResetToken() == null || user.getResetTokenExpiry() == null) {
            return false;
        }
        return user.getResetToken().equals(token) && 
               user.getResetTokenExpiry().isAfter(LocalDateTime.now());
    }
} 