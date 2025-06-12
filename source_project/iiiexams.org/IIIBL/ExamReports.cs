using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using IIIDL;
using Utilities.FileParser;
using System.IO;


namespace IIIBL
{
    public class ExamReports
    {
        public DataSet GetCorporateExaminationReport(System.String ConnectionString, System.String ExamMonth, System.Int32 ExamYear, System.String UserRole)
        {
            IIIDL.ExamReports obj = null;
            DataSet objDataSet = null;
            try
            {
                obj = new IIIDL.ExamReports();
                objDataSet = obj.GetCorporateExaminationReport(ConnectionString, ExamMonth, ExamYear, UserRole);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                obj = null;
            }
            return objDataSet;
        }

        public DataSet GetApprovedCorporateAgent(System.String ConnectionString)
        {
            IIIDL.ExamReports objExamReports = null;
            DataSet objDataSet = null;

            try
            {
                objExamReports = new IIIDL.ExamReports();
                objDataSet = objExamReports.GetApprovedCorporateAgent(ConnectionString);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objExamReports = null;
            }
            return objDataSet;
        }

        public DataSet GetSponsorshipReportForCorporates(System.String ConnectionString, System.Int32 InsurerUserId, System.DateTime ApplicationDateFrom,
            System.DateTime ApplicationDateTo, System.String UserRole, System.String URN, System.String InsurerExtnRefNo,
            System.String ExamBatch, System.Int32 ExamBodyId, System.Int32 ExamCenterId,
            Boolean appStatusALL, Boolean appStatusS, Boolean appStatusT, Boolean appStatusEC, Boolean appStatusEA,
            Boolean appStatusE, Boolean includephoto, Boolean includesign, System.String ExamDateFrom, System.String ExamDateTo)
        {
            IIIDL.ExamReports objExamReports = null;
            DataSet objDataSet = null;

            try
            {
                objExamReports = new IIIDL.ExamReports();
                objDataSet = objExamReports.GetSponsorshipReportForCorporates(ConnectionString, InsurerUserId, ApplicationDateFrom,
               ApplicationDateTo, UserRole, URN, InsurerExtnRefNo,
             ExamBatch, ExamBodyId, ExamCenterId,
             appStatusALL, appStatusS, appStatusT, appStatusEC, appStatusEA,
             appStatusE, includephoto, includesign, ExamDateFrom, ExamDateTo);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objExamReports = null;
            }
            return objDataSet;
        }

        public DataSet GetSponsorshipReport(System.String ConnectionString, System.DateTime ApplicationDateFrom, System.DateTime ApplicationDateTo,
            System.String UserRole, Int16 Tntinstypeid, System.Int32 InsurerUserId, Int32 DPUserId, Int32 ACUserId, System.String URN, System.String InsurerExtnRefNo,
            System.String ExamBatch, System.Int32 ExamBodyId, System.Int32 ExamCenterId,
            Boolean appStatusALL, Boolean appStatusS, Boolean appStatusT, Boolean appStatusEC, Boolean appStatusEA,
            Boolean appStatusE, Boolean includephoto, Boolean includesign, System.String ExamDateFrom, System.String ExamDateTo)
        {
            IIIDL.ExamReports objExamReports = null;
            DataSet objDataSet = null;
            try
            {
                objExamReports = new IIIDL.ExamReports();
                objDataSet = objExamReports.GetSponsorshipReport(ConnectionString, ApplicationDateFrom, ApplicationDateTo,
                                                                 UserRole, Tntinstypeid, InsurerUserId, DPUserId, ACUserId, URN, InsurerExtnRefNo,
                                                                 ExamBatch, ExamBodyId, ExamCenterId,
                                                                 appStatusALL, appStatusS, appStatusT, appStatusEC, appStatusEA,
                                                                 appStatusE, includephoto, includesign, ExamDateFrom, ExamDateTo);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objExamReports = null;
            }
            return objDataSet;
        }

