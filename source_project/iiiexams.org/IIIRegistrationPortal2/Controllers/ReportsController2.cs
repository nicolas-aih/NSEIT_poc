using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using IIIBL;
using System.Text;
using System.Reflection;
using Newtonsoft.Json;

namespace IIIRegistrationPortal2.Controllers
{
    //Only Reports Infra Related Functions Goes In This File
    public partial class ReportsController : Controller
    {
        [HttpGet]
        [AuthorizeExt]
        public ActionResult ReportsDashboard()
        {
            ViewBag.IsLoggedOn = PortalSession.UserID == 0 ? false : true;
            ViewBag.ClassName = "col-sm-9";
            return View();
        }

        [HttpPost]
        [AuthorizeAJAX]
        public JsonResult LoadReportRequests()
        {
            //loading report request data for ajax call
            Boolean _Status = true;

            IIIBL.ReportsInfra objReportsInfra = null;
            JsonResult objJsonResult = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            Boolean Success = false;
            String Message = String.Empty;
            String s = String.Empty;

            if (_Status)
            {
                try
                {
                    objReportsInfra = new IIIBL.ReportsInfra();
                    objDataSet = objReportsInfra.GetReportRequests(PortalApplication.ConnectionString, PortalSession.InsurerUserID);
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
                }
            }
            objJsonResult = new JsonResult();
            objJsonResult.Data = s;
            return objJsonResult;
        }

        [HttpGet]
        [AuthorizeExt]
        public ActionResult DownloadReport(String FileName)
        {
            String FilePath = Server.MapPath( PortalApplication.ReportsDump ) + "\\" + FileName;
            if (System.IO.File.Exists(FilePath))
            {
                String ContentType = MimeMapping.GetMimeMapping(FilePath);
                return File(FilePath, ContentType, "PaymentReceipts.zip");
            }
            else
            {
                return null;
            }
        }

        //Future function would be 
        // 1.   SaveReportsConfig :- For Auto mailers that would be needed in the system
    }

}