using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace IIIRegistrationPortal2
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // create an object of ScriptBundle and 
            // specify bundle name (as virtual path) as constructor parameter 
            //ScriptBundle scriptBundle = new ScriptBundle("~/js/stdscripts");

            ////use Include() method to add all the script files with their pathsD:\IIIExamsRegistrationPortal\IIIRegistrationPortal2 - CSS Branch\IIIRegistrationPortal2\App_Start\BundleConfig.cs 
            //scriptBundle.Include(new String[] {
            //        "~/js/RegexDef.js",
            //        "~/js/jquery-3.2.1.js",
            //        "~/js/bootstrap.js",
            //        "~/js/jquery-ui.js",
            //        "~/js/jquery.validate.js",
            //        "~/js/jquery.timepicker.js",
            //        "~/js/additional-methods.js",
            //        "~/js/app/commonmethods.js" }
            //    );

            //StyleBundle styleBundle = new StyleBundle("~/css/styles");
            //styleBundle.Include("~/css/bootstrap.css", 
            //    "~/css/common.css", 
            //    "~/CSS/jquery-ui.css");

            //Add the bundle into BundleCollection
            //bundles.Add(scriptBundle);
            //bundles.Add(styleBundle);

            /* Commented by MAS as the IIS is not refreshing on Windows 2008R2
            bundles.Add(new ScriptBundle("~/js/app/approvalrejection").Include("~/js/app/approvalrejection.js"));
            bundles.Add(new ScriptBundle("~/js/app/approvedcorporateagent").Include("~/js/app/approvedcorporateagent.js"));
            bundles.Add(new ScriptBundle("~/js/app/bookseat").Include("~/js/app/bookseat.js"));
            bundles.Add(new ScriptBundle("~/js/app/branches").Include("~/js/app/branches.js"));
            bundles.Add(new ScriptBundle("~/js/app/cb").Include("~/js/app/cb.js"));
            bundles.Add(new ScriptBundle("~/js/app/cc").Include("~/js/app/cc.js"));
            bundles.Add(new ScriptBundle("~/js/app/commonmethods").Include("~/js/app/commonmethods.js"));
            bundles.Add(new ScriptBundle("~/js/app/corporateagentexaminationreport").Include("~/js/app/corporateagentexaminationreport.js"));
            bundles.Add(new ScriptBundle("~/js/app/cwpcc").Include("~/js/app/cwpcc.js"));
            bundles.Add(new ScriptBundle("~/js/app/districts").Include("~/js/app/districts.js"));
            bundles.Add(new ScriptBundle("~/js/app/DPRange").Include("~/js/app/DPRange.js"));
            bundles.Add(new ScriptBundle("~/js/app/duplicateurn").Include("~/js/app/duplicateurn.js"));
            bundles.Add(new ScriptBundle("~/js/app/examdetails").Include("~/js/app/examdetails.js"));
            bundles.Add(new ScriptBundle("~/js/app/examdetailsb").Include("~/js/app/examdetailsb.js"));
            bundles.Add(new ScriptBundle("~/js/app/examinationreport").Include("~/js/app/examinationreport.js"));
            bundles.Add(new ScriptBundle("~/js/app/ExamRegistration").Include("~/js/app/ExamRegistration.js"));
            bundles.Add(new ScriptBundle("~/js/app/hallticket").Include("~/js/app/hallticket.js"));
            bundles.Add(new ScriptBundle("~/js/app/insurers").Include("~/js/app/insurers.js"));
            bundles.Add(new ScriptBundle("~/js/app/ledgerreport").Include("~/js/app/ledgerreport.js"));
            bundles.Add(new ScriptBundle("~/js/app/loginrequest").Include("~/js/app/loginrequest.js"));
            bundles.Add(new ScriptBundle("~/js/app/managebatches").Include("~/js/app/managebatches.js"));
            bundles.Add(new ScriptBundle("~/js/app/managebatchesB").Include("~/js/app/managebatchesB.js"));
            bundles.Add(new ScriptBundle("~/js/app/modifications").Include("~/js/app/modifications.js"));
            bundles.Add(new ScriptBundle("~/js/app/modifications2").Include("~/js/app/modifications2.js"));
            bundles.Add(new ScriptBundle("~/js/app/myprofile").Include("~/js/app/myprofile.js"));
            bundles.Add(new ScriptBundle("~/js/app/myprofile2").Include("~/js/app/myprofile2.js"));
            bundles.Add(new ScriptBundle("~/js/app/newvoucherentry").Include("~/js/app/newvoucherentry.js"));
            bundles.Add(new ScriptBundle("~/js/app/Notifications").Include("~/js/app/Notifications.js"));
            bundles.Add(new ScriptBundle("~/js/app/panlookup").Include("~/js/app/panlookup.js"));
            bundles.Add(new ScriptBundle("~/js/app/prelogin").Include("~/js/app/prelogin.js"));
            bundles.Add(new ScriptBundle("~/js/app/rc").Include("~/js/app/rc.js"));
            bundles.Add(new ScriptBundle("~/js/app/rcb").Include("~/js/app/rcb.js"));
            bundles.Add(new ScriptBundle("~/js/app/reconcilebooking").Include("~/js/app/reconcilebooking.js"));
            bundles.Add(new ScriptBundle("~/js/app/RolePermission").Include("~/js/app/RolePermission.js"));
            bundles.Add(new ScriptBundle("~/js/app/roles").Include("~/js/app/roles.js"));
            bundles.Add(new ScriptBundle("~/js/app/schedulereport").Include("~/js/app/schedulereport.js"));
            bundles.Add(new ScriptBundle("~/js/app/scorecard").Include("~/js/app/scorecard.js"));
            bundles.Add(new ScriptBundle("~/js/app/sponsorshipstatus").Include("~/js/app/sponsorshipstatus.js"));
            bundles.Add(new ScriptBundle("~/js/app/sponsorshipstatusforcorporate").Include("~/js/app/sponsorshipstatusforcorporate.js"));
            bundles.Add(new ScriptBundle("~/js/app/tbxschedule").Include("~/js/app/tbxschedule.js"));
            bundles.Add(new ScriptBundle("~/js/app/ticker").Include("~/js/app/ticker.js"));
            bundles.Add(new ScriptBundle("~/js/app/updateurnstatus").Include("~/js/app/updateurnstatus.js"));
            bundles.Add(new ScriptBundle("~/js/app/urncreation").Include("~/js/app/urncreation.js"));
            bundles.Add(new ScriptBundle("~/js/app/urncreationInsurer").Include("~/js/app/urncreationInsurer.js"));
            bundles.Add(new ScriptBundle("~/js/app/urndeletion").Include("~/js/app/urndeletion.js"));
            bundles.Add(new ScriptBundle("~/js/app/urnlookup").Include("~/js/app/urnlookup.js"));
            bundles.Add(new ScriptBundle("~/js/app/urnmodification").Include("~/js/app/urnmodification.js"));
            bundles.Add(new ScriptBundle("~/js/app/urnmodificationInsurer").Include("~/js/app/urnmodificationInsurer.js"));
            bundles.Add(new ScriptBundle("~/js/app/users").Include("~/js/app/users.js"));
            bundles.Add(new ScriptBundle("~/js/app/viewac").Include("~/js/app/viewac.js"));
            bundles.Add(new ScriptBundle("~/js/app/viewbranches").Include("~/js/app/viewbranches.js"));
            bundles.Add(new ScriptBundle("~/js/app/viewdp").Include("~/js/app/viewdp.js"));
            bundles.Add(new ScriptBundle("~/js/app/viewexamcenterNonpl").Include("~/js/app/viewexamcenterNonpl.js"));
            bundles.Add(new ScriptBundle("~/js/app/viewexamcenterpl").Include("~/js/app/viewexamcenterpl.js"));

            bundles.Add(new ScriptBundle("~/js/app/approvalrejectionurncorps").Include("~/js/app/approvalrejectionurncorps.js"));
            bundles.Add(new ScriptBundle("~/js/app/approvalrejectionurn").Include("~/js/app/approvalrejectionurn.js"));

            bundles.Add(new ScriptBundle("~/js/app/urnrequestmodification").Include("~/js/app/urnrequestmodification.js"));

            BundleTable.EnableOptimizations = true;
            */
        }
    }
}