using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Script.Serialization;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Text;
using IIIBL;

namespace IIIRegistrationPortal2
{
    public static class HelperUtilities
    {
        public static Boolean In(this String s, String[] list)
        {
            return list.Contains<String>(s);
        }

        public static String DataTable2JSON(this DataTable objDataTable)
        {
            String Result = String.Empty;
            List<Dictionary<String, Object>> objList = null;
            JavaScriptSerializer javaScriptSerializer = null;
            try
            {
                String retval = String.Empty;

                objList = new List<Dictionary<string, object>>();
                Dictionary<String, Object> row;
                foreach (DataRow dr in objDataTable.Rows)
                {
                    row = new Dictionary<String, Object>();
                    foreach (DataColumn col in objDataTable.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    objList.Add(row);
                }
                javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue;

                Result = javaScriptSerializer.Serialize(objList);
            }
            catch (Exception ex)
            {
                Result = String.Empty;
                throw (ex);
            }
            finally
            {
                objList = null;
                javaScriptSerializer = null;
            }
            return Result;
        }

        /*public static String ToJSON ( String Error, String Message, DataTable objDataTable)
        {
            if (Error.Trim() == String.Empty && Message.Trim() == String.Empty && (objDataTable == null || objDataTable.Rows.Count == 0))
            {
                throw new Exception("Invalid call. All the three parameter cannot be empty");
            }
            else
            {
                if (objDataTable == null)
                {
                    objDataTable = new DataTable();
                }
                Dictionary<String, String> dict = new Dictionary<string, string>();
                dict.Add("_STATUS_", Error);
                dict.Add("_MESSAGE_", Message);
                dict.Add("_DATA_", objDataTable.DataTable2JSON());

                JavaScriptSerializer obj = new JavaScriptSerializer();
                String s = obj.Serialize(dict);
                dict.Clear();
                dict = null;
                return s;
            }
        }*/

        public static String ToJSON4(Boolean Success, String Message, String Data)
        {
            Dictionary<String, String> dict = new Dictionary<string, string>();
            dict.Add("_STATUS_", Success ? "SUCCESS" : "FAIL");
            dict.Add("_MESSAGE_", Message);
            dict.Add("_DATA_", Data);

            JavaScriptSerializer obj = new JavaScriptSerializer();
            obj.MaxJsonLength = Int32.MaxValue;
            String s = obj.Serialize(dict);
            dict.Clear();
            dict = null;
            return s;
        }

        public static String ToJSON(Boolean Success, String Message, DataTable objDataTable)
        {
            if (objDataTable == null)
            {
                objDataTable = new DataTable();
            }
            Dictionary<String, String> dict = new Dictionary<string, string>();
            dict.Add("_STATUS_", Success ? "SUCCESS" : "FAIL");
            dict.Add("_MESSAGE_", Message);
            dict.Add("_DATA_", objDataTable.DataTable2JSON());

            JavaScriptSerializer obj = new JavaScriptSerializer();
            obj.MaxJsonLength = Int32.MaxValue;
            String s = obj.Serialize(dict);
            dict.Clear();
            dict = null;
            return s;
        }

        public static String ToJSON3(Boolean Success, String Message, DataTable[] objDataTables, KeyValuePair<String, String>[] KVPairs)
        {
            Dictionary<String, String> dict = new Dictionary<string, string>();
            dict.Add("_STATUS_", Success ? "SUCCESS" : "FAIL");
            dict.Add("_MESSAGE_", Message);

            if (objDataTables != null)
            {
                for (Int32 i = 0; i < objDataTables.Length; i++)
                {
                    dict.Add(String.Format("_DATA{0}_", i), objDataTables[i].DataTable2JSON());
                }
            }

            for (Int32 i = 0; i < KVPairs.Length; i++)
            {
                dict.Add(KVPairs[i].Key, KVPairs[i].Value);
            }

            JavaScriptSerializer obj = new JavaScriptSerializer();
            obj.MaxJsonLength = Int32.MaxValue;
            String s = obj.Serialize(dict);
            dict.Clear();
            dict = null;
            return s;
        }


        public static String ToJSON2(Boolean Success, String Message, DataTable []objDataTables)
        {
            Dictionary<String, String> dict = new Dictionary<string, string>();
            dict.Add("_STATUS_", Success ? "SUCCESS" : "FAIL");
            dict.Add("_MESSAGE_", Message);

            if (objDataTables != null)
            {
                for( Int32 i = 0; i< objDataTables.Length; i++)
                {
                    dict.Add(String.Format("_DATA{0}_", i), objDataTables[i].DataTable2JSON());
                }
            }

            JavaScriptSerializer obj = new JavaScriptSerializer();
            obj.MaxJsonLength = Int32.MaxValue;
            String s = obj.Serialize(dict);
            dict.Clear();
            dict = null;
            return s;
        }

        public static String ToJSON(Boolean Success, String Message )
        {
            Dictionary<String, String> dict = new Dictionary<string, string>();
            dict.Add("_STATUS_", Success ? "SUCCESS" : "FAIL");
            dict.Add("_MESSAGE_", Message);

            JavaScriptSerializer obj = new JavaScriptSerializer();
            obj.MaxJsonLength = Int32.MaxValue;
            String s = obj.Serialize(dict);
            dict.Clear();
            dict = null;
            return s;
        }

        public static String ToJSON(Boolean Success, String Message, DataTable objDataTable, KeyValuePair<String, String>[] KVPairs)
        {
            if (objDataTable == null)
            {
                objDataTable = new DataTable();
            }

            Dictionary<String, String> dict = new Dictionary<string, string>();
            dict.Add("_STATUS_", Success ? "SUCCESS" : "FAIL");
            dict.Add("_MESSAGE_", Message);
            dict.Add("_DATA_", objDataTable.DataTable2JSON());
            for (Int32 i =0; i< KVPairs.Length; i++)
            {
                dict.Add(KVPairs[i].Key, KVPairs[i].Value);
            }

            JavaScriptSerializer obj = new JavaScriptSerializer();
            String s = obj.Serialize(dict);
            dict.Clear();
            dict = null;
            return s;
        }

        public static void SendMail(String Subject, String To, String CC, String Bcc, String Body, Boolean IsHtml)
        {
            IIIBL.Mailer objMailer = null;
            try
            {
                String Server = System.Configuration.ConfigurationManager.AppSettings.Get("smtpserver");
                Int32 Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("smtpport"));
                String User = System.Configuration.ConfigurationManager.AppSettings.Get("smtpuser");
                String Password = System.Configuration.ConfigurationManager.AppSettings.Get("smtppassword");
                Boolean NeedsSSL = System.Configuration.ConfigurationManager.AppSettings.Get("sslrequired") == "Y";
                String MailFromEmailId = System.Configuration.ConfigurationManager.AppSettings.Get("mailfrom");

                objMailer = new IIIBL.Mailer(Server, Port, User, Password, NeedsSSL);
                objMailer.SendMail(Subject, MailFromEmailId, To, CC, Bcc, Body, IsHtml);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objMailer = null;
            }
        }