        public DataSet UploadExamDetails(System.String ConnectionString, String FileNameAndPath, Boolean AllocateSlot, System.Int32 UserId, out String Message)
        {
            Message = String.Empty;
            DataSet objDataSet = null;
            try
            {
                FileInfo fi = new FileInfo(FileNameAndPath);
                if (fi.Exists)
                {
                    Field[] fields = new Field[8];
                    if (AllocateSlot)
                    {
                        fields[0] = new Field("IRDA URN", String.Empty, 0, 0, typeof(String), String.Empty, false);
                        fields[1] = new Field("Exam Date", String.Empty, 1, 0, typeof(DateTime), "MM/dd/yyyy", false);
                        fields[2] = new Field("Exam Time", String.Empty, 2, 0, typeof(DateTime), "HH:mm", false);
                        fields[3] = new Field("Exam Result", String.Empty, 3, 0, typeof(String), String.Empty, true);
                        fields[4] = new Field("Exam Marks", String.Empty, 4, 0, typeof(Int32), String.Empty, true);
                        fields[5] = new Field("Exam Roll No", String.Empty, 5, 0, typeof(String), String.Empty, true);
                        fields[6] = new Field("is Allocated(Y / N)", String.Empty, 6, 0, typeof(String), String.Empty, true);
                        fields[7] = new Field("ExamCenterCode", String.Empty, 7, 0, typeof(String), String.Empty, true);
                        //------------------
                        //fields[8] = new Field("Process Successful(Y / N)", String.Empty, 8, 0, typeof(String), String.Empty, true);
                        //fields[9] = new Field("Remarks", String.Empty, 9, 0, typeof(String), String.Empty, true);
                    }
                    else
                    {
                        fields[0] = new Field("IRDA URN", String.Empty, 0, 0, typeof(String), String.Empty, false);
                        fields[1] = new Field("Exam Date", String.Empty, 1, 0, typeof(DateTime), "MM/dd/yyyy", false);
                        fields[2] = new Field("Exam Time", String.Empty, 2, 0, typeof(DateTime), "HH:mm", false);
                        fields[3] = new Field("Exam Result", String.Empty, 3, 0, typeof(String), String.Empty, false);
                        fields[4] = new Field("Exam Marks", String.Empty, 4, 0, typeof(Int32), String.Empty, true);
                        fields[5] = new Field("Exam Roll No", String.Empty, 5, 0, typeof(String), String.Empty, false);
                        fields[6] = new Field("is Allocated(Y / N)", String.Empty, 6, 0, typeof(String), String.Empty, false);
                        fields[7] = new Field("ExamCenterCode", String.Empty, 7, 0, typeof(String), String.Empty, false);
                        //------------------
                        //fields[8] = new Field("Process Successful(Y / N)", String.Empty, 8, 0, typeof(String), String.Empty, true);
                        //fields[9] = new Field("Remarks", String.Empty, 9, 0, typeof(String), String.Empty, true);
                    }

                    System.Collections.Specialized.StringCollection Headers = new System.Collections.Specialized.StringCollection();
                    System.Collections.Specialized.StringCollection Footers = new System.Collections.Specialized.StringCollection();

                    IIIDL.ExamReports objExamReports = null;
                    TextParser textParser = null;
                    DataTable InputData = null;
                    try
                    {
                        textParser = new TextParser();
                        InputData = textParser.Parse(FileNameAndPath, 1, false, "\t", fields, 0, out Headers, out Footers);

                        foreach (DataRow dr in InputData.Rows)
                        {
                            if (dr["Exam Date"] != DBNull.Value && dr["Exam Time"] != DBNull.Value)
                            {
                                DateTime dt1 = Convert.ToDateTime(dr["Exam Date"]);
                                DateTime dt2 = Convert.ToDateTime(dr["Exam Time"]);
                                dr["Exam Date"] = Convert.ToDateTime(dt1.ToString("dd-MMM-yyyy") + " " + dt2.ToString("hh:mm:ss tt"));
                            }
                        }

                        objExamReports = new IIIDL.ExamReports();
                        objDataSet = objExamReports.UploadExamDetails(ConnectionString, InputData, AllocateSlot, UserId);
                        Message = String.Empty;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        textParser = null;
                        InputData = null;
                    }
                }
                else
                {
                    Message = "File Not Found";
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {

            }
            return objDataSet;
        }

        public DataSet UploadAIMSResponse(System.String ConnectionString, String FileNameAndPath, System.Int32 UserId, out String Message)
        {
            Message = String.Empty;
            DataSet objDataSet = null;
            try
            {
                FileInfo fi = new FileInfo(FileNameAndPath);
                if (fi.Exists)
                {
                    Field[] fields = new Field[6];
                    fields[0] = new Field("TransactionId", String.Empty, 0, 0, typeof(String), String.Empty, false);
                    fields[1] = new Field("IRDA URN", String.Empty, 1, 0, typeof(String), String.Empty, false);
                    fields[2] = new Field("Sponsorship Date", String.Empty, 2, 0, typeof(DateTime), "MM/dd/yyyy", false);
                    fields[3] = new Field("Candidate First Name", String.Empty, 3, 0, typeof(String), String.Empty, false);
                    fields[4] = new Field("Type", String.Empty, 4, 0, typeof(String), String.Empty, false);
                    fields[5] = new Field("Remarks", String.Empty, 5, 0, typeof(String), String.Empty, false);

                    System.Collections.Specialized.StringCollection Headers = new System.Collections.Specialized.StringCollection();
                    System.Collections.Specialized.StringCollection Footers = new System.Collections.Specialized.StringCollection();

                    IIIDL.ExamReports objExamReports = null;
                    TextParser textParser = null;
                    DataTable InputData = null;
                    try
                    {
                        textParser = new TextParser();
                        InputData = textParser.Parse(FileNameAndPath, 1, false, "\t", fields, 0, out Headers, out Footers);
                        objExamReports = new IIIDL.ExamReports();
                        objDataSet = objExamReports.UploadAIMSResponse(ConnectionString, InputData, UserId);
                        Message = String.Empty;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        textParser = null;
                        InputData = null;
                    }
                }
                else
                {
                    Message = "File Not Found";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return objDataSet;
        }

        public DataSet DownloadApplicantDetails(System.String ConnectionString, System.Int32 UserId, System.DateTime FromDate, System.DateTime TillDate)
        {
            IIIDL.ExamReports objExamReports = null;
            DataSet objDataSet = null;
            try
            {
                objExamReports = new IIIDL.ExamReports();
                objDataSet = objExamReports.DownloadApplicantDetails(ConnectionString, UserId, FromDate, TillDate);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objExamReports = null;
            }
            return objDataSet;
        }

        public DataSet GetExaminationReport(System.String ConnectionString, System.Int32 Option, System.Int32 UserId)
        {
            IIIDL.ExamReports obj = null;
            DataSet objdataset = null;

            try
            {
                obj = new IIIDL.ExamReports();
                objdataset = obj.GetExaminationReport(ConnectionString, Option, UserId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                obj = null;
            }
            return objdataset;
        }

        public DataSet GeneratePaymentReport(String OracleConnectionString, System.String CompanyCode, System.DateTime FromDate, System.DateTime ToDate)
        {
            IIIDL.ExamReports obj = null;
            DataSet objdataset = null;

            try
            {
                obj = new IIIDL.ExamReports();
                objdataset = obj.GeneratePaymentReportOAIMS(OracleConnectionString, CompanyCode, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                obj = null;
            }
            return objdataset;
        }



    }
}
