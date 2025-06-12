using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using System.Data;
using IIIBL;
using System.Configuration;
using IIIRegistrationPortal2;
using System.Text;
using System.IO;
using System.Reflection;

namespace IIIRegistrationPortal2.Controllers
{
    public class BranchesController : Controller
    {
        [HttpGet]
        [AuthorizeExt]
        public ActionResult Branches()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult AddBranch()
        {
            JsonResult objJsonResult = null;
            IIIBL.Branches objBranches = null;
            String Message = String.Empty;
            Boolean Status = true;
            String s = String.Empty;
            //Validate

            String BranchCode = String.Empty;
            String BranchName = string.Empty;
            String BranchAddress = string.Empty;
            String Place = string.Empty;
            Int32 StateId = -1;
            Int32 DistrictId = -1;
            String IsActive = string.Empty;


            try
            {
                    
                BranchCode = Convert.ToString(Request.Form["txtBranchCode"]).Trim();
                if (BranchCode.Trim() == String.Empty)
                {
                    throw new Exception("Invalid BranchCode");
                }

                BranchName = Convert.ToString(Request.Form["txtBranchName"]).Trim();
                if (BranchName.Trim() == String.Empty)
                {
                    throw new Exception("Invalid BranchName");
                }

                BranchAddress = Convert.ToString(Request.Form["txtBranchAddress"]).Trim();
                if (BranchAddress.Trim() == String.Empty)
                {
                    throw new Exception("Invalid BranchAddress");
                }

                Place = Convert.ToString(Request.Form["txtPlace"]).Trim();
                if (Place.Trim() == String.Empty)
                {
                    throw new Exception("Invalid Place");
                }

                String _dummy = Convert.ToString(Request.Form["cboStates"]).Trim();
                StateId = Convert.ToInt32(_dummy);

                _dummy = Convert.ToString(Request.Form["cboDistricts"]).Trim();
                DistrictId = Convert.ToInt32(_dummy);

                IsActive = Convert.ToString(Request.Form["cboIsActive"]).Trim();
                if (IsActive.Trim() == String.Empty && !IsActive.In(new String[] { "ACTIVE", "INACTIVE" }))
                {
                    throw new Exception("Invalid IsActive");
                }
            }
            catch (Exception ex)
            {
                Status = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }


            //Change
            if (Status)
            {
                try
                {
                    objBranches = new IIIBL.Branches();
                    Message = objBranches.InsertBranches(PortalApplication.ConnectionString, PortalSession.UserID, BranchAddress, BranchCode, BranchName, Place, StateId, DistrictId, IsActive == "ACTIVE");
                    if (Message.Trim() == String.Empty)
                    {
                        Status = true;
                        Message = CommonMessages.DATA_SAVE_SUCCESS;
                    }
                    else
                    {
                        Status = false;
                    }
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    Status = false;
                    Message = CommonMessages.ERROR_OCCURED;
                }
                finally
                {
                    objBranches = null;
                }
            }
            else
            {
                Status = false;
            }

            s = HelperUtilities.ToJSON(Status, Message);
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult UpdateBranch()
        {
            JsonResult objJsonResult = null;
            IIIBL.Branches objBranches = null;
            String Message = String.Empty;
            Boolean Status = true;
            String s = String.Empty;

            Int64 BranchId = -1;
            String BranchCode = String.Empty;
            String BranchName = string.Empty;
            String BranchAddress = string.Empty;
            String Place = string.Empty;
            Int32 StateId = -1;
            Int32 DistrictId = -1;
            String IsActive = string.Empty;


            try
            {
                String _dummy = Convert.ToString(Request.Form["txtBranchNo"]).Trim();
                BranchId = Convert.ToInt64(_dummy);

                BranchCode = Convert.ToString(Request.Form["txtBranchCode"]).Trim();
                if (BranchCode.Trim() == String.Empty)
                {
                    throw new Exception("Invalid BranchCode");
                }

                BranchName = Convert.ToString(Request.Form["txtBranchName"]).Trim();
                if (BranchName.Trim() == String.Empty)
                {
                    throw new Exception("Invalid BranchName");
                }

                BranchAddress = Convert.ToString(Request.Form["txtBranchAddress"]).Trim();
                if (BranchAddress.Trim() == String.Empty)
                {
                    throw new Exception("Invalid BranchAddress");
                }

                Place = Convert.ToString(Request.Form["txtPlace"]).Trim();
                if (Place.Trim() == String.Empty)
                {
                    throw new Exception("Invalid Place");
                }

                _dummy = Convert.ToString(Request.Form["cboStates"]).Trim();
                StateId = Convert.ToInt32(_dummy);

                _dummy = Convert.ToString(Request.Form["cboDistricts"]).Trim();
                DistrictId = Convert.ToInt32(_dummy);

                IsActive = Convert.ToString(Request.Form["cboIsActive"]).Trim();
                if (IsActive.Trim() == String.Empty && !IsActive.In(new String[] { "ACTIVE", "INACTIVE" }))
                {
                    throw new Exception("Invalid IsActive");
                }
            }
            catch (Exception ex)
            {
                Status = false;
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex, Request.Form);
                s = HelperUtilities.ToJSON(false, CommonMessages.INVALID_INPUT);
            }

            //Change
            if (Status)
            {
                try
                {
                    objBranches = new IIIBL.Branches();
                    Message = objBranches.UpdateBranches(PortalApplication.ConnectionString, BranchId , PortalSession.UserID, BranchAddress, BranchCode, BranchName, Place, StateId, DistrictId, IsActive == "ACTIVE");
                    if (Message.Trim() == String.Empty)
                    {
                        Status = true;
                        Message = CommonMessages.DATA_SAVE_SUCCESS;
                    }
                    else
                    {
                        Status = false;
                    }
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                    Status = false;
                    Message = CommonMessages.ERROR_OCCURED;
                }
                finally
                {
                    objBranches = null;
                }
            }
            s = HelperUtilities.ToJSON(Status, Message);
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpGet]
        [AuthorizeExt]
        public ActionResult ViewBranches()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetBranchesForStateDistrict(Int32 StateId, Int32 DistrictId, Int32? BranchId = null)
        {
            JsonResult objJsonResult = null;
            IIIBL.Branches objBranches = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            Boolean Success = false;
            String Message = String.Empty;
            String s = String.Empty;
            try
            {
                objBranches = new IIIBL.Branches();
                objDataSet = objBranches.GetBranchDetails(PortalApplication.ConnectionString, PortalSession.UserID, StateId, DistrictId);
                if (objDataSet != null && objDataSet.Tables.Count == 1)
                {
                    objDataTable = objDataSet.Tables[0];
                    if (objDataTable.Rows.Count > 0)
                    {
                        Success = true;
                        Message = String.Empty;
                    }
                    else
                    {
                        Success = true;
                        Message = CommonMessages.NO_DATA_FOUND;
                    }
                }
                else
                {
                    Success = true;
                    Message = CommonMessages.NO_DATA_FOUND;
                }
                s = HelperUtilities.ToJSON(Success, Message, objDataTable);
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                Success = false;
                Message = CommonMessages.ERROR_OCCURED;
                s = HelperUtilities.ToJSON(Success, Message);
            }
            finally
            {
                objDataTable = null;
                objDataSet = null;
                objBranches = null;
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult GetBranchDetailsForModification(Int32 BranchId)
        {
            JsonResult objJsonResult = null;
            IIIBL.Branches objBranches = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            Boolean Success = false;
            String Message = String.Empty;
            String s = String.Empty;
            try
            {
                objBranches = new IIIBL.Branches();
                objDataSet = objBranches.GetBranchDetails(PortalApplication.ConnectionString, PortalSession.UserID, BranchId);
                if (objDataSet != null && objDataSet.Tables.Count == 1)
                {
                    objDataTable = objDataSet.Tables[0];
                    if (objDataTable.Rows.Count > 0)
                    {
                        Success = true;
                        Message = String.Empty;
                    }
                    else
                    {
                        Success = true;
                        Message = CommonMessages.NO_DATA_FOUND;
                    }
                }
                else
                {
                    Success = true;
                    Message = CommonMessages.NO_DATA_FOUND;
                }
                s = HelperUtilities.ToJSON(Success, Message, objDataTable);
            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                Success = false;
                Message = CommonMessages.ERROR_OCCURED;
                s = HelperUtilities.ToJSON(Success, Message);
            }
            finally
            {
                objDataTable = null;
                objDataSet = null;
                objBranches = null;
            }

            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult UploadBranches()
        {
            IIIBL.Branches objBranches = null;
            String Message = String.Empty;
            String Status = String.Empty;
            DataSet objDataSet = null;
            String s = String.Empty;
            try
            {
                String FileNamePart = Guid.NewGuid().ToString().Replace("-", String.Empty);
                String FileName = FileNamePart + Path.GetExtension(Request.Files[0].FileName);
                String FilePath = String.Format("../Uploads/{0}", FileName);
                String OutputFileName = String.Format("../Downloads/{0}", FileNamePart + ".xlsx");
                String OutputFileName2 = Server.MapPath(OutputFileName);

                FilePath = Server.MapPath(FilePath);
                Request.Files[0].SaveAs(FilePath);

                objBranches = new IIIBL.Branches();
                objDataSet = objBranches.UploadBranches(PortalApplication.ConnectionString, PortalSession.UserID, FilePath, out Message);

                if (Message == String.Empty)
                {
                    String[] DisplayColumns = new String[] { "Branch Code", "Upload Remarks" };
                    String[] DisplayHeaders = new String[] { "Branch Code", "Upload Remarks" };
                    String[] DisplayFormat = new String[] { String.Empty, String.Empty };

                    XLXporter.WriteExcel(OutputFileName2, objDataSet.Tables[0], DisplayColumns, DisplayHeaders, DisplayFormat);

                    KeyValuePair<String, String>[] KVPair = new KeyValuePair<String, String>[1];
                    KVPair[0] = new KeyValuePair<string, string>("_RESPONSE_FILE_", OutputFileName);
                    s = HelperUtilities.ToJSON(true, CommonMessages.FILE_PROCESS_SUCCESS, null, KVPair);
                }
                else
                {
                    s = HelperUtilities.ToJSON(false, CommonMessages.FILE_PROCESS_FAIL + " : " + Message);
                }

                try
                {
                    System.IO.File.Delete(FilePath);
                }
                catch (Exception ex)
                {
                    Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                }
            }
            catch( Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
                s = HelperUtilities.ToJSON(false, CommonMessages.ERROR_OCCURED);
            }
            finally
            {
                objBranches = null;
            }
            
            JsonResult result = new JsonResult();
            result.Data = s;
            return result;
        }

        [HttpGet]
        [AuthorizeExt]
        public void DownloadReport()
        {
            IIIBL.Branches objBranches = null;
            String Message = String.Empty;
            String Status = String.Empty;
            string fileName = string.Empty;
            DataSet objDataSet = null;
            String s = String.Empty;
            try
            {
                objBranches = new IIIBL.Branches();
                objDataSet = objBranches.DownloadReport(PortalApplication.ConnectionString, PortalSession.UserID);
                DataTable dtBranch = objDataSet.Tables[0];

                string strTargetDir = Server.MapPath("~/Downloads/");
                DirectoryInfo oDir = new DirectoryInfo(strTargetDir);
                oDir = oDir.CreateSubdirectory(DateTime.Now.Ticks.ToString());
                fileName = "BranchData_" + DateTime.Today.ToString("ddMMMyy") + ".xlsx";
                string strPath = oDir.FullName + "\\" + "BranchData_" + DateTime.Today.ToString("ddMMMyy") + ".xlsx";

                String[] DisplayColumns = new String[] { "BranchCode", "BranchName", "Address", "City", "District", "State", "IsActive" };
                String[] DisplayHeaders = new String[] { "Branch Code", "Branch Name", "Address", "City", "District", "State",  "Is Active ?" };
                String[] DisplayFormat = new String[] { String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty };

                XLXporter.WriteExcel(strPath, dtBranch, DisplayColumns , DisplayHeaders, DisplayFormat);

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                Response.TransmitFile(strPath);

            }
            catch (Exception ex)
            {
                Errorlogger.LogError(this.GetType().Name, MethodBase.GetCurrentMethod().Name, ex);
            }
        }
    }
}