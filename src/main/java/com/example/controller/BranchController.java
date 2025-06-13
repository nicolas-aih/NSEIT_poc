package com.example.controller;

import com.example.model.ApiResponse;
import com.example.model.Branch;
import com.example.service.BranchService;
import com.example.config.PortalSession;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

import java.util.List;

@Controller
@RequestMapping("/branches")
public class BranchController {

    @Autowired
    private BranchService branchService;

    @Autowired
    private PortalSession portalSession;

    @GetMapping
    @PreAuthorize("isAuthenticated()")
    public String getBranchesPage(Model model) {
        model.addAttribute("isLoggedOn", portalSession.getUserId() != 0);
        model.addAttribute("className", "col-sm-9");
        return "branches";
    }

    @PostMapping
    @PreAuthorize("isAuthenticated()")
    @ResponseBody
    public ResponseEntity<ApiResponse> addBranch(@RequestBody Branch branch) {
        try {
            // Validate required fields
            if (isNullOrEmpty(branch.getBranchCode())) {
                return ResponseEntity.ok(new ApiResponse(false, "Branch code is required"));
            }
            if (isNullOrEmpty(branch.getBranchName())) {
                return ResponseEntity.ok(new ApiResponse(false, "Branch name is required"));
            }
            if (isNullOrEmpty(branch.getBranchAddress())) {
                return ResponseEntity.ok(new ApiResponse(false, "Branch address is required"));
            }
            if (isNullOrEmpty(branch.getPlace())) {
                return ResponseEntity.ok(new ApiResponse(false, "Place is required"));
            }
            if (branch.getStateId() == null || branch.getStateId() <= 0) {
                return ResponseEntity.ok(new ApiResponse(false, "Valid state is required"));
            }
            if (branch.getDistrictId() == null || branch.getDistrictId() <= 0) {
                return ResponseEntity.ok(new ApiResponse(false, "Valid district is required"));
            }
            if (branch.getIsActive() == null) {
                return ResponseEntity.ok(new ApiResponse(false, "Active status is required"));
            }

            String message = branchService.insertBranch(
                portalSession.getUserId(),
                branch.getBranchAddress(),
                branch.getBranchCode(),
                branch.getBranchName(),
                branch.getPlace(),
                branch.getStateId(),
                branch.getDistrictId(),
                branch.getIsActive()
            );

            if (message.isEmpty()) {
                return ResponseEntity.ok(new ApiResponse(true, "Branch added successfully"));
            } else {
                return ResponseEntity.ok(new ApiResponse(false, message));
            }
        } catch (Exception e) {
            return ResponseEntity.ok(new ApiResponse(false, "An error occurred while processing your request"));
        }
    }

    @PutMapping("/{branchId}")
    @PreAuthorize("isAuthenticated()")
    @ResponseBody
    public ResponseEntity<ApiResponse> updateBranch(
            @PathVariable Long branchId,
            @RequestBody Branch branch) {
        try {
            // Validate required fields
            if (isNullOrEmpty(branch.getBranchCode())) {
                return ResponseEntity.ok(new ApiResponse(false, "Branch code is required"));
            }
            if (isNullOrEmpty(branch.getBranchName())) {
                return ResponseEntity.ok(new ApiResponse(false, "Branch name is required"));
            }
            if (isNullOrEmpty(branch.getBranchAddress())) {
                return ResponseEntity.ok(new ApiResponse(false, "Branch address is required"));
            }
            if (isNullOrEmpty(branch.getPlace())) {
                return ResponseEntity.ok(new ApiResponse(false, "Place is required"));
            }
            if (branch.getStateId() == null || branch.getStateId() <= 0) {
                return ResponseEntity.ok(new ApiResponse(false, "Valid state is required"));
            }
            if (branch.getDistrictId() == null || branch.getDistrictId() <= 0) {
                return ResponseEntity.ok(new ApiResponse(false, "Valid district is required"));
            }
            if (branch.getIsActive() == null) {
                return ResponseEntity.ok(new ApiResponse(false, "Active status is required"));
            }

            String message = branchService.updateBranch(
                branchId,
                portalSession.getUserId(),
                branch.getBranchAddress(),
                branch.getBranchCode(),
                branch.getBranchName(),
                branch.getPlace(),
                branch.getStateId(),
                branch.getDistrictId(),
                branch.getIsActive()
            );

            if (message.isEmpty()) {
                return ResponseEntity.ok(new ApiResponse(true, "Branch updated successfully"));
            } else {
                return ResponseEntity.ok(new ApiResponse(false, message));
            }
        } catch (Exception e) {
            return ResponseEntity.ok(new ApiResponse(false, "An error occurred while processing your request"));
        }
    }

    @GetMapping("/view")
    @PreAuthorize("isAuthenticated()")
    public String viewBranches(Model model) {
        model.addAttribute("isLoggedOn", portalSession.getUserId() != 0);
        model.addAttribute("className", "col-sm-9");
        return "view-branches";
    }

    @GetMapping("/search")
    @PreAuthorize("isAuthenticated()")
    @ResponseBody
    public ResponseEntity<ApiResponse> getBranchesForStateDistrict(
            @RequestParam Integer stateId,
            @RequestParam Integer districtId,
            @RequestParam(required = false) Integer branchId) {
        try {
            List<Branch> branches = branchService.getBranchesForStateDistrict(stateId, districtId, branchId);
            return ResponseEntity.ok(new ApiResponse(true, "Branches retrieved successfully", branches));
        } catch (Exception e) {
            return ResponseEntity.ok(new ApiResponse(false, "An error occurred while retrieving branches"));
        }
    }

    @GetMapping("/{branchId}")
    @PreAuthorize("isAuthenticated()")
    @ResponseBody
    public ResponseEntity<ApiResponse> getBranchDetails(@PathVariable Integer branchId) {
        try {
            Branch branch = branchService.getBranchDetails(branchId);
            if (branch != null) {
                return ResponseEntity.ok(new ApiResponse(true, "Branch details retrieved successfully", branch));
            } else {
                return ResponseEntity.ok(new ApiResponse(false, "Branch not found"));
            }
        } catch (Exception e) {
            return ResponseEntity.ok(new ApiResponse(false, "An error occurred while retrieving branch details"));
        }
    }

    @PostMapping("/upload")
    @PreAuthorize("isAuthenticated()")
    @ResponseBody
    public ResponseEntity<ApiResponse> uploadBranches(@RequestParam("file") MultipartFile file) {
        try {
            String message = branchService.uploadBranches(file, portalSession.getUserId());
            if (message.isEmpty()) {
                return ResponseEntity.ok(new ApiResponse(true, "Branches uploaded successfully"));
            } else {
                return ResponseEntity.ok(new ApiResponse(false, message));
            }
        } catch (Exception e) {
            return ResponseEntity.ok(new ApiResponse(false, "An error occurred while uploading branches"));
        }
    }

    @GetMapping("/download")
    @PreAuthorize("isAuthenticated()")
    public ResponseEntity<byte[]> downloadReport() {
        try {
            byte[] report = branchService.generateReport();
            return ResponseEntity
                .ok()
                .header("Content-Type", "application/vnd.ms-excel")
                .header("Content-Disposition", "attachment; filename=branches.xlsx")
                .body(report);
        } catch (Exception e) {
            return ResponseEntity.badRequest().build();
        }
    }

    private boolean isNullOrEmpty(String str) {
        return str == null || str.trim().isEmpty();
    }
} 