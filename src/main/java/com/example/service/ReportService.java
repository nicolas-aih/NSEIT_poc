package com.example.service;

import com.example.model.ExamReport;
import com.example.model.ScheduleReport;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;

@Service
public class ReportService {
    
    @Autowired
    private JdbcTemplate jdbcTemplate;

    @Value("${portal.connection-string}")
    private String connectionString;

    @Value("${portal.reports-dump-path}")
    private String reportsDumpPath;

    public List<ExamReport> getCorporateExaminationReport(String examMonth, Integer examYear, String roleName) {
        String sql = "EXEC sp_GetCorporateExaminationReport @ExamMonth = ?, @ExamYear = ?, @RoleName = ?";
        
        return jdbcTemplate.query(sql, 
            (rs, rowNum) -> {
                ExamReport report = new ExamReport();
                report.setUrn(rs.getString("URN"));
                report.setApplicantName(rs.getString("ApplicantName"));
                report.setExamDate(rs.getTimestamp("ExamDate").toLocalDateTime());
                report.setExamRollNumber(rs.getString("ExamRollNumber"));
                report.setExamMarks(rs.getInt("ExamMarks"));
                report.setResult(rs.getString("Result"));
                return report;
            },
            examMonth, examYear, roleName
        );
    }

    public List<ExamReport> getExaminationReport(Integer option, Integer userId) {
        String sql = "EXEC sp_GetExaminationReport @Option = ?, @UserId = ?";
        
        return jdbcTemplate.query(sql, 
            (rs, rowNum) -> {
                ExamReport report = new ExamReport();
                report.setApplicationDate(rs.getTimestamp("ApplicationDate").toLocalDateTime());
                report.setUrn(rs.getString("URN"));
                report.setApplicantName(rs.getString("ApplicantName"));
                report.setExamDate(rs.getTimestamp("ExamDate").toLocalDateTime());
                report.setExamRollNumber(rs.getString("ExamRollNumber"));
                report.setExamMarks(rs.getInt("ExamMarks"));
                report.setResult(rs.getString("Result"));
                
                // Additional fields for option 2 & 3
                if (option > 1) {
                    report.setTrainingStartDate(rs.getTimestamp("TrainingStartDate").toLocalDateTime());
                    report.setTrainingEndDate(rs.getTimestamp("TrainingEndDate").toLocalDateTime());
                    report.setTccExpiryDate(rs.getTimestamp("TCCExpiryDate").toLocalDateTime());
                    report.setTotalTrainingHours(rs.getInt("TotalTrainingHrs"));
                    report.setCenterName(rs.getString("CenterName"));
                    report.setPreferredDate(rs.getTimestamp("PreferredDate").toLocalDateTime());
                    report.setPaymentMode(rs.getString("PaymentMode"));
                    report.setSchedulingMode(rs.getString("SchedulingMode"));
                }
                
                return report;
            },
            option, userId
        );
    }

    public List<ScheduleReport> getScheduleReport(Integer hint, LocalDateTime fromDate, LocalDateTime toDate) {
        String sql = "EXEC sp_GetScheduleReport @Hint = ?, @FromDate = ?, @ToDate = ?";
        
        return jdbcTemplate.query(sql, 
            (rs, rowNum) -> {
                ScheduleReport report = new ScheduleReport();
                report.setExamDate(rs.getTimestamp("exam_date").toLocalDateTime());
                report.setCandidateCount(rs.getInt("candidate_count"));
                
                // Additional fields based on hint
                if (hint >= 2) {
                    report.setCenterId(rs.getInt("center_id"));
                    report.setCenterName(rs.getString("center_name"));
                }
                if (hint == 3) {
                    report.setExamBatch(rs.getString("exam_batch"));
                }
                
                return report;
            },
            hint, fromDate, toDate
        );
    }

    public List<Map<String, Object>> getCenterWisePendingScheduleCount() {
        String sql = "EXEC sp_GetCenterWisePendingScheduleCount";
        return jdbcTemplate.queryForList(sql);
    }

    public List<Map<String, Object>> getPaymentReport(String userLoginId, LocalDateTime fromDate, LocalDateTime toDate) {
        String sql = "EXEC sp_GeneratePaymentReport @UserLoginId = ?, @FromDate = ?, @ToDate = ?";
        return jdbcTemplate.queryForList(sql, userLoginId, fromDate, toDate);
    }

