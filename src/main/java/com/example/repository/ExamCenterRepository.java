package com.example.repository;

import com.example.model.ExamCenter;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface ExamCenterRepository extends JpaRepository<ExamCenter, Long> {
    
    List<ExamCenter> findByStateId(Integer stateId);
    
    List<ExamCenter> findByDistrictId(Integer districtId);
    
    List<ExamCenter> findByStateIdAndDistrictId(Integer stateId, Integer districtId);
    
    List<ExamCenter> findByStatus(String status);
    
    boolean existsByCenterCode(String centerCode);
    
    @Query("SELECT ec FROM ExamCenter ec WHERE ec.capacity BETWEEN :minCapacity AND :maxCapacity")
    List<ExamCenter> findByCapacityRange(@Param("minCapacity") Integer minCapacity, 
                                       @Param("maxCapacity") Integer maxCapacity);
    
    @Query(value = "SELECT * FROM exam_centers WHERE " +
           "6371 * acos(cos(radians(:latitude)) * cos(radians(latitude)) * " +
           "cos(radians(longitude) - radians(:longitude)) + " +
           "sin(radians(:latitude)) * sin(radians(latitude))) < :distance " +
           "ORDER BY (6371 * acos(cos(radians(:latitude)) * cos(radians(latitude)) * " +
           "cos(radians(longitude) - radians(:longitude)) + " +
           "sin(radians(:latitude)) * sin(radians(latitude))))", 
           nativeQuery = true)
    List<ExamCenter> findNearbyCenters(@Param("latitude") Double latitude,
                                      @Param("longitude") Double longitude,
                                      @Param("distance") Double distance);
    
    @Query("SELECT ec FROM ExamCenter ec WHERE ec.pincode = :pincode")
    List<ExamCenter> findByPincode(@Param("pincode") String pincode);
} 