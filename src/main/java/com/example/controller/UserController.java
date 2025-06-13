package com.example.controller;

import com.example.dto.LoginRequestDto;
import com.example.dto.ChangePasswordDto;
import com.example.dto.ResetPasswordDto;
import com.example.service.UserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.Map;

@RestController
@RequestMapping("/api/user")
public class UserController {

    @Autowired
    private UserService userService;

    @PostMapping("/login")
    public ResponseEntity<?> login(@RequestBody LoginRequestDto loginRequest) {
        return ResponseEntity.ok(userService.login(loginRequest));
    }

    @GetMapping("/logout")
    public ResponseEntity<?> logout() {
        return ResponseEntity.ok(userService.logout());
    }

    @PostMapping("/change-password")
    public ResponseEntity<?> changePassword(@RequestBody ChangePasswordDto changePasswordDto) {
        return ResponseEntity.ok(userService.changePassword(changePasswordDto));
    }

    @PostMapping("/reset-password")
    public ResponseEntity<?> resetPassword(@RequestBody ResetPasswordDto resetPasswordDto) {
        return ResponseEntity.ok(userService.resetPassword(resetPasswordDto));
    }

    @GetMapping("/my-profile")
    public ResponseEntity<?> myProfile() {
        return ResponseEntity.ok(userService.myProfile());
    }

    @PostMapping("/get-profile")
    public ResponseEntity<?> getProfile() {
        return ResponseEntity.ok(userService.getProfile());
    }

    @PostMapping("/save-profile")
    public ResponseEntity<?> saveProfile(@RequestBody Map<String, Object> profileData) {
        return ResponseEntity.ok(userService.saveProfile(profileData));
    }

    @PostMapping("/update-profile2")
    public ResponseEntity<?> updateProfile2(@RequestBody Map<String, Object> profileData) {
        return ResponseEntity.ok(userService.updateProfile2(profileData));
    }

    @GetMapping("/view-dp")
    public ResponseEntity<?> viewDP() {
        return ResponseEntity.ok(userService.viewDP());
    }

    @PostMapping("/save-dp")
    public ResponseEntity<?> saveDP(@RequestBody Map<String, Object> dpData) {
        return ResponseEntity.ok(userService.saveDP(dpData));
    }

    @PostMapping("/generate-new-dp-id")
    public ResponseEntity<?> generateNewDPId(@RequestBody Map<String, Object> requestData) {
        return ResponseEntity.ok(userService.generateNewDPId(requestData));
    }

    @GetMapping("/new-ac")
    public ResponseEntity<?> newAC() {
        return ResponseEntity.ok(userService.newAC());
    }

    @GetMapping("/view-ac")
    public ResponseEntity<?> viewAC() {
        return ResponseEntity.ok(userService.viewAC());
    }

    @PostMapping("/save-ac")
    public ResponseEntity<?> saveAC(@RequestBody Map<String, Object> acData) {
        return ResponseEntity.ok(userService.saveAC(acData));
    }

    @PostMapping("/delete-ac")
    public ResponseEntity<?> deleteAC(@RequestBody Map<String, Object> acData) {
        return ResponseEntity.ok(userService.deleteAC(acData));
    }

    @GetMapping("/roles")
    public ResponseEntity<?> roles() {
        return ResponseEntity.ok(userService.roles());
    }

    @PostMapping("/save-role")
    public ResponseEntity<?> saveRole(@RequestBody Map<String, Object> roleData) {
        return ResponseEntity.ok(userService.saveRole(roleData));
    }

    @PostMapping("/save-insurer")
    public ResponseEntity<?> saveInsurer(@RequestBody Map<String, Object> insurerData) {
        return ResponseEntity.ok(userService.saveInsurer(insurerData));
    }

    @GetMapping("/users")
    public ResponseEntity<?> users() {
        return ResponseEntity.ok(userService.users());
    }

    @PostMapping("/save-user")
    public ResponseEntity<?> saveUser(@RequestBody Map<String, Object> userData) {
        return ResponseEntity.ok(userService.saveUser(userData));
    }

    @GetMapping("/role-permission")
    public ResponseEntity<?> rolePermission() {
        return ResponseEntity.ok(userService.rolePermission());
    }
} 