        public static void DataTable2File (this DataTable objDataTable, String FilePathAndName, String Delimiter = "|" )
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(FilePathAndName, false, System.Text.Encoding.ASCII);
                String s = String.Empty;
                foreach (DataColumn dc in objDataTable.Columns)
                {
                    s += dc.ColumnName + Delimiter;
                }
                s = s.Trim(Delimiter.ToCharArray());
                sw.WriteLine(s);
                foreach (DataRow dr in objDataTable.Rows)
                {
                    sw.WriteLine(String.Join(Delimiter, dr.ItemArray));
                }
                sw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
                sw = null;
            }
        }

        public static void DataTable2File(this DataTable objDataTable, String []Headers, String []Fields,  String FilePathAndName, String Delimiter = "|")
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(FilePathAndName, false, System.Text.Encoding.ASCII);

                for(Int32 i = 0; i < Headers.Length; i++)
                {
                    sw.WriteLine(Headers[i]);
                }

                StringBuilder sb = null;
                foreach (DataRow dr in objDataTable.Rows)
                {
                    sb = new StringBuilder();
                    foreach (String s in Fields)
                    {
                        sb.Append(String.Format("{0}{1}", dr[s], Delimiter));
                    }
                    sw.WriteLine(sb.ToString().TrimEnd(Delimiter.ToCharArray()));
                    sb.Clear();
                }
                sw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
                sw = null;
            }
        }


        public static Boolean AreEqual(this Byte[] a, Byte[] b)
        {
            Boolean retvalue = false;
            if (a.Length != b.Length)
                retvalue = false;
            else
            {
                Boolean IsSame = true;
                for (Int32 i = 0; i < a.Length; i++)
                {
                    IsSame = a[i] == b[i];
                    if (!IsSame)
                    {
                        break;
                    }
                }
                retvalue = IsSame;
            }
            return retvalue;
        }

        //public static string EncryptAadhaar(String Aadhaar)
        //{
        //    Byte[] key = UTF8Encoding.UTF8.GetBytes(System.Configuration.ConfigurationManager.AppSettings["AKey"].ToString());
        //    Byte[] IV = UTF8Encoding.UTF8.GetBytes(System.Configuration.ConfigurationManager.AppSettings["AIV"].ToString());

        //    Byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(Aadhaar);

        //    Byte[] byteEncrypt = IIIBL.AadhaarEncryptorDecryptor.Encrypt(toEncryptArray, IIIBL.EncryptionAlgorithm.TripleDES, key, IV);
        //    String encryptedvalue = Convert.ToBase64String(byteEncrypt);
        //    return encryptedvalue;
        //}

        //public static string DecryptAadhaar(String Aadhaar)
        //{
        //    Byte[] key = UTF8Encoding.UTF8.GetBytes(System.Configuration.ConfigurationManager.AppSettings["AKey"].ToString());
        //    Byte[] IV = UTF8Encoding.UTF8.GetBytes(System.Configuration.ConfigurationManager.AppSettings["AIV"].ToString());

        //    Byte[] toDecryptArray = Convert.FromBase64String(Aadhaar);

        //    Byte[] byteDecrypt = IIIBL.AadhaarEncryptorDecryptor.Decrypt(toDecryptArray, IIIBL.EncryptionAlgorithm.TripleDES, key, IV);
        //    String Decryptedvalue = UTF8Encoding.UTF8.GetString(byteDecrypt);
        //    return Decryptedvalue;
        //}


        public static void ZipTheFile(String ZipFilePathAndName, String FilePathAndName)
        {
            ZipArchive zip = ZipFile.Open(ZipFilePathAndName, ZipArchiveMode.Create);
            zip.CreateEntryFromFile(FilePathAndName, Path.GetFileName(FilePathAndName), CompressionLevel.Optimal);
            zip.Dispose();
        }

        public static void ZipTheDirectory(String DirectoryPathAndName, String ZipFilePathAndName )
        {
            ZipFile.CreateFromDirectory(DirectoryPathAndName, ZipFilePathAndName, CompressionLevel.Optimal, true);
        }

        public static void UnzipFile(String ZipFilePathAndName, String TargetDirectory)
        {
            ZipFile.ExtractToDirectory(ZipFilePathAndName, TargetDirectory);
        }

    }

    public static class CommonMessages
    {
        public static String INVALID_INPUT = "Invalid Input";
        public static String NO_DATA_FOUND = "No data found";
        public static String NO_DATA_FOUND_FOR_SELECTED_CRITERIA = "No data found for selected criteria";
        public static String ERROR_OCCURED = "Error occured while processing your request";
        public static String DATA_DELETION_SUCCESS = "Data deleted successfully";
        public static String DATA_DELETION_FAIL = "Data deletion failed";
        public static String DATA_SAVE_SUCCESS = "Data saved successfully";
        public static String DATA_SAVE_FAIL = "Error occured while saving data";
        public static String DATA_APPROVAL_SUCCESS = "Data approved successfully";
        public static String DATA_APPROVAL_FAIL = "Data approval failed";
        public static String DATA_REJECTION_SUCCESS = "Data rejected successfully";
        public static String DATA_REJECTION_FAIL = "Data rejection failed";
        public static String FILE_PROCESS_SUCCESS = "File processed successfully. Please check the response file for recordwise details.";
        public static String FILE_PROCESS_FAIL = "File processing failed";
        public static String REPORT_PROCESSING_SUCCESS_MIN = "Report generated successfully.";
        public static String REPORT_PROCESSING_SUCCESS = "Report generated successfully. Kindly download the same";

        public static String NULL_DATASET = "Null dataset returned";
        public static String EMPTY_DATASET = "Empty dataset returned";

    }



}