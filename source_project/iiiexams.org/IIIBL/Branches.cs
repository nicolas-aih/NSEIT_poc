using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using IIIDL;
using Utilities.FileParser;
using System.Text.RegularExpressions;

namespace IIIBL
{
    public class Branches
    {
        public String InsertBranches(System.String ConnectionString, System.Int32 UserId, System.String BranchAddress, System.String BranchCode, System.String BranchName, System.String BranchPlace, System.Int32 StateId, System.Int32 DistrictId, System.Boolean IsActive)
        {
            IIIDL.Branches objBranches = null;
            String Result = String.Empty;
            try
            {
                objBranches = new IIIDL.Branches();
                Result = objBranches.InsertBranches(ConnectionString, UserId, BranchAddress, BranchCode, BranchName, BranchPlace, StateId, DistrictId, IsActive);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objBranches = null;
            }
            return Result;
        }

        public String UpdateBranches(System.String ConnectionString, Int64 BranchId, System.Int32 UserId, System.String BranchAddress, System.String BranchCode, System.String BranchName, System.String BranchPlace, System.Int32 StateId, System.Int32 DistrictId, System.Boolean IsActive)
        {
            IIIDL.Branches objBranches = null;
            String Result = String.Empty;
            try
            {
                objBranches = new IIIDL.Branches();
                Result = objBranches.UpdateBranches(ConnectionString, BranchId, UserId, BranchAddress, BranchCode, BranchName, BranchPlace, StateId, DistrictId, IsActive);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objBranches = null;
            }
            return Result;
        }

        public DataSet GetBranchDetails(System.String ConnectionString, System.Int32 UserId, System.Int32 StateId, System.Int32 DistrictId)
        {
            IIIDL.Branches objBranches = null;
            DataSet objDataSet = null;
            try
            {
                objBranches = new IIIDL.Branches();
                objDataSet = objBranches.GetBranchDetails(ConnectionString, UserId, StateId, DistrictId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objBranches = null;
            }
            return objDataSet;
        }

        public DataSet GetBranchDetails(System.String ConnectionString, System.Int32 UserId, System.Int32 BranchId)
        {
            IIIDL.Branches objBranches = null;
            DataSet objDataSet = null;
            try
            {
                objBranches = new IIIDL.Branches();
                objDataSet = objBranches.GetBranchDetails(ConnectionString, UserId, BranchId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objBranches = null;
            }
            return objDataSet;
        }

        public DataSet UploadBranches(System.String ConnectionString, System.Int32 CorporateUserId, String FileNameAndPath, out System.String Message)
        {
            String[] Fields = { "Branch Code", "Branch Name", "City", "Address", "State", "District" , "IsActive"};
            IIIDL.Branches objBranches = null;
            DataSet objDataSet = null;
            ExcelParser excelParser = null;
            DataTable ExcelOutput = null;
            try
            {
                DataTable BranchDetails = new DataTable();
                BranchDetails.Columns.Add("varBranchCode", typeof(String));
                BranchDetails.Columns.Add("varBranchName", typeof(String));
                BranchDetails.Columns.Add("varBranchPlace", typeof(String));
                BranchDetails.Columns.Add("varBranchAddress", typeof(String));
                BranchDetails.Columns.Add("State", typeof(String));
                BranchDetails.Columns.Add("District", typeof(String));
                BranchDetails.Columns.Add("IsActive", typeof(String));
                BranchDetails.Columns.Add("UploadRemark", typeof(String));
                BranchDetails.Columns.Add("IsValidRecord", typeof(Int32));

                excelParser = new ExcelParser();
                ExcelOutput = excelParser.ParseAllAsData(FileNameAndPath, "Sheet1$", true);

                for (Int32 i = ExcelOutput.Columns.Count -1; i >= 0; i-- )
                {
                    if (!Fields.Contains<String>(ExcelOutput.Columns[i].ColumnName))
                    {
                        ExcelOutput.Columns.RemoveAt(i);
                    }
                }

                ExcelOutput.Columns.Add("Upload Remarks", typeof(String));
                ExcelOutput.Columns.Add("IsValidRecord", typeof(Int32));

                if (ExcelOutput.Rows.Count == 0)
                {
                    Message = "File has no record";
                }
                else
                {
                    String s = String.Empty;
                    foreach (String fld in Fields)
                    {
                        if (!ExcelOutput.Columns.Contains(fld))
                        {
                            s += fld;
                            s += ",";
                        }
                    }
                    s = s.Trim(',');
                    if (s != String.Empty)
                    {
                        Message = String.Format("Invalid file format. The required fields : {0} not found.", s);
                    }
                    else
                    {
                        String val = String.Empty;
                        Int32 InvalidRowCount = 0;
                        foreach (DataRow dr in ExcelOutput.Rows)
                        {
                            String Error = String.Empty;
                            foreach (DataColumn c in ExcelOutput.Columns)
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
                            {
                                dr["Upload Remarks"] = String.Empty;
                                dr["IsValidRecord"] = 1;
                            }
                            else
                            {
                                Error = "Invalid charachters found in columns: " + Error;
                                dr["IsValidRecord"] = false;
                                dr["Upload Remarks"] = Error;
                                InvalidRowCount++;
                            }

                            BranchDetails.Rows.Add(
                                dr["Branch Code"],
                                dr["Branch Name"],
                                dr["City"],
                                dr["Address"],
                                dr["State"],
                                dr["District"],
                                dr["IsActive"],
                                dr["Upload Remarks"],
                                dr["IsValidRecord"]
                                );
                        }

                        if (InvalidRowCount == BranchDetails.Rows.Count)
                        {
                            objDataSet = new DataSet();
                            objDataSet.Tables.Add(ExcelOutput);
                            Message = "Unable to proceed. Kindly rectify the errors and try uploading again";
                        }
                        else
                        {
                            Message = String.Empty;
                            objBranches = new IIIDL.Branches();
                            objDataSet = objBranches.UploadBranches(ConnectionString, CorporateUserId, BranchDetails, out Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objBranches = null;
            }
            return objDataSet;
        }

        public DataSet DownloadReport(System.String ConnectionString, System.Int32 UserId)
        {
            IIIDL.Branches objBranches = null;
            DataSet objDataSet = null;
            try
            {
                objBranches = new IIIDL.Branches();
                objDataSet = objBranches.GetBranchReport(ConnectionString, UserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objBranches = null;
            }
            return objDataSet;
        }

    }
}
