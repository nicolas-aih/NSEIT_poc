using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using IIIDL;
using Utilities.FileParser;
using System.Text.RegularExpressions;
using System.IO;

namespace IIIBL
{
    public class URN
    {
        public DataSet GetURNForPAN(System.String ConnectionString, System.String PAN)
        {
            IIIDL.URN objURN = null;
            DataSet objDataSet = null;
            try
            {
                objURN = new IIIDL.URN();
                objDataSet = objURN.GetURNForPAN(ConnectionString, PAN);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return objDataSet;
        }

        public String UnarchiveURN ( System.String ConnectionString, System.String URN)
        {
            IIIDL.URN objURN = null;
            String Message = String.Empty;
            try
            {
                objURN = new IIIDL.URN();
                Message = objURN.UnarchiveURN(ConnectionString, URN);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return Message;
        }

        public DataSet GetURNDetails(System.String ConnectionString, System.String URN, System.Int32 UserId)
        {
            IIIDL.URN objURN = null;
            DataSet objDataSet = null;
            try
            {
                objURN = new IIIDL.URN();
                objDataSet = objURN.GetURNDetails(ConnectionString, URN, UserId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return objDataSet;
        }

        public DataSet GetPhotoAndSignature(System.String ConnectionString, System.String URN)
        {
            IIIDL.URN objURN = null;
            DataSet objDataSet = null;
            try
            {
                objURN = new IIIDL.URN();
                objDataSet = objURN.GetPhotoAndSignature(ConnectionString, URN);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return objDataSet;
        }

        public void GenerateURN(System.String ConnectionString, System.Data.DataTable ApplicantData, System.String RoleCode, System.Int32 UserId, out System.String RollNo, out System.String Message)
        {
            IIIDL.URN objURN = null;
            DataSet objDataSet = null;
            RollNo = String.Empty;
            Message = String.Empty;
            try
            {
                objURN = new IIIDL.URN();
                objURN.GenerateURN(ConnectionString, ApplicantData, RoleCode, UserId, out RollNo, out Message);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
        }

        public void GenerateDuplicateURN(System.String ConnectionString, System.Data.DataTable ApplicantData, System.String RoleCode, System.Int32 UserId, out System.String RollNo, out System.String Message)
        {
            IIIDL.URN objURN = null;
            //DataSet objDataSet = null;
            try
            {
                objURN = new IIIDL.URN();
                objURN.GenerateDuplicateURN(ConnectionString, ApplicantData, RoleCode, UserId, out RollNo, out Message);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
        }

        public DataSet GetDataForPartialModification(System.String ConnectionString, System.String URN, System.DateTime DOB, out String Message, Int32 InsurerUserId)
        {
            IIIDL.URN objURN = null;
            DataSet objDataSet = null;
            try
            {
                objURN = new IIIDL.URN();
                objDataSet = objURN.GetDataForPartialModification(ConnectionString, URN, DOB, out Message, InsurerUserId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return objDataSet;
        }

        public DataSet GetDataForModification(System.String ConnectionString, System.String URN, Int32 UserId, out String Status, out String Message)
        {
            IIIDL.URN objURN = null;
            DataSet objDataSet = null;
            try
            {
                objURN = new IIIDL.URN();
                objDataSet = objURN.GetDataForModification(ConnectionString, URN, UserId, out Status, out Message);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return objDataSet;
        }

        public DataSet GetDataForURNRequestModification(System.String ConnectionString, System.Int64 Id, Int32 UserId, out String Message)
        {
            IIIDL.URN objURN = null;
            DataSet objDataSet = null;
            try
            {
                objURN = new IIIDL.URN();
                objDataSet = objURN.GetDataForURNRequestModification(ConnectionString, Id, UserId, out Message);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return objDataSet;
        }


        public DataSet GetDataForURNDuplication(System.String ConnectionString, System.String URN, Int32 UserId, out String Status, out String Message)
        {
            IIIDL.URN objURN = null;
            DataSet objDataSet = null;
            try
            {
                objURN = new IIIDL.URN();
                objDataSet = objURN.GetDataForURNDuplication(ConnectionString, URN, UserId, out Status, out Message);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return objDataSet;
        }

        public void SaveModification(System.String ConnectionString,  System.String URN, System.Int32 Languageid, System.Int32 Examcenterid, Byte[] Photo, Byte[] Sign, System.String AllowWhatsappMessage, System.String WhatsappNumber, out System.String Status, out System.String Message,
            Int32 UserId, Int32 InsurerUserId)
        {
            IIIDL.URN objURN = null;
            try
            {
                objURN = new IIIDL.URN();
                objURN.SaveModification(ConnectionString, URN, Languageid, Examcenterid, Photo, Sign,AllowWhatsappMessage, WhatsappNumber, out Status, out Message, UserId, InsurerUserId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
        }

        public void SaveModificationOAIMS(System.String ConnectionString, String OracleConnectionString, System.String URN, System.Int32 Languageid, System.Int32 Examcenterid, Byte[] Photo, Byte[] Sign,System.String AllowWhatsappMessage, System.String WhatsappNumber, out System.String Status, out System.String Message,
            String BasePathP, String BasePathS, String ShareUser, String SharePass, Int32 UserId, Int32 InsurerUserId, out Exception exOut)
        {
            IIIDL.URN objURN = null;
            try
            {
                objURN = new IIIDL.URN();
                objURN.SaveModificationOAIMS(ConnectionString, OracleConnectionString, URN, Languageid, Examcenterid, Photo, Sign, AllowWhatsappMessage, WhatsappNumber,out Status, out Message, BasePathP, BasePathS, ShareUser, SharePass, UserId, InsurerUserId, out exOut);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
        }

        public String UpdateURNDetails(System.String ConnectionString, System.Data.DataTable ApplicantData, System.Int32 UserId)
        {
            IIIDL.URN objURN = null;
            String Message = String.Empty;
            try
            {
                objURN = new IIIDL.URN();
                Message = objURN.UpdateURNDetails(ConnectionString, ApplicantData, UserId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return Message;
        }

        public String /*Message*/ UpdateURNRequest(System.String ConnectionString, Int64 Id, System.Data.DataTable ApplicantData, System.String RoleCode, System.Int32 UserId, out String URN)
        {
            IIIDL.URN objURN = null;
            String Message = String.Empty;
            URN = String.Empty;
            try
            {
                objURN = new IIIDL.URN();
                Message = objURN.UpdateURNRequest(ConnectionString, Id, ApplicantData, RoleCode, UserId,out URN);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return Message;
        }

        //Validation functions
        public Boolean ValidateInternalRefNo(System.String ConnectionString, System.String InternalRefNo, System.Int32 InsurerId, System.Int64 ApplicantId = 0)
        {
            IIIDL.URN objURN = null;
            Boolean RetVal = false;
            try
            {
                objURN = new IIIDL.URN();
                RetVal = objURN.ValidateInternalRefNo(ConnectionString, InternalRefNo, InsurerId, ApplicantId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return RetVal;
        }

        public Boolean ValidateInternalRefNoApp(System.String ConnectionString, System.String InternalRefNo, System.Int32 InsurerId, System.Int64 ApplicantDataId)
        {
            IIIDL.URN objURN = null;
            Boolean RetVal = false;
            try
            {
                objURN = new IIIDL.URN();
                RetVal = objURN.ValidateInternalRefNoApp(ConnectionString, InternalRefNo, InsurerId, ApplicantDataId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return RetVal;
        }


        public String ValidatePAN(System.String ConnectionString, System.String PAN)
        {
            IIIDL.URN objURN = null;
            String RetVal = String.Empty;
            try
            {
                objURN = new IIIDL.URN();
                RetVal = objURN.ValidatePAN(ConnectionString, PAN);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return RetVal;
        }

        public String ValidatePAN(System.String ConnectionString, System.String PAN, Int64 ApplicantId, Int32 InsurerUserId)
        {
            IIIDL.URN objURN = null;
            String RetVal = String.Empty;
            try
            {
                objURN = new IIIDL.URN();
                RetVal = objURN.ValidatePAN(ConnectionString, PAN, ApplicantId, InsurerUserId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return RetVal;
        }

        public String ValidateAadhaarCorporates(System.String ConnectionString, System.String AadhaarNo, System.String PAN, System.String URN)
        {
            IIIDL.URN objURN = null;
            String RetVal = String.Empty;
            try
            {
                objURN = new IIIDL.URN();
                RetVal = objURN.ValidateAadhaarCorporates(ConnectionString, AadhaarNo, PAN, URN );
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return RetVal;
        }

        public String ValidateAadhaarCorporatesApp(System.String ConnectionString, System.String AadhaarNo, System.String PAN, System.Int64 ApplicantDataId)
        {
            IIIDL.URN objURN = null;
            String RetVal = String.Empty;
            try
            {
                objURN = new IIIDL.URN();
                RetVal = objURN.ValidateAadhaarCorporatesApp(ConnectionString, AadhaarNo, PAN, ApplicantDataId);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return RetVal;
        }

        public String ValidateEmailCorporates(System.String ConnectionString, System.String EmailId, System.Int64 Applicantid, System.String PAN)
        {
            IIIDL.URN objURN = null;
            String RetVal = String.Empty;
            try
            {
                objURN = new IIIDL.URN();
                RetVal = objURN.ValidateEmailCorporates(ConnectionString, EmailId, Applicantid, PAN);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return RetVal;
        }

        public String ValidateEmailCorporatesApp(System.String ConnectionString, System.String EmailId, System.Int64 ApplicantDataId, System.String PAN)
        {
            IIIDL.URN objURN = null;
            String RetVal = String.Empty;
            try
            {
                objURN = new IIIDL.URN();
                RetVal = objURN.ValidateEmailCorporatesApp(ConnectionString, EmailId, ApplicantDataId, PAN);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return RetVal;
        }

        public String ValidateMobileCorporates(System.String ConnectionString, System.String MobileNo, System.Int64 Applicantid, System.String PAN)
        {
            IIIDL.URN objURN = null;
            String RetVal = String.Empty;
            try
            {
                objURN = new IIIDL.URN();
                RetVal = objURN.ValidateMobileCorporates(ConnectionString, MobileNo, Applicantid, PAN);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return RetVal;
        }

        public String ValidateWhatsAppCorporates(System.String ConnectionString, System.String MobileNo, System.Int64 Applicantid, System.String PAN)
        {
            IIIDL.URN objURN = null;
            String RetVal = String.Empty;
            try
            {
                objURN = new IIIDL.URN();
                RetVal = objURN.ValidateWhatsAppCorporates(ConnectionString, MobileNo, Applicantid, PAN);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return RetVal;
        }
        public String ValidateMobileCorporatesApp(System.String ConnectionString, System.String MobileNo, System.Int64 ApplicantDataId, System.String PAN)
        {
            IIIDL.URN objURN = null;
            String RetVal = String.Empty;
            try
            {
                objURN = new IIIDL.URN();
                RetVal = objURN.ValidateMobileCorporatesApp(ConnectionString, MobileNo, ApplicantDataId, PAN);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return RetVal;
        }

        public String ValidateWhatsAppCorporatesApp(System.String ConnectionString, System.String MobileNo, System.Int64 ApplicantDataId, System.String PAN)
        {
            IIIDL.URN objURN = null;
            String RetVal = String.Empty;
            try
            {
                objURN = new IIIDL.URN();
                RetVal = objURN.ValidateWhatsAppCorporatesApp(ConnectionString, MobileNo, ApplicantDataId, PAN);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return RetVal;
        }


        public String ValidateWhatsAppCorporatesForMod(System.String ConnectionString, System.String URN, System.String MobileNo)
        {
            IIIDL.URN objURN = null;
            String RetVal = String.Empty;
            try
            {
                objURN = new IIIDL.URN();
                RetVal = objURN.ValidateWhatsAppCorporatesForMod(ConnectionString,URN, MobileNo);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return RetVal;
        }

        public DataSet UploadDuplicateURN(System.String ConnectionString, System.Int32 UserId, String RoleCode , String FileNameAndPath, out String Message)
        {
            IIIDL.URN objURN = null;
            DataSet objDataSet = null;
            ExcelParser excelParser = null;
            DataTable ExcelOutput = null;
            try
            {
                try
                {
                    excelParser = new ExcelParser();
                    ExcelOutput = excelParser.ParseAllAsData(FileNameAndPath, "Sheet1$", true);
                }
                catch (Exception ex)
                {
                    Message = "Error occured while reading the file";
                    return objDataSet;
                }

                if  (ExcelOutput.Rows.Count == 0)
                {
                    Message = "File has no record";
                    return objDataSet;
                }
                if (!(ExcelOutput.Columns.Count == 1 && ExcelOutput.Columns.Contains("URN")))
                { 
                    Message = "Invalid file format. Please download the template and try uploading again.";
                    return objDataSet;
                }

                //ExcelOutput.Columns.Add("NewURN", typeof(String));
                ExcelOutput.Columns.Add("is_valid", typeof(Int32));
                ExcelOutput.Columns.Add("remarks", typeof(String));

                Int32 InvalidCount = 0;
                foreach (DataRow dr in ExcelOutput.Rows)
                {
                    String Error = String.Empty;
                    System.Boolean IsValid = true;
                    String URN = Convert.ToString(dr["URN"]);
                    if (!Regex.IsMatch(URN, Common.regexURN))
                    {
                        Error += "Invalid URN. URN must be alphanumeric without any spaces or special characters";
                        IsValid = false;
                        InvalidCount++;
                    }
                    dr["remarks"] = Error;
                    dr["is_valid"] = IsValid ? 1 : 0;
                }
                if (InvalidCount == ExcelOutput.Rows.Count)
                {
                    ExcelOutput.Columns.Remove("is_valid");
                    objDataSet = new DataSet();
                    objDataSet.Tables.Add(ExcelOutput);
                    Message = "Unable to proceed. Kindly rectify the errors and try uploading again";
                    return objDataSet;
                }

                objURN = new IIIDL.URN();
                objDataSet = objURN.UploadDuplicateURN(ConnectionString, ExcelOutput, UserId, RoleCode);
                Message = String.Empty; // As of now.
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objURN = null;
            }
            return objDataSet;
        }

        public DataSet UploadURN(System.String ConnectionString, String OriginalFileName, System.Int32 UserId, String RoleCode, String InsuranceType, String DirectoryNameAndPath, out String Message, Byte[] Key, Byte[] IV)
        {
            
            String[] FieldsCA  = new String[] { "CoR Type", "Insurance Category", "Sponsorship Date", "Name Initial", "Candidate/Corporate Name", "Father Name", "Category", "Current House Number", "Current Street", "Current Town", "Current District", "Current State", "Current Pincode", "Permanent House Number", "Permanent Street", "Permanent Town", "Permanent District", "Permanent State", "Permanent Pincode", "Area", "Basic Qualification", "Board Name For Basic Qualification", "Roll Number For Basic Qualification", "Year of Passing For Basic Qualification", "Educational Qualification", "Date of Birth", "Sex", "Primary Profession", "Landline No", "Mobile No", "Branch Name", "Exam Mode", "Exam Body Name", "Exam Center Location", "Exam Language", "Email ID", "Contact Person Email ID", "PAN",  "Employee No", "Voter ID Card", "Driving License No", "Passport No", "Central Govt ID Card", "Nationality", "Photo File Name", "Signature File Name", "Allow WhatsApp Messages", "WhatsApp Number", "Telemarketer TRAI reg no" };
            String[] FieldsWA  = new String[] { "CoR Type", "Insurance Category", "Sponsorship Date", "Name Initial", "Candidate/Corporate Name", "Father Name", "Category", "Current House Number", "Current Street", "Current Town", "Current District", "Current State", "Current Pincode", "Permanent House Number", "Permanent Street", "Permanent Town", "Permanent District", "Permanent State", "Permanent Pincode", "Area", "Basic Qualification", "Board Name For Basic Qualification", "Roll Number For Basic Qualification", "Year of Passing For Basic Qualification", "Educational Qualification", "Date of Birth", "Sex", "Primary Profession", "Landline No", "Mobile No", "Branch Name", "Exam Mode", "Exam Body Name", "Exam Center Location", "Exam Language", "Email ID", "Contact Person Email ID", "PAN",  "Employee No", "Voter ID Card", "Driving License No", "Passport No", "Central Govt ID Card", "Nationality", "Photo File Name", "Signature File Name", "Allow WhatsApp Messages", "WhatsApp Number", "Telemarketer TRAI reg no" };
            String[] FieldsBR  = new String[] { "CoR Type", "Insurance Category", "Sponsorship Date", "Name Initial", "Candidate/Corporate Name", "Father Name", "Category", "Current House Number", "Current Street", "Current Town", "Current District", "Current State", "Current Pincode", "Permanent House Number", "Permanent Street", "Permanent Town", "Permanent District", "Permanent State", "Permanent Pincode", "Area", "Basic Qualification", "Board Name For Basic Qualification", "Roll Number For Basic Qualification", "Year of Passing For Basic Qualification", "Educational Qualification", "Date of Birth", "Sex", "Primary Profession", "Landline No", "Mobile No", "Branch Name", "Exam Mode", "Exam Body Name", "Exam Center Location", "Exam Language", "Email ID", "Contact Person Email ID", "PAN", "Employee No", "Voter ID Card", "Driving License No", "Passport No", "Central Govt ID Card", "Nationality", "Photo File Name", "Signature File Name", "Allow WhatsApp Messages", "WhatsApp Number", "Telemarketer TRAI reg no" };
            String[] FieldsIMF = new String[] { "CoR Type", "Insurance Category", "Sponsorship Date", "Name Initial", "Candidate/Corporate Name", "Father Name", "Category", "Current House Number", "Current Street", "Current Town", "Current District", "Current State", "Current Pincode", "Permanent House Number", "Permanent Street", "Permanent Town", "Permanent District", "Permanent State", "Permanent Pincode", "Area", "Basic Qualification", "Board Name For Basic Qualification", "Roll Number For Basic Qualification", "Year of Passing For Basic Qualification", "Educational Qualification", "Date of Birth", "Sex", "Primary Profession", "Landline No", "Mobile No", "Branch Name", "Exam Mode", "Exam Body Name", "Exam Center Location", "Exam Language", "Email ID", "Contact Person Email ID", "PAN", "Employee No", "Voter ID Card", "Driving License No", "Passport No", "Central Govt ID Card", "Nationality", "Photo File Name", "Signature File Name", "Allow WhatsApp Messages", "WhatsApp Number" };
            String[] FieldsI = new String[] { "Sponsorship Date","Name Initial","Candidate Name","Father Name","Category","Current House Number","Current Street","Current Town","Current District","Current State","Current Pincode","Permanent House Number","Permanent Street","Permanent Town","Permanent District","Permanent State","Permanent Pincode","Area","Basic Qualification","Board Name For Basic Qualification","Roll Number For Basic Qualification","Year of Passing For Basic Qualification","Educational Qualification","Other Qualification","Date of Birth","Sex","Phone No","Mobile No","Exam Mode","Exam Body Name","Exam Center Location","Exam Language","Email ID","PAN","Insurer Ref No","Voter ID Card","Driving License No","Passport No","Central Govt ID Card","Primary Profession","Nationality","Photo File Name","Signature File Name", "Allow WhatsApp Messages", "WhatsApp Number", "Candidate Type", "Telemarketer TRAI reg no", "Contact Person Email ID" };

            Message = String.Empty;
            IIIDL.URN objURN = null;
            DataSet objDataSet = null;
            ExcelParser excelParser = null;
            DataTable ExcelOutput = null;
            String PhotoPath = DirectoryNameAndPath + "\\Photo";
            String SignaturePath = DirectoryNameAndPath + "\\Signature";
            String ExcelPath = String.Empty;
            try
            {
                DirectoryInfo di = new DirectoryInfo(DirectoryNameAndPath);
                if (di.Exists)
                {
                    //DirectoryInfo diPhotos = new DirectoryInfo(PhotoPath);
                    //DirectoryInfo diSign = new DirectoryInfo(SignaturePath);
                    FileInfo[]fi = di.GetFiles("*.xls*", SearchOption.AllDirectories);
                    if ( fi.Length != 1 /*|| !diPhotos.Exists || !diSign.Exists*/)
                    {
                        //Invalid zip file 
                        Message = "Invalid zip file template. Zip uploaded is not as per the template";
                        return objDataSet;
                    }
                    else
                    {
                        excelParser = new ExcelParser();
                        ExcelOutput = excelParser.ParseAllAsData(fi[0].FullName, "Sheet1$", true);

                        if (ExcelOutput.Rows.Count == 0)
                        {
                            Message = "File has no record";
                            return objDataSet;
                        }
                        String[] Fields = null;
                        switch (RoleCode)
                        {
                            case "CA":
                                Fields = FieldsCA;
                                break;
                            case "IMF":
                                Fields = FieldsIMF;
                                break;
                            case "BR":
                                Fields = FieldsBR;
                                break;
                            case "WA":
                                Fields = FieldsWA;
                                break;
                            case "I":
                                Fields = FieldsI;
                                break;
                            default:
                                throw new Exception("Invalid Role Code");
                                break;
                        }
                        if (ExcelOutput.Columns.Contains("Aadhaar No")) //Aadhaar
                        {
                            ExcelOutput.Columns.Remove("Aadhaar No"); //Aadhaar
                        }
                        Boolean True1 = ExcelOutput.Columns.Contains("Allow WhatsApp Messages");
                        Boolean True2 = ExcelOutput.Columns.Contains("WhatsApp Number");
                        if (True1 && True2) //Clean
                        {
                            //Both the fields are existing.//New Format
                            //No processing needed.
                        }
                        if (!True1 && !True2)
                        {
                            //None of the field is there //Old format
                            //Add both the fields and set the value to permission value to N
                            ExcelOutput.Columns.Add("Allow WhatsApp Messages", typeof(String));
                            ExcelOutput.Columns.Add("WhatsApp Number", typeof(String));
                            foreach(DataRow dr in ExcelOutput.Rows)
                            {
                                dr["Allow WhatsApp Messages"] = "N";
                                dr["WhatsApp Number"] = DBNull.Value;
                            }
                        }
                        else
                        {
                            //Maybe one field is not around.
                            //This will be caught in the next loop.
                            //Hence no processing needed here
                        }

                        if(RoleCode == "I")
                        {
                            if (!ExcelOutput.Columns.Contains("Candidate Type"))
                            {
                                ExcelOutput.Columns.Add("Candidate Type", typeof(String));
                                foreach (DataRow dr in ExcelOutput.Rows)
                                {
                                    dr["Candidate Type"] = "Insurance Agent";
                                }
                            }
                        }


                        if (!ExcelOutput.Columns.Contains("Telemarketer TRAI reg no"))
                        {
                            ExcelOutput.Columns.Add("Telemarketer TRAI reg no", typeof(String));
                            foreach (DataRow dr in ExcelOutput.Rows)
                            {
                                dr["Telemarketer TRAI reg no"] = DBNull.Value;
                            }
                        }

                        if (RoleCode =="I")
                        {
                            if (!ExcelOutput.Columns.Contains("Contact Person Email ID"))
                            {
                                ExcelOutput.Columns.Add("Contact Person Email ID", typeof(String));
                                foreach (DataRow dr in ExcelOutput.Rows)
                                {
                                    dr["Contact Person Email ID"] = DBNull.Value;
                                }
                            }
                        }

                        String s = String.Empty;
                        foreach ( String fld in Fields)
                        {
                            if (! ExcelOutput.Columns.Contains(fld))
                            {
                                s += fld;
                                s += ",";
                            }
                        }
                        s = s.Trim(',');
                        if (s != String.Empty)
                        {
                            Message = String.Format( "Invalid file format. The required fields : {0} not found.", s) ;
                        }
                        else
                        {
                            if (ExcelOutput.Columns.Contains("_chrLicIndOrCorporate")) { ExcelOutput.Columns.Remove("_chrLicIndOrCorporate"); }
                            if (ExcelOutput.Columns.Contains("_bntApplicantID")) { ExcelOutput.Columns.Remove("_bntApplicantID"); }
                            if (ExcelOutput.Columns.Contains("_sntExamCenterID")) { ExcelOutput.Columns.Remove("_sntExamCenterID"); }
                            if (ExcelOutput.Columns.Contains("_bntID")) { ExcelOutput.Columns.Remove("_bntID"); }
                            if (ExcelOutput.Columns.Contains("_chrRollNumber")) { ExcelOutput.Columns.Remove("_chrRollNumber"); }
                            if (ExcelOutput.Columns.Contains("applicantPhoto")) { ExcelOutput.Columns.Remove("applicantPhoto"); }
                            if (ExcelOutput.Columns.Contains("applicantSignature")) { ExcelOutput.Columns.Remove("applicantSignature"); }

                            if (ExcelOutput.Columns.Contains("Allow WhatsApp Messages 2")) { ExcelOutput.Columns.Remove("Allow WhatsApp Messages 2"); }
                            if (ExcelOutput.Columns.Contains("WhatsApp Number 2")) { ExcelOutput.Columns.Remove("WhatsApp Number 2"); }
                            if (ExcelOutput.Columns.Contains("Candidate Type 2")) { ExcelOutput.Columns.Remove("Candidate Type 2"); }
                            if (ExcelOutput.Columns.Contains("Telemarketer TRAI reg no 2")) { ExcelOutput.Columns.Remove("Telemarketer TRAI reg no 2"); }
                            if (ExcelOutput.Columns.Contains("Contact Person Email ID 2")) { ExcelOutput.Columns.Remove("Contact Person Email ID 2"); }

                            if (ExcelOutput.Columns.Contains("UploadRemark")) { ExcelOutput.Columns.Remove("UploadRemark"); }
                            if (ExcelOutput.Columns.Contains("IsValidRecord")) { ExcelOutput.Columns.Remove("IsValidRecord"); }
                            if (ExcelOutput.Columns.Contains("rownum")) { ExcelOutput.Columns.Remove("rownum"); }

                            ExcelOutput.Columns.Add("_chrLicIndOrCorporate", typeof(String));
                            ExcelOutput.Columns.Add("_bntApplicantID", typeof(Byte[]));
                            ExcelOutput.Columns.Add("_sntExamCenterID", typeof(Byte[]));
                            ExcelOutput.Columns.Add("_bntID", typeof(Byte[]));
                            ExcelOutput.Columns.Add("_chrRollNumber", typeof(Byte[]));
                            ExcelOutput.Columns.Add("applicantPhoto", typeof(Byte[]));
                            ExcelOutput.Columns.Add("applicantSignature", typeof(Byte[]));

                            ExcelOutput.Columns.Add("Allow WhatsApp Messages 2", typeof(String));
                            ExcelOutput.Columns.Add("WhatsApp Number 2", typeof(String));
                            if (RoleCode == "I")
                            {
                                ExcelOutput.Columns.Add("Candidate Type 2", typeof(String));
                            }
                            ExcelOutput.Columns.Add("Telemarketer TRAI reg no 2", typeof(String));
                            if (RoleCode == "I")
                            {
                                ExcelOutput.Columns.Add("Contact Person Email ID 2", typeof(String));
                            }

                            ExcelOutput.Columns.Add("UploadRemark", typeof(String));
                            ExcelOutput.Columns.Add("IsValidRecord", Type.GetType("System.Boolean"));
                            ExcelOutput.Columns.Add("rownum", typeof(Int32));

                            String regexStdDate = TextValidator.GetPattern(typeof(DateTime), "dd-MMM-yyyy");
                            String regexMonthYear = TextValidator.GetPattern(typeof(DateTime), "MMM yyyy");

                            Int32 InvalidCount = 0;
                            Int32 i = 1;
                            Regex regex = new Regex(Common.regexLowAscii);
                            Int32 RowCount = 1;
                            foreach (DataRow dr in ExcelOutput.Rows)
                            {//Validate data for prohibited characters only...
                                dr["rownum"] = RowCount;
                                RowCount++;
                                Boolean AllColumnsAreEmpty = true;

                                String Error = String.Empty;
                                foreach (DataColumn c in ExcelOutput.Columns)
                                {
                                    if (Fields.Contains(c.ColumnName))
                                    {
                                        s = Convert.ToString(dr[c.ColumnName]).Trim();
                                        if (s != String.Empty)
                                        {
                                            AllColumnsAreEmpty = false;
                                            if (!regex.IsMatch(s))
                                            {
                                                Error += c.ColumnName + ",";
                                            }
                                        }
                                        //dr[c.ColumnName] = s;
                                    }
                                }
                                Error = Error.Trim(',').Trim();
                                if (AllColumnsAreEmpty)
                                {
                                    dr["IsValidRecord"] = false;
                                    dr["UploadRemark"] = "The row is empty";
                                    InvalidCount++;
                                }
                                else if (!AllColumnsAreEmpty && Error != String.Empty)
                                {
                                    Error = "Invalid charachters found in column: " + Error;
                                    dr["IsValidRecord"] = false;
                                    dr["UploadRemark"] = Error;
                                    InvalidCount++;
                                }
                                else
                                {
                                    Error = String.Empty;

                                    String PhotoFileName = Convert.ToString(dr["Photo File Name"]).Trim();
                                    String SignFileName = Convert.ToString(dr["Signature File Name"]).Trim();
                                    String DOB = Convert.ToString(dr["Date of Birth"]).Trim();
                                    String ApplicantDate = Convert.ToString(dr["Sponsorship Date"]).Trim();
                                    String DateofPassing = Convert.ToString(dr["Year of Passing For Basic Qualification"]).Trim();
                                    String allowwhatsapp_message = Convert.ToString(dr["Allow WhatsApp Messages"]).Trim();
                                    String EmailID = Convert.ToString(dr["Email ID"]);
                                    String ContactPersonsEmailId = Convert.ToString(dr["Contact Person Email ID"]);

                                    if (PhotoFileName == String.Empty)
                                    {
                                        Error += " Photo file name is required";
                                    }
                                    else if (!(PhotoFileName.ToLower().EndsWith(".jpg") || PhotoFileName.ToLower().EndsWith(".jpeg")))
                                    {
                                        Error += " Photo file should be jpg / jpeg. Other formats are not supported";
                                    }
                                    else if (PhotoFileName.ToLower() == SignFileName.ToLower())
                                    {
                                        Error += " Photo and Signature file name cannot be the same";
                                    }
                                    else
                                    {
                                        FileInfo[] FileInfoPhoto = di.GetFiles(PhotoFileName, SearchOption.AllDirectories);
                                        if (FileInfoPhoto.Length == 1)
                                        {
                                            if (FileInfoPhoto[0].Length > 51200)
                                            {
                                                Error += " Photo File size should be less that 50 kilobytes";
                                            }
                                            else
                                            {
                                                dr["applicantPhoto"] = File.ReadAllBytes(FileInfoPhoto[0].FullName);
                                            }
                                        }
                                        else if (FileInfoPhoto.Length > 1)
                                        {
                                            Error += " Multiple photo files found for the candidate.";
                                        }
                                        else if (FileInfoPhoto.Length == 0)
                                        {
                                            Error += " Photo file not found.";
                                        }
                                    }

                                    if (SignFileName == String.Empty)
                                    {
                                        Error += " Signature file name is required";
                                    }
                                    else if (!(SignFileName.ToLower().EndsWith(".jpg") || SignFileName.ToLower().EndsWith(".jpeg")))
                                    {
                                        Error += " Signature should be jpg / jpeg. Other formats are not supported";
                                    }
                                    else
                                    {
                                        FileInfo[] FileInfoSign = di.GetFiles(SignFileName, SearchOption.AllDirectories);
                                        if (FileInfoSign.Length == 1)
                                        {
                                            if (FileInfoSign[0].Length > 51200)
                                            {
                                                Error += " Signature File size should be less that 50 kilobytes";
                                            }
                                            else
                                            {
                                                dr["applicantSignature"] = File.ReadAllBytes(FileInfoSign[0].FullName);
                                            }
                                        }
                                        else if (FileInfoSign.Length > 1)
                                        {
                                            Error += " Multiple signature files found for the candidate.";
                                        }
                                        else if (FileInfoSign.Length == 0)
                                        {
                                            Error += " Signature file not found.";
                                        }
                                    }
                                    if (DOB == String.Empty)
                                    {
                                        Error += " [Date of Birth] is required field.";
                                    }
                                    else
                                    {
                                        if (!TextValidator.ValidateDate2(DOB, "dd-MMM-yyyy", regexStdDate))
                                        {
                                            Error += " Invalid [Date of Birth].";
                                        }
                                    }
                                    if (ApplicantDate == String.Empty)
                                    {
                                        Error += " [Sponsorship Date] is required field.";
                                    }
                                    else
                                    {
                                        if (!TextValidator.ValidateDate2(ApplicantDate, "dd-MMM-yyyy", regexStdDate))
                                        {
                                            Error += " Invalid [Sponsorship Date]";
                                        }
                                    }
                                    if (DateofPassing == String.Empty)
                                    {
                                        Error += " [Year of Passing For Basic Qualification] is required field.";
                                    }
                                    else
                                    {
                                        if (!TextValidator.ValidateDate2(DateofPassing, "MMM yyyy", regexMonthYear))
                                        {
                                            Error += " Invalid [Year of Passing For Basic Qualification]";
                                        }
                                    }

                                    if (allowwhatsapp_message.Trim() == "")
                                    {
                                        Error += " [Allow WhatsApp Messages] is required field.";
                                    }
                                    else if (allowwhatsapp_message.ToUpper() != "Y" && allowwhatsapp_message.ToUpper() != "N")
                                    {
                                        Error += " Invalid [Allow WhatsApp Messages], select Y for Yes and N for no.";
                                    }
                                    else
                                    {
                                        allowwhatsapp_message = allowwhatsapp_message.Trim().ToUpper();
                                    }

                                    if (EmailID != String.Empty && !Regex.IsMatch(EmailID, Common.regexEmail) )
                                    {
                                        Error += "Invalid [Email ID]";
                                    }
                                    if (ContactPersonsEmailId != String.Empty && !Regex.IsMatch(ContactPersonsEmailId, Common.regexEmail))
                                    {
                                        Error += "Invalid [Contact Person Email ID]";
                                    }

                                    //if dr["Candidate Type"]

                                    if (Error.Trim() != String.Empty)
                                    {
                                        dr["IsValidRecord"] = false;
                                        dr["UploadRemark"] = Error.Trim();
                                        InvalidCount++;
                                    }
                                }

                                String strdt = Convert.ToString(dr["Year of Passing For Basic Qualification"]);
                                if (strdt != String.Empty)
                                {
                                    DateTime dt;
                                    if (DateTime.TryParse(strdt, out dt))
                                    {
                                        dr["Year of Passing For Basic Qualification"] = dt.ToString("MMM yyyy");
                                    }
                                }

                                dr["_chrLicIndOrCorporate"] = "I";
                                dr["_bntID"] = i;


                                dr["Allow WhatsApp Messages 2"] = dr["Allow WhatsApp Messages"];
                                dr["WhatsApp Number 2"] = dr["WhatsApp Number"];
                                dr["Telemarketer TRAI reg no 2"] = dr["Telemarketer TRAI reg no"];
                                
                                if (RoleCode == "I")
                                {
                                    String CandidateType = Convert.ToString(dr["Candidate Type"]).Trim();
                                    if (CandidateType == String.Empty )
                                    {
                                        Error += " [Candidate Type] is required field";
                                    }
                                    else if (CandidateType != "Insurance Agent" && CandidateType != "Authorised Verifier")
                                    {
                                        Error += " Invalid [Candidate Type]";
                                    }
                                    else
                                    {
                                        dr["Candidate Type 2"] = dr["Candidate Type"];
                                    }
                                    dr["Contact Person Email ID 2"] = dr["Contact Person Email ID"];
                                }
                                i++;
                            }                                                        

                            if (InvalidCount == ExcelOutput.Rows.Count)
                            {
                                if (ExcelOutput.Columns.Contains("_chrLicIndOrCorporate")) { ExcelOutput.Columns.Remove("_chrLicIndOrCorporate"); }
                                if (ExcelOutput.Columns.Contains("_bntApplicantID")) { ExcelOutput.Columns.Remove("_bntApplicantID"); }
                                if (ExcelOutput.Columns.Contains("_sntExamCenterID")) { ExcelOutput.Columns.Remove("_sntExamCenterID"); }
                                if (ExcelOutput.Columns.Contains("_bntID")) { ExcelOutput.Columns.Remove("_bntID"); }
                                if (ExcelOutput.Columns.Contains("_chrRollNumber")) { ExcelOutput.Columns.Remove("_chrRollNumber"); }
                                if (ExcelOutput.Columns.Contains("applicantPhoto")) { ExcelOutput.Columns.Remove("applicantPhoto"); }
                                if (ExcelOutput.Columns.Contains("applicantSignature")) { ExcelOutput.Columns.Remove("applicantSignature"); }

                                if (ExcelOutput.Columns.Contains("Allow WhatsApp Messages 2")) { ExcelOutput.Columns.Remove("Allow WhatsApp Messages 2"); }
                                if (ExcelOutput.Columns.Contains("WhatsApp Number 2")) { ExcelOutput.Columns.Remove("WhatsApp Number 2"); }
                                if (ExcelOutput.Columns.Contains("Candidate Type 2")) { ExcelOutput.Columns.Remove("Candidate Type 2"); }
                                if (ExcelOutput.Columns.Contains("Telemarketer TRAI reg no 2")) { ExcelOutput.Columns.Remove("Telemarketer TRAI reg no 2"); }
                                if (ExcelOutput.Columns.Contains("Contact Person Email ID 2")) { ExcelOutput.Columns.Remove("Contact Person Email ID 2"); }

                                if (ExcelOutput.Columns.Contains("IsValidRecord")) { ExcelOutput.Columns.Remove("IsValidRecord"); }                                
                                if (ExcelOutput.Columns.Contains("rownum")) { ExcelOutput.Columns.Remove("rownum"); }
                                if (ExcelOutput.Columns.Contains("IRDA URN")) { ExcelOutput.Columns.Remove("IRDA URN"); }
                                ExcelOutput.Columns.Add("IRDA URN", typeof(String)).SetOrdinal(0);
                                objDataSet = new DataSet();
                                objDataSet.Tables.Add(ExcelOutput);

                                Message = "Unable to proceed. Kindly rectify the errors and try uploading again";
                                return objDataSet;
                            }
                            else
                            {
                                ExcelOutput.Columns.Remove("Allow WhatsApp Messages");
                                ExcelOutput.Columns.Remove("WhatsApp Number");
                                if (RoleCode == "I")
                                {
                                    ExcelOutput.Columns.Remove("Candidate Type");
                                    ExcelOutput.Columns.Remove("Contact Person Email ID");
                                }
                                ExcelOutput.Columns.Remove("Telemarketer TRAI reg no");

                                ExcelOutput.Columns["Allow WhatsApp Messages 2"].ColumnName = "Allow WhatsApp Messages";
                                ExcelOutput.Columns["WhatsApp Number 2"].ColumnName = "WhatsApp Number";
                                if (RoleCode == "I")
                                {
                                    ExcelOutput.Columns["Candidate Type 2"].ColumnName = "Candidate Type";
                                    ExcelOutput.Columns["Contact Person Email ID 2"].ColumnName = "Contact Person Email ID";
                                }
                                ExcelOutput.Columns["Telemarketer TRAI reg no 2"].ColumnName = "Telemarketer TRAI reg no";

                                objURN = new IIIDL.URN();
                                if (RoleCode == "I")
                                {
                                    objDataSet = objURN.UploadSponsorshipFileAgents(ConnectionString, ExcelOutput, UserId, InsuranceType, OriginalFileName, out Message);
                                }
                                else
                                {
                                    objDataSet = objURN.UploadSponsorshipFileCorporates(ConnectionString, ExcelOutput, UserId, RoleCode, OriginalFileName, out Message);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Message = "Error occured while reading the file : Unable to read unzipped data";
                    return objDataSet;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objURN = null;
            }
            return objDataSet;
        }

        public DataSet GetURNDetailForEE(System.String ConnectionString, System.String URN)
        {
            IIIDL.URN objURN = null;
            DataSet objDataSet = null;
            try
            {
                objURN = new IIIDL.URN();
                objDataSet = objURN.GetURNDetailForEE(ConnectionString, URN);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return objDataSet;
        }

        public String SaveExamDetails(System.String ConnectionString, System.Int32 Hint, System.String URN, System.String ExamDate, System.String ExamineeId, System.Int32 Marks, System.Int32 Result, System.Int32 CurrentUser)
        {
            IIIDL.URN objURN = null;
            String s = String.Empty;
            try
            {
                objURN = new IIIDL.URN();
                s = objURN.SaveExamDetails(ConnectionString, Hint, URN, ExamDate, ExamineeId, Marks, Result, CurrentUser);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return s;
        }

        public String UpdateUrnStatus(System.String ConnectionString, System.String URN, System.String ChangedStatus, System.String UserName, System.String UserId, System.String UserMachineIP)
        {
            IIIDL.URN objURN = null;
            String RetVal = String.Empty;
            try
            {
                objURN = new IIIDL.URN();
                RetVal = objURN.UpdateUrnStatus(ConnectionString, URN, ChangedStatus, UserName, UserId, UserMachineIP);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return RetVal;
        }

        public void UpdatePaymentStatus(System.String ConnectionString, System.String TransactionId, System.String NseitRefNo, System.String PgRefNo, System.String PgStatus, System.String PgResponse)
        {
            IIIDL.URN objURN = null;
            try
            {
                objURN = new IIIDL.URN();
                objURN.UpdatePaymentStatus(ConnectionString, TransactionId, NseitRefNo, PgRefNo, PgStatus, PgResponse);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
        }

        public DataSet GetURNDataForDeletion(String ConnectionString,String URN, Int32 UserID, out String Message)
        {
            IIIDL.URN objURN = null;
            DataSet objDataSet = null;
            try
            {
                objURN = new IIIDL.URN();
                objDataSet = objURN.GetURNDataForDeletion(ConnectionString, URN, UserID, out Message);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return objDataSet;
        }

        
        public String DeleteURN(String ConnectionString, String URN, Int32 UserID )
        {
            IIIDL.URN objURN = null;
            DataSet objDataSet = null;
            String Message = String.Empty;
            try
            {
                objURN = new IIIDL.URN();
                Message = objURN.DeleteURN(ConnectionString, URN, UserID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return Message;
        }

        public DataSet GetQualData(String ConnectionString, String URN, Int32 UserID)
        {
            IIIDL.URN objURN = null;
            DataSet objDataSet = null;
            try
            {
                objURN = new IIIDL.URN();
                objDataSet = objURN.GetQualData(ConnectionString, URN, UserID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return objDataSet;
        }
        
        public DataSet GetURNDataForApproval(String ConnectionString,Int32 Hint, Int64 Id, Int32 UserID, out String Message)
        {
            IIIDL.URN objURN = null;
            DataSet objDataSet = null;
            try
            {
                objURN = new IIIDL.URN();
                objDataSet = objURN.GetURNDataForApproval(ConnectionString, Hint, Id, UserID, out Message);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return objDataSet;
        }

        public String ApproveRejectURN(String ConnectionString, Int64 Id, Int32 UserID, String ApproversRemarks, String Status)
        {
            IIIDL.URN objURN = null;
            String Message = String.Empty;
            try
            {
                objURN = new IIIDL.URN();
                Message = objURN.ApproveRejectURN(ConnectionString, Id, UserID, ApproversRemarks, Status);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return Message;
        }

        public DataSet UpdatePS(System.String ConnectionString, String OriginalFileName, System.Int32 UserId, String DirectoryNameAndPath, out String Message)
        {
            Message = String.Empty;
            IIIDL.URN objURN = null;
            DataSet objDataSet = null;
            ExcelParser excelParser = null;
            DataTable ExcelOutput = null;
            String PhotoPath = DirectoryNameAndPath + "\\Photo";
            String SignaturePath = DirectoryNameAndPath + "\\Signature";
            String ExcelPath = String.Empty;
            try
            {
                DirectoryInfo di = new DirectoryInfo(DirectoryNameAndPath);
                if (di.Exists)
                {
                    //DirectoryInfo diPhotos = new DirectoryInfo(PhotoPath);
                    //DirectoryInfo diSign = new DirectoryInfo(SignaturePath);
                    FileInfo[] fi = di.GetFiles("*.xls*", SearchOption.AllDirectories);
                    if (fi.Length != 1 /*|| !diPhotos.Exists || !diSign.Exists*/)
                    {
                        //Invalid zip file 
                        Message = "Invalid zip file template. Zip uploaded is not as per the template";
                        return objDataSet;
                    }
                    else
                    {
                        excelParser = new ExcelParser();
                        ExcelOutput = excelParser.ParseAllAsData(fi[0].FullName, "Sheet1$", true);

                        if (ExcelOutput.Rows.Count == 0)
                        {
                            Message = "File has no record";
                            return objDataSet;
                        }
                        String[] Fields = new string[] { "urn","photo_file_name","sign_file_name"};
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
                            ExcelOutput.Columns.Add("photo", typeof(System.Byte[]));
                            ExcelOutput.Columns.Add("signature", typeof(System.Byte[]));
                            ExcelOutput.Columns.Add("remarks", typeof(String));
                            foreach (DataRow dr in ExcelOutput.Rows)
                            {
                                //if (dr["urn"] == DBNull.Value)
                                //{
                                //    dr["remarks"] = String.Format("URN not specified");
                                //    continue;
                                //}
                                //else
                                //{
                                //    if ( Convert.ToString(dr["urn"]).Trim() == String.Empty  )
                                //    {
                                //        dr["remarks"] = String.Format("URN mentioned is empty string");
                                //        continue;
                                //    }
                                //}
                                fi = null;
                                StringBuilder sb = new StringBuilder();
                                if (dr["photo_file_name"] != DBNull.Value)
                                {
                                    fi = di.GetFiles(Convert.ToString(dr["photo_file_name"]),SearchOption.AllDirectories);
                                    if (fi.Length == 0)
                                    {
                                        sb.Append ( String.Format("File not found {0};", dr["photo_file_name"]));
                                    }
                                    if (fi.Length == 1)
                                    {
                                        dr["photo"] = File.ReadAllBytes(fi[0].FullName);
                                    }
                                    if (fi.Length > 1)
                                    {
                                        sb.Append ( String.Format("Multiple files with same names found {0};", dr["photo_file_name"]));
                                    }
                                }

                                if (dr["sign_file_name"] != DBNull.Value)
                                {
                                    fi  = di.GetFiles(Convert.ToString(dr["sign_file_name"]), SearchOption.AllDirectories);
                                    if (fi.Length == 0)
                                    {
                                        sb.Append ( String.Format("File not found {0};", dr["sign_file_name"]));
                                    }
                                    if (fi.Length == 1)
                                    {
                                        dr["signature"] = File.ReadAllBytes(fi[0].FullName);
                                    }
                                    if (fi.Length > 1)
                                    {
                                        sb.Append ( String.Format("Multiple files with same names found {0}", dr["sign_file_name"]));
                                    }
                                }
                                dr["remarks"] = sb.ToString().TrimEnd(';').Trim();
                                sb.Clear();
                                sb = null;
                            }

                            objURN = new IIIDL.URN();
                            objDataSet = objURN.UpdatePS(ConnectionString, ExcelOutput, UserId, OriginalFileName ); //STP_LIC_BulkUpdateApplicantPhotoSign
                            Message = String.Empty;
                        }
                    }
                }
                else
                {
                    Message = "Error occured while reading the file : Unable to read unzipped data";
                    return objDataSet;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objURN = null;
            }
            return objDataSet;
        }

        public DataSet UploadUrnExamCentreUpdate(System.String ConnectionString, System.Int32 UserId, String FileNameAndPath, out String Message)
        {
            String[] Fields = { "URN", "TEST_CENTER" };
            IIIDL.URN objURN = null;
            DataSet objDataSet = null;
            DataTable ExcelOutput = null;
            Boolean Status = true;
            Message = String.Empty;
            try
            {

                Utilities.FileParser.ExcelParser objExcelParser = new ExcelParser();
                ExcelOutput = objExcelParser.ParseAllAsData(FileNameAndPath, "Sheet1$", true);
                //Remove the unwanted columns
                for (Int32 i = ExcelOutput.Columns.Count - 1; i >= 0; i--)
                {
                    if (!Fields.Contains<String>(ExcelOutput.Columns[i].ColumnName))
                    {
                        ExcelOutput.Columns.RemoveAt(i);
                    }
                }


                //ExcelOutput.Columns.Add("UploadRemark", typeof(System.String));

                if (ExcelOutput.Rows.Count == 0)
                {
                    Status = false;
                    Message = "File has no record";
                }
                else
                {
                    String s = String.Empty;
                    //Check for the wanted columns
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
                        Status = false;
                        Message = String.Format("Invalid file format. The required fields : {0} not found.", s);
                    }
                    else
                    {
                        String val = String.Empty;
                        //Int32 InvalidRowCount = 0;
                        foreach (DataRow dr in ExcelOutput.Rows)
                        {
                            Boolean IsRecordValid = true;
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

                            if (Error != String.Empty)
                            {
                                Error = "Invalid characters found in columns: " + Error;
                                IsRecordValid = false;
                            }

                        }

                        //Bulkupload Excel Data To Database
                        objURN = new IIIDL.URN();
                        objDataSet = objURN.UploadUrnExamCentreUpdate(ConnectionString, ExcelOutput, UserId);
                        Message = String.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objURN = null;
            }
            return objDataSet;
        }

        public DataSet UploadTrainingDetails(System.String ConnectionString, String FileNameAndPath, System.Int32 UserId, out String Message)
        {
            Message = String.Empty;
            DataSet objDataSet = null;
            try
            {
                FileInfo fi = new FileInfo(FileNameAndPath);
                if (fi.Exists)
                {
                    Field[] fields = new Field[5];
                    fields[0] = new Field("urn", String.Empty, 0, 0, typeof(String), String.Empty, false);
                    fields[1] = new Field("training_start_date", String.Empty, 1, 0, typeof(DateTime), "dd-MMM-yyyy", false);
                    fields[2] = new Field("training_end_date", String.Empty, 2, 0, typeof(DateTime), "dd-MMM-yyyy", false);
                    fields[3] = new Field("training_hours", String.Empty, 3, 0, typeof(Int64), String.Empty, false);
                    fields[4] = new Field("tcc_expiry_date", String.Empty, 4, 0, typeof(DateTime), "dd-MMM-yyyy", false);

                    System.Collections.Specialized.StringCollection Headers = new System.Collections.Specialized.StringCollection();
                    System.Collections.Specialized.StringCollection Footers = new System.Collections.Specialized.StringCollection();
                    IIIDL.URN objURN = null;
                    TextParser textParser = null;
                    DataTable InputData = null;
                    try
                    {
                        textParser = new TextParser();

                        List<String> ExpectedHeaders = new List<String>();
                        ExpectedHeaders.Add("urn|training_start_date|training_end_date|training_hours|tcc_expiry_date");

                        InputData = textParser.Parse2(FileNameAndPath, 1, false, "|", fields, ExpectedHeaders, out Headers, out Message);
                        if (Message == String.Empty)
                        {
                            InputData.Columns.Remove("ERROR_FLAG");
                            InputData.Columns.Remove("ROW_ID");
                            //InputData.Columns.Remove("ORIGINAL_ROW");
                            //InputData.Columns.Remove("ERROR");

                            objURN = new IIIDL.URN();
                            objDataSet = objURN.UploadTrainingDetails(ConnectionString, InputData, UserId);
                            Message = String.Empty;
                        }
                        else
                        {
                            InputData.Columns.Remove("ERROR_FLAG");
                            InputData.Columns.Remove("ROW_ID");
                        }
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

        public DataSet GetPaymentReceiptData(System.String ConnectionString, System.String PAN)
        {
            IIIDL.URN objURN = null;
            DataSet objDataSet = null;
            try
            {
                objURN = new IIIDL.URN();
                objDataSet = objURN.GetURNForPAN(ConnectionString, PAN);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return objDataSet;
        }

        public Boolean ValidateURNandDOB(System.String ConnectionString, System.String URN, System.DateTime DOB)
        {
            IIIDL.URN objURN = null;
            Boolean RetVal = false;
            try
            {
                objURN = new IIIDL.URN();
                RetVal = objURN.ValidateURNandDOB(ConnectionString, URN, DOB);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                objURN = null;
            }
            return RetVal;
        }

    }
}