    public List<Map<String, Object>> getSponsorshipReport(
            LocalDateTime applicationDateFrom,
            LocalDateTime applicationDateTo,
            String roleName,
            Integer insurerUserId,
            Integer dpUserId,
            Integer acUserId,
            String urn,
            String insurerRefNo,
            String examBatch,
            Integer examBodyId,
            Integer examCenterId,
            boolean statusAll,
            boolean statusSponsored,
            boolean statusTrained,
            boolean statusEc,
            boolean statusEa,
            boolean statusE,
            boolean photo,
            boolean sign,
            LocalDateTime examDateFrom,
            LocalDateTime examDateTo) {
        
        String sql = "EXEC sp_GetSponsorshipReport @ApplicationDateFrom = ?, @ApplicationDateTo = ?, " +
                    "@RoleName = ?, @InsurerUserId = ?, @DPUserId = ?, @ACUserId = ?, @URN = ?, " +
                    "@InsurerRefNo = ?, @ExamBatch = ?, @ExamBodyId = ?, @ExamCenterId = ?, " +
                    "@StatusAll = ?, @StatusSponsored = ?, @StatusTrained = ?, @StatusEC = ?, " +
                    "@StatusEA = ?, @StatusE = ?, @Photo = ?, @Sign = ?, @ExamDateFrom = ?, @ExamDateTo = ?";
        
        return jdbcTemplate.queryForList(sql,
            applicationDateFrom, applicationDateTo, roleName, insurerUserId, dpUserId,
            acUserId, urn, insurerRefNo, examBatch, examBodyId, examCenterId,
            statusAll, statusSponsored, statusTrained, statusEc, statusEa, statusE,
            photo, sign, examDateFrom, examDateTo);
    }

    public Map<String, Object> generateCorporateExaminationReport(String examMonth, int examYear, String roleName) {
        return jdbcTemplate.queryForMap(
            "EXEC sp_GetCorporateExaminationReport @ExamMonth=?, @ExamYear=?, @RoleName=?",
            examMonth, examYear, roleName
        );
    }

    public Map<String, Object> generateApprovedCorporateAgentReport() {
        return jdbcTemplate.queryForMap(
            "EXEC sp_GetApprovedCorporateAgent"
        );
    }

    public Map<String, Object> generateSponsorshipReport(
            LocalDateTime applicationDateFrom,
            LocalDateTime applicationDateTo,
            String roleName,
            Integer insurerUserId,
            Integer dpUserId,
            Integer acUserId,
            String urn,
            String insurerRefNo,
            String examBatch,
            Integer examBodyId,
            Integer examCenterId,
            boolean statusAll,
            boolean statusSponsored,
            boolean statusTrained,
            boolean statusEC,
            boolean statusEA,
            boolean statusE,
            boolean photo,
            boolean sign,
            LocalDateTime examDateFrom,
            LocalDateTime examDateTo) {
        
        return jdbcTemplate.queryForMap(
            "EXEC sp_GetSponsorshipReport @ApplicationDateFrom=?, @ApplicationDateTo=?, @RoleName=?, " +
            "@InsurerUserId=?, @DPUserId=?, @ACUserId=?, @URN=?, @InsurerRefNo=?, @ExamBatch=?, " +
            "@ExamBodyId=?, @ExamCenterId=?, @StatusAll=?, @StatusSponsored=?, @StatusTrained=?, " +
            "@StatusEC=?, @StatusEA=?, @StatusE=?, @Photo=?, @Sign=?, @ExamDateFrom=?, @ExamDateTo=?",
            applicationDateFrom, applicationDateTo, roleName, insurerUserId, dpUserId, acUserId,
            urn, insurerRefNo, examBatch, examBodyId, examCenterId, statusAll, statusSponsored,
            statusTrained, statusEC, statusEA, statusE, photo, sign, examDateFrom, examDateTo
        );
    }

    public Map<String, Object> generateExaminationReport(int option, int userId) {
        return jdbcTemplate.queryForMap(
            "EXEC sp_GetExaminationReport @Option=?, @UserId=?",
            option, userId
        );
    }

    public Map<String, Object> generateScheduleReport(int hint, LocalDateTime fromDate, LocalDateTime toDate) {
        return jdbcTemplate.queryForMap(
            "EXEC sp_GetScheduleReport @Hint=?, @FromDate=?, @ToDate=?",
            hint, fromDate, toDate
        );
    }

    public Map<String, Object> generateCenterWisePendingScheduleReport() {
        return jdbcTemplate.queryForMap(
            "EXEC sp_GetCenterWisePendingScheduleCount"
        );
    }

    public Map<String, Object> generatePaymentReport(LocalDateTime fromDate, LocalDateTime toDate, String topUserLoginId) {
        return jdbcTemplate.queryForMap(
            "EXEC sp_GeneratePaymentReport @FromDate=?, @ToDate=?, @TopUserLoginId=?",
            fromDate, toDate, topUserLoginId
        );
    }

    public byte[] generatePaymentReceipts(String[] transactionIds) {
        // TODO: Implement logic to generate and zip payment receipts
        return null;
    }
} 