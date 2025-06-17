package com.example.repository;

import com.example.model.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface UserRepository extends JpaRepository<User, Long> {
    
    Optional<User> findByEmail(String email);
    
    List<User> findByRole(String role);
    
    List<User> findByStatus(String status);
    
    Optional<User> findByResetToken(String resetToken);
    
    boolean existsByEmail(String email);
    
    List<User> findByRoleAndStatus(String role, String status);
    
    List<User> findByLastLoginDateBefore(java.time.LocalDateTime date);
} 