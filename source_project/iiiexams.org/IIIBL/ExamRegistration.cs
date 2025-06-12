using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using IIIDL;
using Utilities.FileParser;

namespace IIIBL
{
    public class ExamRegistration
    {
        IIIDL.ExamRegistrations objDP = null;

        public DataSet GetExambody(System.String ConnectionString)
        {

            DataSet objdataset = null;

            try
            {
                objDP = new IIIDL.ExamRegistrations();
                objdataset = objDP.GetExamBody(ConnectionString, 0, "A", 0);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDP = null;
            }
            return objdataset;
        }

        public DataSet GetTrainedApplicants(System.String ConnectionString, Int32 UserId , Int32 ExamBodyId, Int32 ExamCenterId, DateTime FromDate, DateTime ToDate)
        {
            IIIDL.ExamRegistrations objDP = null;
            DataSet objdataset = null;

            try
            {
                objDP = new IIIDL.ExamRegistrations();
                objdataset = objDP.GetTrainedApplicants(ConnectionString, UserId, ExamBodyId, ExamCenterId, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDP = null;
            }
            return objdataset;
        }


        //public DataSet ConfirmExamination(System.String ConnectionString, string AppData, string bactchmode, string paymentmode, string schmode, string remarks, int userid, out System.String Status)
        //{
        //    IIIDL.ExamRegistrations objDP = null;
        //    DataSet objdataset = null;

        //    try
        //    {
        //        objDP = new IIIDL.ExamRegistrations();
        //        objdataset = objDP.ConfirmExamination(ConnectionString, AppData, paymentmode, remarks, userid, bactchmode, schmode, out Status);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        objDP = null;
        //    }
        //    return objdataset;
        //}

        public DataSet BulkUploadExamRegData(string connectionString, string InputFile, int userid,out Boolean Status,out String Message)
        {
            String[] Fields = { "IRDA URN", "Payment Mode", "Insurer Remark", "Enrollment No", "OnOrAfterDate", "EmailIds", "Batch Mode", "Scheduling Mode" };
            ExamRegistrations objSp = null;
            DataSet objDataSet = null;
            DataTable oExcelData = null;
            Status = true;
            Message = String.Empty;
            try
            {
                Utilities.FileParser.ExcelParser objExcelParser = new ExcelParser();
                oExcelData = objExcelParser.ParseAllAsData(InputFile, "Sheet1$", true);

                for (Int32 i = oExcelData.Columns.Count - 1; i >= 0; i--)
                {
                    if (!Fields.Contains<String>(oExcelData.Columns[i].ColumnName))
                    {
                        oExcelData.Columns.RemoveAt(i);
                    }
                }

                if (oExcelData.Columns.Contains("ExamBatchNo")){ oExcelData.Columns.Remove("ExamBatchNo"); }
                if (oExcelData.Columns.Contains("IsValidRecord")){ oExcelData.Columns.Remove("IsValidRecord"); }
                if (oExcelData.Columns.Contains("UploadRemark")){ oExcelData.Columns.Remove("UploadRemark"); }

                oExcelData.Columns.Add("ExamBatchNo", typeof(System.String));
                oExcelData.Columns.Add("IsValidRecord", typeof(System.Boolean)).DefaultValue = true;
                oExcelData.Columns.Add("UploadRemark", typeof(System.String));

                if (oExcelData.Rows.Count == 0)
                {
                    Status = false;
                    Message = "File has no record";
                }
                else
                {
                    String s = String.Empty;
                    foreach (String fld in Fields)
                    {
                        if (!oExcelData.Columns.Contains(fld))
                        {
                            s += fld;
                            s += ",";
                        }
                    }
                    s = s.Trim(',');
                    if (s != String.Empty)
                    {
                        Status = false;
                        Message = String.Format("Invalid file format. The required fields : {0} not found.", s);
                    }
                    else
                    {
                        String val = String.Empty;
                        //Int32 InvalidRowCount = 0;
                        foreach (DataRow dr in oExcelData.Rows)
                        {
                            Boolean IsRecordValid = true;
                            String Error = String.Empty;
                            foreach (DataColumn c in oExcelData.Columns)
                            {
                                s = Convert.ToString(dr[c]).Trim();
                                Regex regex = new Regex(Common.regexLowAscii);
                                if (s != String.Empty)
                                {
                                    if (!regex.IsMatch(s))
                                    {
                                        Error += c.ColumnName + ",";
                                    }
                                }
                            }
                            Error = Error.Trim(',').Trim();

                            if (Error == String.Empty)
                            {//

                            }
                            else
                            {
                                Error = "Invalid characters found in columns: " + Error;
                                IsRecordValid = false;
                            }


                            //String fldval = Convert.ToString(dr["IRDA URN"]).Trim();
                            //if (fldval.Trim() == String.Empty)
                            //{
                            //    Error += " [IRDA URN] is required field ";
                            //    IsRecordValid = false;
                            //}

                            //fldval = Convert.ToString(dr["Payment Mode"]).Trim();
                            //if (fldval.Trim() == String.Empty)
                            //{
                            //    Error += " [Payment Mode] is required field ";
                            //    IsRecordValid = false;
                            //}
                            //else
                            //{
                            //    if ( ! (fldval.ToUpper() == "ONLINE PAYMENT" || fldval.ToUpper() == "CREDIT"))
                            //    {
                            //        Error += " Invalid [Payment Mode]. Expected - Online Payment OR Credit ";
                            //        IsRecordValid = false;
                            //    }
                            //}

                            String regexdate = TextValidator.GetPattern(typeof(DateTime), "dd-MMM-yyyy");
                            String strDate = Convert.ToString(dr["OnOrAfterDate"]).Trim();
                            if (strDate == String.Empty)
                            {
                                Error += " [OnOrAfterDate] is required field ";
                                IsRecordValid = false;
                            }
                            else
                            {
                                if (TextValidator.ValidateDate2(strDate, "dd-MMM-yyyy", regexdate))
                                {
                                    DateTime d1, d2, d3;
                                    string currDate = DateTime.Now.AddDays(3).ToString("dd-MMM-yyyy");
                                    string dt30 = DateTime.Now.AddDays(30).ToString("dd-MMM-yyyy");

                                    d1 = Convert.ToDateTime(strDate); // onorafterdate
                                    d2 = Convert.ToDateTime(currDate); //cureentdate
                                    d3 = Convert.ToDateTime(dt30);// 30 days from current date 

                                    int value = DateTime.Compare(d1, d2);
                                    int value1 = DateTime.Compare(d1, d3);

                                    if (value >= 0 && value1 <= 0)
                                    {

                                    }
                                    else
                                    {
                                        Error += "On or After Date must be between " + currDate + " & " + dt30;
                                        IsRecordValid = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    Error += "Invalid  [OnOrAfterDate]";
                                    IsRecordValid = false;
                                }
                            }

                            String Emails = Convert.ToString(dr["EmailIds"]).Trim();
                            if (Emails == string.Empty)
                            {

                            }
                            else
                            {
                                String[] MailIds = Emails.Split(',');
                                foreach (String MailId in MailIds)
                                {
                                    Regex r = new Regex(Common.regexEmail);
                                    if (!r.IsMatch(MailId)) //pattern matching
                                    {
                                        Error = "Invalid Email Id ";
                                        IsRecordValid = false;
                                        break;
                                    }
                                }
                            }

                            dr["IsValidRecord"] = IsRecordValid;
                            dr["UploadRemark"] = Error.Trim();
                        }

                        //Bulkupload Excel Data To Database
                        objSp = new IIIDL.ExamRegistrations();
                        objDataSet = objSp.BulkUploadToDatabase(connectionString, oExcelData, userid);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objSp = null;
            }
            return objDataSet;
        }

        public DataSet BulkUploadExamRegData2(string connectionString, DataTable dataTable, int userid)
        {
            ExamRegistrations objSp = null;
            DataSet oResultTable = null;
            try
            {
                //Bulkupload Excel Data To Database
                objSp = new IIIDL.ExamRegistrations();
                oResultTable = objSp.BulkUploadToDatabase(connectionString, dataTable, userid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objSp = null;
            }
            return oResultTable;
        }

        public DataSet GetPaymentModes(System.String ConnectionString, Int32 InsurerId)
        {
            IIIDL.ExamRegistrations objDP = null;
            DataSet objdataset = null;

            try
            {
                objDP = new IIIDL.ExamRegistrations();
                objdataset = objDP.GetPaymentMode(ConnectionString, InsurerId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objDP = null;
            }
            return objdataset;
        }

    }
}