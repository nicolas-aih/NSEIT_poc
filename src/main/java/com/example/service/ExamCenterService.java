package com.example.service;

import com.example.model.ExamCenter;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.sql.ResultSet;
import java.util.List;
import java.util.Map;

@Service
public class ExamCenterService {
    
    private final JdbcTemplate jdbcTemplate;

    @Value("${portal.connection-string}")
    private String connectionString;

    public ExamCenterService(JdbcTemplate jdbcTemplate) {
        this.jdbcTemplate = jdbcTemplate;
    }

    public List<ExamCenter> getExamCentersForDownload() {
        String sql = "SELECT ec.*, s.StateName, d.DistrictName " +
                    "FROM ExamCenters ec " +
                    "INNER JOIN States s ON ec.StateId = s.StateId " +
                    "INNER JOIN Districts d ON ec.DistrictId = d.DistrictId " +
                    "WHERE ec.IsActive = 1";
        
        return jdbcTemplate.query(sql, (ResultSet rs, int rowNum) -> {
            ExamCenter center = new ExamCenter();
            center.setId(rs.getLong("ExamCenterId"));
            center.setName(rs.getString("CenterName"));
            center.setAddress(rs.getString("Address"));
            center.setStateId(rs.getInt("StateId"));
            center.setStateName(rs.getString("StateName"));
            center.setDistrictId(rs.getInt("DistrictId"));
            center.setDistrictName(rs.getString("DistrictName"));
            center.setPincode(rs.getString("Pincode"));
            center.setCapacity(rs.getInt("Capacity"));
            return center;
        });
    }

    public List<ExamCenter> findNearest3ExamCenters(Integer pincode) {
        String sql = "EXEC sp_FindNearest3ExamCenters @Pincode = ?";
        
        return jdbcTemplate.query(sql, 
            new Object[]{pincode},
            (ResultSet rs, int rowNum) -> {
                ExamCenter center = new ExamCenter();
                center.setId(rs.getLong("ExamCenterId"));
                center.setName(rs.getString("CenterName"));
                center.setAddress(rs.getString("Address"));
                center.setStateId(rs.getInt("StateId"));
                center.setDistrictId(rs.getInt("DistrictId"));
                center.setPincode(rs.getString("Pincode"));
                center.setDistance(rs.getDouble("Distance"));
                return center;
            }
        );
    }

    public List<ExamCenter> getCentersForDistrict(Integer districtId) {
        String sql = "SELECT * FROM ExamCenters WHERE DistrictId = ? AND IsActive = 1";
        
        return jdbcTemplate.query(sql, 
            new Object[]{districtId},
            (ResultSet rs, int rowNum) -> {
                ExamCenter center = new ExamCenter();
                center.setId(rs.getLong("ExamCenterId"));
                center.setName(rs.getString("CenterName"));
                center.setAddress(rs.getString("Address"));
                center.setStateId(rs.getInt("StateId"));
                center.setDistrictId(rs.getInt("DistrictId"));
                center.setPincode(rs.getString("Pincode"));
                center.setCapacity(rs.getInt("Capacity"));
                return center;
            }
        );
    }

    public Map<String, Object> getSeatsAvailability() {
        String sql = "EXEC sp_GetSeatsAvailability";
        
        return jdbcTemplate.queryForMap(sql);
    }
} 