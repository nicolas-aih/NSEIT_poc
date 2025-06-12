using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using IIIDL;
using System.IO.Compression;


namespace IIIBL
{
    public class HSTCPrinter
    {
        public String PrintHallTicket(String ConnectionString, String URN, DateTime DOB, DateTime pExamDate, Int32 Source, String LogoFileNameAndPath, String OutputPath, String FooterFile1, String FooterFile2, String FooterFileRP)
        {
            String RetVal = String.Empty;
            IIIDL.HSTCPrinter objHSTCPrinter = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String Error = String.Empty;
            try
            {
                objHSTCPrinter = new IIIDL.HSTCPrinter();
                objDataSet = objHSTCPrinter.GetHallTicket(ConnectionString, URN, DOB, pExamDate, Source, out Error);
                if (Error.Trim() == String.Empty)
                {
                    if (objDataSet != null && objDataSet.Tables.Count != 0)
                    {
                        objDataTable = objDataSet.Tables[0];

                        /******************************/
                        String strNote = String.Empty;
                        String CurrentDirectory = OutputPath;
                        byte[] img_Logo = File.ReadAllBytes(LogoFileNameAndPath);

                        if (objDataTable.Rows.Count == 0)
                        {
                            RetVal = "NO_DATA_FOUND";
                        }
                        else
                        {
                            foreach (DataRow dr in objDataTable.Rows)
                            {
                                byte[] _img = (byte[])dr["imgApplicantPhoto"];
                                byte[] _sign = (byte[])dr["imgApplicantSign"];
                                String ExamDate = DateTime.Parse(dr["dtExamDate"].ToString()).ToString("dd MMM yyyy");
                                String ExamSlot = DateTime.Parse(dr["dtExamDate"].ToString()).ToString("hh:mm tt");
                                Boolean IsRP = Convert.ToInt32(dr["IS_RP"]) == 1;
                                String ReportingTime = Convert.ToDateTime(dr["dtExamDate"]).AddMinutes(IsRP ? -30 : -15).ToString("hh:mm tt");

                                String CandidateAddress = dr["ApplicantHouse"].ToString().Trim() + "\n" + dr["ApplicantStreet"].ToString().Trim() + "\n" + dr["ApplicantTown"].ToString().Trim() + "\n" + dr["varDistrictName"].ToString().Trim() + " - " + (Int32)dr["intCurrPINCode"];
                                String ExamCenterAddress = dr["ExamHouse"].ToString().Trim() + "\n" + dr["ExamStreet"].ToString().Trim() + "\n" + dr["ExamTown"].ToString().Trim() + "\n" + dr["ExamDistrict"].ToString().Trim() + " - " + (Int32)dr["ExamPincode"] + "\n" + dr["ExamStateName"].ToString().Trim();
                                
                                GenerateHallticket(OutputPath + "\\", dr["varApplicantName"].ToString(), CandidateAddress, dr["varExamCenterName"].ToString(), ExamCenterAddress, URN,
                                    dr["varExamRollNo"].ToString(), dr["TCC"].ToString(), ExamDate, ExamSlot, dr["InsType"].ToString(), dr["varLanguage"].ToString(),
                                    _img, _sign, img_Logo, ReportingTime, FooterFile1, FooterFile2, FooterFileRP, IsRP);

                                RetVal = "SUCCESS";

                                //Response.Clear();
                                //Response.Buffer = true;
                                //Response.AddHeader("Content-Disposition", "attachment; filename=" + "HT_" + dr["chrRollNumber"].ToString() + ".pdf");
                                //Response.TransmitFile(OutputPath + "HT_" + dr["chrRollNumber"].ToString() + ".pdf");
                                //Response.Flush();
                                //Response.End();
                            }
                        }
                        /******************************/
                    }
                    else
                    {
                        RetVal = "NO_DATA_FOUND";
                    }
                }
                else
                {
                    RetVal = Error;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objHSTCPrinter = null;
                objDataSet = null;
                objDataTable = null;
            }
            return RetVal;
        }

        public String PrintScorecard(String ConnectionString, String URN, DateTime DOB, DateTime ExamDate, Int32 Source, String LogoFileNameAndPath, String OutputPath)
        {
            String RetVal = String.Empty;
            IIIDL.HSTCPrinter objHSTCPrinter = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String Error = String.Empty;
            try
            {
                objHSTCPrinter = new IIIDL.HSTCPrinter();
                objDataSet = objHSTCPrinter.GetScoreCard(ConnectionString, URN, DOB, ExamDate, Source, out Error);
                if (Error.Trim() == String.Empty)
                {
                    if (objDataSet != null && objDataSet.Tables.Count != 0)
                    {
                        objDataTable = objDataSet.Tables[0];

                        /******************************/
                        if (objDataTable.Rows.Count == 0)
                        {
                            RetVal = "NO_DATA_FOUND";
                        }
                        else
                        {
                            String strNote = String.Empty;
                            byte[] img_Logo = File.ReadAllBytes(LogoFileNameAndPath);

                            foreach (DataRow dr in objDataTable.Rows)
                            {
                                byte[] _img = (byte[])dr["Photo"];

                                GenerateScorecard(OutputPath + "\\", dr["RegistrationNo"].ToString(), URN, dr["ApplicantName"].ToString(), dr["Certification"].ToString(),
                                    dr["TestCenter"].ToString(), dr["TestDate"].ToString(), dr["TestSlot"].ToString(), dr["MarksScored"].ToString(), dr["MaximumMarks"].ToString(), dr["Percentage"].ToString(),
                                    dr["Required Percentage"].ToString(), _img, dr["Remarks"].ToString(), dr["cert_type"].ToString(), img_Logo);

                                RetVal = "SUCCESS";
                            }
                        }
                        /******************************/
                    }
                    else
                    {
                        RetVal = "NO_DATA_FOUND";
                    }
                }
                else
                {
                    RetVal = Error;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objHSTCPrinter = null;
                objDataSet = null;
                objDataTable = null;
            }
            return RetVal;
        }

        private void GenerateHallticket(String CurrentDirectoryPath, String CandidateName, String CandidateAddress, String TestCenterName, String TestCenterAddress, String URN, String RegistrationNo, String TCC,
                String ExaminationDate, String ExamSlot, String ExamModule, String ExamLanguage, Byte[] Photo, Byte[] Sign, Byte[] Logo,
                String ReportingTime, String FooterFile1, String FooterFile2, String FooterFileRP, Boolean IsRP)
        {
            Document document = null;
            PdfWriter writer = null;
            System.IO.FileStream fs = null;
            try
            {
                iTextSharp.text.Font f = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font f8 = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.ITALIC);
                iTextSharp.text.Font f8bi = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLDITALIC);
                iTextSharp.text.Font f9bi = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLDITALIC);

                String strAgent = String.Empty;
                fs = new FileStream(CurrentDirectoryPath + String.Format("HT_{0}.pdf", URN), FileMode.Create);
                document = new Document(PageSize.A4, 25, 25, 30, 30);
                // Create an instance to the PDF file by creating an instance of the PDF 
                // Writer class using the document and the filestrem in the constructor. 

                writer = PdfWriter.GetInstance(document, fs);

                // Open the document to enable you to write to the document 

                document.Open();

                // Add a simple and wellknown phrase to the document in a flow layout manner 
                PdfPTable table = new PdfPTable(1);
                table.WidthPercentage = 100;

                iTextSharp.text.Image i = iTextSharp.text.Image.GetInstance(Logo);
                i.ScalePercent(40f);
                i.Alignment = 1;
                PdfPCell Headercell = new PdfPCell();
                Headercell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                Headercell.AddElement(i);

                Paragraph para = new Paragraph("\"G\" Block, Plot No. C-46, Bandra-Kurla Complex, Near American Counsulate,Mumbai-400051.\nMaharashtra. Email ID - reg.exams@iii.org.in\n ", f);
                para.Alignment = 1;
                Headercell.AddElement(para);
                table.AddCell(Headercell);

                //Row 2 of table 1
                //PdfPCell Headercell1 = new PdfPCell();
                //Headercell1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                //Headercell1.AddElement(new Paragraph("\"G\" Block, Plot No. C-46, Bandra-Kurla Complex, Near American Counsulate,Mumbai-400051.\nMaharashtra.India.Tel No - 022 - 26544220 / 257 / 224 / 208.Email ID - reg.exams@iii.org.in.\n ", f));
                //table.AddCell(Headercell1);

                //3rd Row of table 1
                PdfPCell Contentcell1 = new PdfPCell();
                Contentcell1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                Contentcell1.Padding = 0;
                PdfPTable table2 = new PdfPTable(new float[] { 3, 1f });
                table2.WidthPercentage = 100;

                if (URN.Trim().StartsWith("CAI"))
                {
                    strAgent = "HALL TICKET\n\nPRE RECRUITMENT TEST FOR CORPORATE AGENTS\n ";
                }
                else if (URN.Trim().StartsWith("WAI"))
                {
                    strAgent = "HALL TICKET\n\nPRE RECRUITMENT TEST FOR WEB AGGREGATORS\n ";
                }
                else if (URN.Trim().StartsWith("IMF"))
                {
                    strAgent = "HALL TICKET\n\nPRE RECRUITMENT TEST FOR INSURANCE MARKETING FIRM\n ";
                }
                else if (URN.Trim().StartsWith("BAV"))
                {
                    strAgent = "HALL TICKET\n\nPRE RECRUITMENT TEST FOR INSURANCE BROKERS\n ";
                }
                else
                {
                    strAgent = "HALL TICKET\n\nPRE RECRUITMENT TEST FOR INSURANCE COMPANIES\n ";
                }

                //"Hall Ticket\n\nPre Recruitment test for insurance agents\n "
                //table 2 Row 1
                PdfPCell TopCell = new PdfPCell(new Phrase(strAgent, f));
                TopCell.Colspan = 2;
                TopCell.HorizontalAlignment = 1;
                table2.AddCell(TopCell);
                //Table 2 Row 2
                PdfPCell LeftCell = new PdfPCell();
                LeftCell.Padding = 0;
                PdfPTable table3 = new PdfPTable(new float[] { 1f, 2f });
                table3.WidthPercentage = 100;
                table3.AddCell(new PdfPCell(new Phrase("Candidate Name", f)));
                table3.AddCell(new PdfPCell(new Phrase(CandidateName, f)));
                table3.AddCell(new PdfPCell(new Phrase("URN", f)));
                table3.AddCell(new PdfPCell(new Phrase(URN, f)));
                table3.AddCell(new PdfPCell(new Phrase("Registration Number", f)));
                table3.AddCell(new PdfPCell(new Phrase(RegistrationNo, f)));
                table3.AddCell(new PdfPCell(new Phrase("Module Name", f)));
                table3.AddCell(new PdfPCell(new Phrase(ExamModule, f)));
                table3.AddCell(new PdfPCell(new Phrase("TCC Status", f)));
                table3.AddCell(new PdfPCell(new Phrase(TCC, f)));
                table3.AddCell(new PdfPCell(new Phrase("Exam Language", f)));
                table3.AddCell(new PdfPCell(new Phrase(ExamLanguage, f)));
                table3.AddCell(new PdfPCell(new Phrase("Exam Date", f)));
                table3.AddCell(new PdfPCell(new Phrase(ExaminationDate, f)));
                table3.AddCell(new PdfPCell(new Phrase("Exam Time", f)));
                table3.AddCell(new PdfPCell(new Phrase(ExamSlot, f)));

                if (IsRP)
                {
                    table3.AddCell(new PdfPCell(new Phrase("Login Time", f)));
                    table3.AddCell(new PdfPCell(new Phrase(ReportingTime, f)));

                    table3.AddCell(new PdfPCell(new Phrase("Username", f)));
                    table3.AddCell(new PdfPCell(new Phrase("Mock exam - URN No\nLive exam - Registration Number", f)));

                    table3.AddCell(new PdfPCell(new Phrase("Password", f)));
                    table3.AddCell(new PdfPCell(new Phrase("Enter Your Date of Birth (DD-MM-YYYY)", f)));
                }
                else
                {
                    table3.AddCell(new PdfPCell(new Phrase("Reporting Time", f)));
                    table3.AddCell(new PdfPCell(new Phrase(ReportingTime, f)));
                }

                LeftCell.AddElement(table3);

                PdfPCell RightCell = new PdfPCell(new Phrase("Candidate Photo Goes Here"));

                PdfPTable tableN = new PdfPTable(new float[] { 1f });
                tableN.WidthPercentage = 100;
                tableN.HorizontalAlignment = 1;

                iTextSharp.text.Image i2 = iTextSharp.text.Image.GetInstance(Photo);
                i2.ScaleAbsoluteHeight(126f);
                i2.ScaleAbsoluteWidth(84f);
                i2.Alignment = 1;

                PdfPCell cellPhoto = new PdfPCell(i2);
                cellPhoto.HorizontalAlignment = 1;
                cellPhoto.Padding = 5;

                iTextSharp.text.Image i3 = iTextSharp.text.Image.GetInstance(Sign);
                i3.ScaleAbsoluteHeight(50f);
                i3.ScaleAbsoluteWidth(100f);
                i3.Alignment = 1;

                PdfPCell cellSignature = new PdfPCell(i3);
                cellSignature.HorizontalAlignment = 1;
                cellSignature.Padding = 5;

                tableN.AddCell(cellPhoto);
                //tableN.AddCell(new PdfPCell());
                tableN.AddCell(cellSignature);

                RightCell.AddElement(tableN);


                //RightCell.AddElement(i2);
                //RightCell.AddElement(i3);

                table2.AddCell(LeftCell);
                table2.AddCell(RightCell);

                Contentcell1.AddElement(table2);
                table.AddCell(Contentcell1);

                if (IsRP)
                {

                }
                else
                {
                    //4th Row of table
                    PdfPCell Contentcell2 = new PdfPCell();
                    Contentcell2.Padding = 0;
                    Contentcell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                    PdfPTable table4 = new PdfPTable(new float[] { 1f, 3f });
                    table4.WidthPercentage = 100;
                    //table4.AddCell(new PdfPCell(new Phrase("Candidate Name", f)));
                    //table4.AddCell(new PdfPCell(new Phrase(CandidateName, f)));
                    //table4.AddCell(new PdfPCell(new Phrase("Address", f)));
                    //table4.AddCell(new PdfPCell(new Phrase(CandidateAddress, f)));
                    table4.AddCell(new PdfPCell(new Phrase("Examination Center\n& Address", f)));
                    table4.AddCell(new PdfPCell(new Phrase(TestCenterName + "\n\n" + TestCenterAddress, f)));
                    //table4.AddCell(new PdfPCell(new Phrase("Exam Date & Time", f)));
                    //table4.AddCell(new PdfPCell(new Phrase(ExaminationDate + "\t" + ExamSlot, f)));
                    Contentcell2.AddElement(table4);

                    table.AddCell(Contentcell2);
                }
                //5th row of table

                PdfPCell Footercell = new PdfPCell();
                StringBuilder sb = new StringBuilder();
                ////sb.AppendLine("Generated On: " + DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));
                ////sb.AppendLine("Note :- In case of any exam or schedule related queries please contact +91-22-42706500");
                ////sb.AppendLine(" ");
                ////sb.AppendLine("Instructions for Candidates :- ");
                //String FooterFile = FooterFile1;
                String[] FooterLines = File.ReadAllLines(FooterFile1);
                foreach (String s in FooterLines)
                {
                    sb.AppendLine(s);
                }

                StringBuilder sb2 = new StringBuilder();
                //String FooterFile2 = FooterFile2;
                String[] FooterLines2 = File.ReadAllLines(FooterFile2);
                foreach (String s in FooterLines2)
                {
                    sb2.AppendLine(s);
                }

                StringBuilder sbRP = new StringBuilder();
                String[] FooterLinesRP = File.ReadAllLines(FooterFileRP);//to be sent as parameter...
                foreach (String s in FooterLinesRP)
                {
                    sbRP.AppendLine(s);
                }

                Phrase phr1, phr2, phr3, phr4, phr5 = null;

                if (IsRP)
                {
                    phr1 = new Phrase("Generated On: ", f8bi);
                    phr2 = new Phrase(DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt") + "\n", f8);
                    //Phrase phr3 = new Phrase("Note :-", f8bi);
                    //Phrase phr4 = new Phrase("In case of any exam or schedule related queries please contact + 91 - 22 - 42706500\n", f8);
                    phr5 = new Phrase("Instructions Applicable only for Remotely Proctored Exam:- \n", f9bi);

                    Paragraph p = new Paragraph();
                    p.Add(phr1);
                    p.Add(phr2);
                    p.Add(phr5);

                    p.Add(new Phrase(sbRP.ToString(), f8));
                    Footercell.AddElement(p);

                    table.AddCell(Footercell);
                }
                else
                {
                    phr1 = new Phrase("Generated On: ", f8bi);
                    phr2 = new Phrase(DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt") + "\n", f8);
                    phr3 = new Phrase("Note :-", f8bi);
                    phr4 = new Phrase("In case of any exam or schedule related queries please contact + 91 - 22 - 42706500\n", f8);
                    phr5 = new Phrase("Instructions for Candidates :- \n", f9bi);
                    Paragraph p = new Paragraph();
                    p.Add(phr1);
                    p.Add(phr2);
                    p.Add(phr3);
                    p.Add(phr4);
                    p.Add(phr5);

                    p.Add(new Phrase(sb.ToString(), f8));
                    Footercell.AddElement(p);

                    table.AddCell(Footercell);

                    if (FooterLines2.Length > 0)
                    {
                        Footercell = new PdfPCell();
                        p = new Paragraph();
                        p.Add(new Phrase(sb2.ToString(), f8));
                        Footercell.AddElement(p);
                        table.AddCell(Footercell);
                    }
                }


                //Candidate Instructions End....

                Headercell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                table.KeepTogether = true;
                document.Add(table);

                // Close the document 
                document.Close();

                // Close the writer instance 
                writer.Close();
                // Always close open filehandles explicity
                fs.Close();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs!=null)
                {
                    fs.Close();
                }
                fs = null;
            }
        }

        private void GenerateScorecard(String CurrentDirectoryPath, String RegistrationNo, String URN, String CandidateName, String Certification, String TestCenterName, String ExamDate, String ExamSlot,
                String MarksScored, String MaximumMarks, String Percentage, String RequiredPercentage, Byte[] Photo, String Remarks, String VVV, Byte[] Logo
                )
        {
            Document document = null;
            PdfWriter writer = null;
            System.IO.FileStream fs = null;
            try
            {
                iTextSharp.text.Font f = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD);

                fs = new FileStream(CurrentDirectoryPath + String.Format("SC_{0}.pdf", URN), FileMode.Create);
                document = new Document(PageSize.A4, 25, 25, 30, 30);
                // Create an instance to the PDF file by creating an instance of the PDF 
                // Writer class using the document and the filestrem in the constructor. 

                writer = PdfWriter.GetInstance(document, fs);

                // Open the document to enable you to write to the document 

                document.Open();

                // Add a simple and wellknown phrase to the document in a flow layout manner 
                PdfPTable table = new PdfPTable(1);
                table.WidthPercentage = 100;

                iTextSharp.text.Image i = iTextSharp.text.Image.GetInstance(Logo);
                i.ScalePercent(40f);
                i.Alignment = 1;
                PdfPCell Headercell = new PdfPCell();
                Headercell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                Headercell.AddElement(i);

                

                //Paragraph para = new Paragraph("\"G\" Block, Plot No. C-46, Bandra-Kurla Complex, Near American Counsulate,Mumbai-400051.\nMaharashtra. India. Tel No - 022 - 26544220 / 257 / 224 / 208. Email ID - reg.exams@iii.org.in\n ", f);
                Paragraph para = new Paragraph("\"G\" Block, Plot No. C-46, Bandra-Kurla Complex, Near American Counsulate,Mumbai-400051.\nMaharashtra. India. Email ID -reg.exams@iii.org.in\n ", f);
                para.Alignment = 1;
                Headercell.AddElement(para);
                table.AddCell(Headercell);

                //Row 2 of table 1
                //PdfPCell Headercell1 = new PdfPCell();
                //Headercell1.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
                //Headercell1.AddElement(new Paragraph("\"G\" Block, Plot No. C-46, Bandra-Kurla Complex, Near American Counsulate,Mumbai-400051.\nMaharashtra.India.Tel No - 022 - 26544220 / 257 / 224 / 208.Email ID - reg.exams@iii.org.in.\n ", f));
                //table.AddCell(Headercell1);

                //3rd Row of table 1
                PdfPCell Contentcell1 = new PdfPCell();
                Contentcell1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                Contentcell1.Padding = 0;
                PdfPTable table2 = new PdfPTable(new float[] { 3, 1f });
                table2.WidthPercentage = 100;
                //table 2 Row 1
                PdfPCell TopCell = new PdfPCell(new Phrase(VVV, f));
                TopCell.Colspan = 2;
                TopCell.HorizontalAlignment = 1;
                TopCell.FixedHeight = 30f;
                //TopCell.Width = 30f;
                TopCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                table2.AddCell(TopCell);
                //Table 2 Row 2
                PdfPCell LeftCell = new PdfPCell();
                LeftCell.Padding = 0;
                PdfPTable table3 = new PdfPTable(new float[] { 1f, 2f });
                table3.WidthPercentage = 100;
                table3.AddCell(new PdfPCell(new Phrase("Registration Number:", f)));
                table3.AddCell(new PdfPCell(new Phrase(RegistrationNo, f)));
                table3.AddCell(new PdfPCell(new Phrase("URN IRDA Number:", f)));
                table3.AddCell(new PdfPCell(new Phrase(URN, f)));
                table3.AddCell(new PdfPCell(new Phrase("Examinee Name:", f)));
                table3.AddCell(new PdfPCell(new Phrase(CandidateName, f)));
                table3.AddCell(new PdfPCell(new Phrase("Examination:", f)));
                table3.AddCell(new PdfPCell(new Phrase(Certification, f)));
                table3.AddCell(new PdfPCell(new Phrase("Test Center:", f)));
                table3.AddCell(new PdfPCell(new Phrase(TestCenterName, f)));
                table3.AddCell(new PdfPCell(new Phrase("Test Date:", f)));
                table3.AddCell(new PdfPCell(new Phrase(ExamDate, f)));
                table3.AddCell(new PdfPCell(new Phrase("Test Slot:", f)));
                table3.AddCell(new PdfPCell(new Phrase(ExamSlot, f)));
                LeftCell.AddElement(table3);

                PdfPCell RightCell = new PdfPCell(new Phrase("Candidate Photo Goes Here"));
                iTextSharp.text.Image i2 = iTextSharp.text.Image.GetInstance(Photo);
                i2.ScaleAbsoluteHeight(123f);
                i2.ScaleAbsoluteWidth(84f);
                i2.Alignment = 1;

                //iTextSharp.text.Image i3 = iTextSharp.text.Image.GetInstance(Sign);
                //i3.ScaleAbsoluteHeight(50f);
                //i3.ScaleAbsoluteWidth(100f);
                //i3.Alignment = 1;
                RightCell.AddElement(i2);
                //RightCell.AddElement(i3);

                table2.AddCell(LeftCell);
                table2.AddCell(RightCell);

                table2.SpacingBefore = 30;
                table2.SpacingAfter = 30;
                Contentcell1.AddElement(table2);
                table.AddCell(Contentcell1);

                //4th Row of table
                PdfPCell Contentcell2 = new PdfPCell();
                Contentcell2.Padding = 0;
                Contentcell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right

                PdfPTable table4 = new PdfPTable(new float[] { 1f, 1f, 1f, 1f, 1f });
                table4.WidthPercentage = 100;
                table4.HorizontalAlignment = 0;

                PdfPCell p1 = new PdfPCell(new Phrase("\nMarks Scored\n", f));
                p1.HorizontalAlignment = 1;
                table4.AddCell(p1);

                PdfPCell p2 = new PdfPCell(new Phrase("\nTotal Marks\n", f));
                p2.HorizontalAlignment = 1;
                table4.AddCell(p2);

                PdfPCell p3 = new PdfPCell(new Phrase("\nPercentage Secured\n", f));
                p3.HorizontalAlignment = 1;
                table4.AddCell(p3);

                PdfPCell p4 = new PdfPCell(new Phrase("\nPassing Percentage Required\n", f));
                p4.HorizontalAlignment = 1;
                table4.AddCell(p4);

                PdfPCell p5 = new PdfPCell(new Phrase("\nStatus\n", f));
                p5.HorizontalAlignment = 1;
                table4.AddCell(p5);

                PdfPCell p6 = new PdfPCell(new Phrase("\n" + MarksScored + "\n", f));
                p6.HorizontalAlignment = 1;
                table4.AddCell(p6);

                PdfPCell p7 = new PdfPCell(new Phrase("\n" + MaximumMarks + "\n", f));
                p7.HorizontalAlignment = 1;
                table4.AddCell(p7);

                PdfPCell p8 = new PdfPCell(new Phrase("\n" + Percentage + "\n", f));
                p8.HorizontalAlignment = 1;
                table4.AddCell(p8);

                PdfPCell p9 = new PdfPCell(new Phrase("\n" + RequiredPercentage + "\n", f));
                p9.HorizontalAlignment = 1;
                table4.AddCell(p9);

                PdfPCell p10 = new PdfPCell(new Phrase("\n" + Remarks + "\n", f));
                p10.HorizontalAlignment = 1;
                table4.AddCell(p10);

                table4.SpacingAfter = 30;
                table4.SpacingBefore = 30;
                Contentcell2.AddElement(table4);

                table.AddCell(Contentcell2);
                //5th row of table
                PdfPCell Footercell = new PdfPCell();
                Paragraph p = new Paragraph("Executed On: " + DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt") + "\n", f);
                Paragraph pa = new Paragraph("Executed On: " + DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt") + "\n" + "NOTE:Please download Examination Passing Certificate using following link: https://www.insuranceinstituteofindia.com/web/guest/process-to-share-applicant-exam-result .\n(This is not applicable to migrated URNs. This is applicable to only URNs which are registered for online training on www.insuranceinstituteofindia.com on/after 01st April,2016 )", f);

                if (URN.Trim().StartsWith("CAI") || URN.Trim().StartsWith("WAI") || URN.Trim().StartsWith("IMF") || URN.Trim().StartsWith("BAV"))
                {
                    Footercell.AddElement(pa);
                }
                else
                {
                    Footercell.AddElement(p);
                }


                Headercell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right


                table.AddCell(Footercell);
                document.Add(table);

                // Close the document 
                document.Close();

                // Close the writer instance 
                writer.Close();
                // Always close open filehandles explicity
                fs.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
                fs = null;
            }
        }

        private String NumberToString(decimal d)
        {
            Hashtable hashtable = new Hashtable();
            //hashtable.Add(0, "Zero");
            hashtable.Add(1, "One");
            hashtable.Add(2, "Two");
            hashtable.Add(3, "Three");
            hashtable.Add(4, "Four");
            hashtable.Add(5, "Five");
            hashtable.Add(6, "Six");
            hashtable.Add(7, "Seven");
            hashtable.Add(8, "Eight");
            hashtable.Add(9, "Nine");
            hashtable.Add(10, "Ten");
            hashtable.Add(11, "Eleven");
            hashtable.Add(12, "Twelve");
            hashtable.Add(13, "Thirteen");
            hashtable.Add(14, "Fourteen");
            hashtable.Add(15, "Fifteen");
            hashtable.Add(16, "Sixteen");
            hashtable.Add(17, "Seventeen");
            hashtable.Add(18, "Eighteen");
            hashtable.Add(19, "Nineteen");
            hashtable.Add(20, "Twenty");
            hashtable.Add(21, "Twenty One");
            hashtable.Add(22, "Twenty Two");
            hashtable.Add(23, "Twenty Three");
            hashtable.Add(24, "Twenty Four");
            hashtable.Add(25, "Twenty Five");
            hashtable.Add(26, "Twenty Six");
            hashtable.Add(27, "Twenty Seven");
            hashtable.Add(28, "Twenty Eight");
            hashtable.Add(29, "Twenty Nine");
            hashtable.Add(30, "Thirty");
            hashtable.Add(31, "Thirty One");
            hashtable.Add(32, "Thirty Two");
            hashtable.Add(33, "Thirty Three");
            hashtable.Add(34, "Thirty Four");
            hashtable.Add(35, "Thirty Five");
            hashtable.Add(36, "Thirty Six");
            hashtable.Add(37, "Thirty Seven");
            hashtable.Add(38, "Thirty Eight");
            hashtable.Add(39, "Thirty Nine");
            hashtable.Add(40, "Fourty");
            hashtable.Add(41, "Fourty One");
            hashtable.Add(42, "Fourty Two");
            hashtable.Add(43, "Fourty Three");
            hashtable.Add(44, "Fourty Four");
            hashtable.Add(45, "Fourty Five");
            hashtable.Add(46, "Fourty Six");
            hashtable.Add(47, "Fourty Seven");
            hashtable.Add(48, "Fourty Eight");
            hashtable.Add(49, "Fourty Nine");
            hashtable.Add(50, "Fifty");
            hashtable.Add(51, "Fifty One");
            hashtable.Add(52, "Fifty Two");
            hashtable.Add(53, "Fifty Three");
            hashtable.Add(54, "Fifty Four");
            hashtable.Add(55, "Fifty Five");
            hashtable.Add(56, "Fifty Six");
            hashtable.Add(57, "Fifty Seven");
            hashtable.Add(58, "Fifty Eight");
            hashtable.Add(59, "Fifty Nine");
            hashtable.Add(60, "Sixty");
            hashtable.Add(61, "Sixty One");
            hashtable.Add(62, "Sixty Two");
            hashtable.Add(63, "Sixty Three");
            hashtable.Add(64, "Sixty Four");
            hashtable.Add(65, "Sixty Five");
            hashtable.Add(66, "Sixty Six");
            hashtable.Add(67, "Sixty Seven");
            hashtable.Add(68, "Sixty Eight");
            hashtable.Add(69, "Sixty Nine");
            hashtable.Add(70, "Seventy");
            hashtable.Add(71, "Seventy One");
            hashtable.Add(72, "Seventy Two");
            hashtable.Add(73, "Seventy Three");
            hashtable.Add(74, "Seventy Four");
            hashtable.Add(75, "Seventy Five");
            hashtable.Add(76, "Seventy Six");
            hashtable.Add(77, "Seventy Seven");
            hashtable.Add(78, "Seventy Eight");
            hashtable.Add(79, "Seventy Nine");
            hashtable.Add(80, "Eighty");
            hashtable.Add(81, "Eighty One");
            hashtable.Add(82, "Eighty Two");
            hashtable.Add(83, "Eighty Three");
            hashtable.Add(84, "Eighty Four");
            hashtable.Add(85, "Eighty Five");
            hashtable.Add(86, "Eighty Six");
            hashtable.Add(87, "Eighty Seven");
            hashtable.Add(88, "Eighty Eight");
            hashtable.Add(89, "Eighty Nine");
            hashtable.Add(90, "Ninety");
            hashtable.Add(91, "Ninety One");
            hashtable.Add(92, "Ninety Two");
            hashtable.Add(93, "Ninety Three");
            hashtable.Add(94, "Ninety Four");
            hashtable.Add(95, "Ninety Five");
            hashtable.Add(96, "Ninety Six");
            hashtable.Add(97, "Ninety Seven");
            hashtable.Add(98, "Ninety Eight");
            hashtable.Add(99, "Ninety Nine");

            String s = String.Empty;
            Decimal Rupees = Decimal.Floor(d);
            Decimal Paise = (d - Rupees) * 100;
            Int32 di = Convert.ToInt32(Rupees);

            s = Rupees > 0 ? "Rupees " : s;

            Int32 Crores = (di / 10000000);
            di = Crores > 0 ? di % 10000000 : di;
            s += Crores > 0 ? hashtable[Crores] + " Crore " : String.Empty;

            Int32 Lakhs = (di / 100000);
            di = Lakhs > 0 ? di % 100000 : di;
            s += Lakhs > 0 ? hashtable[Lakhs] + " Lakh " : String.Empty;

            Int32 Thousands = (di / 1000);
            di = Thousands > 0 ? di % 1000 : di;
            s += Thousands > 0 ? hashtable[Thousands] + " Thousand " : String.Empty;

            Int32 Hundreds = (di / 100);
            di = Hundreds > 0 ? di % 100 : di;
            s += Hundreds > 0 ? hashtable[Hundreds] + " Hundred " : String.Empty;

            s += di > 0 ? "and " + hashtable[di] : String.Empty;

            if (Rupees > 0)
            {
                s += Paise > 0 ? " and " + hashtable[(Int32)Paise] + " Paisa Only" : " Only";
            }
            else
            {
                s += Paise > 0 ? "Rupees Zero and " + hashtable[(Int32)Paise] + " Paisa Only" : " Only";
            }


            hashtable = null;

            return s;
        }

        public String PrintReceipt(String ConnectionString, String URN, String LogoImagePath, String SignatureImagePath, String OutputPath, String GuidName)
        {
            String RetVal = String.Empty;
            IIIDL.URN objUrn = null;
            DataSet objDataSet = null;
            DataTable objDataTable = null;
            String Error = String.Empty;
            Int32 DataCount = 0;
            String FileName = String.Empty;
            Int32 FileCounter = 1;
            String OutputFile = String.Empty;
            try
            {
                objUrn = new IIIDL.URN();
                objDataSet = objUrn.GetPaymentReceiptData(ConnectionString, URN);
                if (Error.Trim() == String.Empty)
                {
                    if (objDataSet != null && objDataSet.Tables.Count != 0)
                    {
                        objDataTable = objDataSet.Tables[0];

                        /******************************/
                        String strNote = String.Empty;
                        String CurrentDirectory = String.Empty;
                        if (objDataTable.Rows.Count == 0)
                        {
                            RetVal = "NO_DATA_FOUND";
                        }
                        else
                        {
                            CurrentDirectory = OutputPath + "\\" + GuidName;
                            //creating folder for zip
                            if (!Directory.Exists(CurrentDirectory))
                            {
                                Directory.CreateDirectory(CurrentDirectory);
                            }

                            foreach (DataRow dr in objDataTable.Rows)
                            {

                                FileName = CurrentDirectory + "\\PR_" + URN + "_" + FileCounter + ".pdf";
                                FileCounter++;
                                String SignatureFile = SignatureImagePath + "\\" + dr["SIG_NAME"].ToString();
                                byte[] SignatureImage = File.ReadAllBytes(SignatureFile);

                                String LogoImageFile = LogoImagePath + "\\" + dr["LOGO_NAME"].ToString();
                                byte[] LogoImage = File.ReadAllBytes(LogoImageFile);

                                GeneratePaymentReceipt(FileName, SignatureImage, LogoImage, dr["PRVD_CUSTOMER_NAME"].ToString(),
                                    dr["PRVD_CUSTOMER_ADDRESS"].ToString(), dr["PRVD_CUSTOMER_GST_NO"].ToString(), dr["PRVD_CONSIGNEE_NAME"].ToString(),
                                    dr["PRVD_CONSIGNEE_ADDRESS"].ToString(), dr["PRVD_CONSIGNEE_GST_NO"].ToString(), dr["PRVD_RECEIPT_NO"].ToString(),
                                   Convert.ToDateTime(dr["PRVD_RECEIPT_DATE"]), dr["PRVD_PROJECT_CODE"].ToString(), dr["PRVD_MODE_OF_PAYMENT"].ToString(),
                                    dr["PRVD_ORDER_NO"].ToString(), Convert.ToString(dr["PRVD_ORDER_DATE"]), dr["PRVD_TRANSACTION_NO"].ToString(), Convert.ToDateTime(dr["PRVD_TRANSACTION_DATE"]),
                                    dr["PRVD_TERMS_OF_DELIVERY"].ToString(), dr["PRVD_TRANSACTION_NARRATION"].ToString(), dr["PRVD_DESCRIPTION"].ToString(),
                                    dr["PRVD_BATCH_ID"].ToString(), dr["PRVD_URN"].ToString(), Convert.ToDecimal(dr["PRVD_AMOUNT"]), Convert.ToDecimal(dr["PRVD_CGST_PERCENT"]),
                                    Convert.ToDecimal(dr["PRVD_CGST_AMOUNT"]), Convert.ToDecimal(dr["PRVD_SGST_PERCENT"]), Convert.ToDecimal(dr["PRVD_SGST_AMOUNT"]), Convert.ToDecimal(dr["PRVD_IGST_PERCENT"]),
                                    Convert.ToDecimal(dr["PRVD_IGST_AMOUNT"]), Convert.ToDecimal(dr["PRVD_CESS_PERCENT"]), Convert.ToDecimal(dr["PRVD_CESS_AMOUNT"]), Convert.ToDecimal(dr["PRVD_TOTAL_AMOUNT"]),
                                    dr["PRVD_NSEIT_GST_NO"].ToString(), dr["PRVD_NSEIT_PAN_NO"].ToString(), dr["PRVD_NEFT_BENEFICIARY_NAME"].ToString(),
                                    dr["PRVD_NEFT_BANK_NAME"].ToString(), dr["PRVD_NEFT_BANK_AC_NO"].ToString(), dr["PRVD_NEFT_BANK_ADDRESS"].ToString(),
                                    dr["PRVD_NEFT_BRANCH_CODE"].ToString(), dr["PRVD_NEFT_IFSC_CODE"].ToString(), dr["PRVD_NEFT_MICR_CODE"].ToString());

                                RetVal = "SUCCESS";

                                //Response.Clear();
                                //Response.Buffer = true;
                                //Response.AddHeader("Content-Disposition", "attachment; filename=" + "HT_" + dr["chrRollNumber"].ToString() + ".pdf");
                                //Response.TransmitFile(OutputPath + "HT_" + dr["chrRollNumber"].ToString() + ".pdf");
                                //Response.Flush();
                                //Response.End();

                            }
                            //converting folder to zip
                            OutputFile = OutputPath + "\\" + GuidName + ".zip";
                            ZipTheDirectory(CurrentDirectory, OutputFile);


                        }
                        /******************************/
                    }
                    else
                    {
                        RetVal = "NO_DATA_FOUND";
                    }
                }
                else
                {
                    RetVal = Error;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objUrn = null;
                objDataSet = null;
                objDataTable = null;
            }
            return RetVal;
        }
        private void GeneratePaymentReceipt(String OutputFolderWithFile, Byte[] SignatureImage, Byte[] logoImage,
            String CustomerName, String CustomerAddress, String CustomerGstNo, String ConsigneeName, String ConsigneeAddress,
            String ConsigneeGstNo, String ReceiptNo, DateTime ReceiptDate, String ProjectCode, String ModeOfPayment, String OrderNo,
            String OrderDate, String TransactionNo, DateTime TransactionDate, String TermsOfDelivery, String TransactionNarration, String Description,
            String BatchId, String URN, Decimal Amount, Decimal CgstPer, Decimal CgstAmt, Decimal SgstPer, Decimal SgstAmt,
            Decimal IgstPer, Decimal IgstAmt, Decimal CessPer, Decimal CessAmt, Decimal TotalAmount, String NseitGstNo, String NseitPanNo,
            String NeftBeneficiaryName, String NeftBankName, String NeftBankAccountNo, String NeftBankAddress, String NeftBranchCode,
            String NeftIfscCode, String NeftMicrCode)
        {
            Document document = null;
            PdfWriter writer = null;
            System.IO.FileStream fs = null;

            try
            {

                iTextSharp.text.Font f = FontFactory.GetFont("HELVETICA", 9, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font f8 = FontFactory.GetFont("HELVETICA", 8, iTextSharp.text.Font.ITALIC);
                iTextSharp.text.Font f8bi = FontFactory.GetFont("HELVETICA", 8, iTextSharp.text.Font.BOLDITALIC);
                iTextSharp.text.Font fn = FontFactory.GetFont("HELVETICA", 9, iTextSharp.text.Font.NORMAL);
                iTextSharp.text.Font fu = FontFactory.GetFont("HELVETICA", 9, iTextSharp.text.Font.UNDERLINE);
                iTextSharp.text.Font fsn = FontFactory.GetFont("HELVETICA", 6, iTextSharp.text.Font.NORMAL);
                iTextSharp.text.Font fh = FontFactory.GetFont("HELVETICA", 16, iTextSharp.text.Font.BOLD);

                fs = new FileStream(OutputFolderWithFile, FileMode.Create);
                //document = new Document(PageSize.A4, 25, 25, 30, 30);
                document = new Document(PageSize.A4);
                // Create an instance to the PDF file by creating an instance of the PDF 
                // Writer class using the document and the filestrem in the constructor. 

                writer = PdfWriter.GetInstance(document, fs);

                // Open the document to enable you to write to the document 
                document.Open();
                PdfPTable table1 = new PdfPTable(new float[] { 4f, 6f });
                table1.WidthPercentage = 100;
                PdfPTable table2 = new PdfPTable(new float[] { 3f, 4f, 3f });
                table2.WidthPercentage = 100;
                PdfPCell cell = new PdfPCell();
                Phrase phrase = new Phrase();
                PdfPCell Headercell = new PdfPCell();
                Headercell.Border = 0;
                cell.Border = 0;
                cell.HorizontalAlignment = 0;
                iTextSharp.text.Image i = iTextSharp.text.Image.GetInstance(logoImage);
                //i.ScalePercent(100f);
                i.ScaleAbsolute(85f, 42.5f);
                i.SetAbsolutePosition(36f, 750f);
                //i.Alignment = 0;
                cell.AddElement(i);
                table2.AddCell(cell);

                Chunk boldChunk = new Chunk("Receipt Voucher", fh);
                Chunk normalChunk = new Chunk("\n\n(See Rule 50 of the Central Goods and Services Tax Rules, 2017)", fsn);
                phrase.Add(boldChunk);
                phrase.Add(normalChunk);
                Headercell = new PdfPCell();
                Headercell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                Headercell.Border = 0;
                Headercell.Phrase = phrase;
                //Headercell.AddElement(new Phrase("Receipt Voucher", fh));
                //Headercell.AddElement(new Phrase("(See Rule 50 of the Central Goods and Services Tax Rules, 2017)", fsn));                
                table2.AddCell(Headercell);

                cell = new PdfPCell(new Phrase(""));
                cell.Border = 0;
                table2.AddCell(cell);
                Headercell = new PdfPCell();
                Headercell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                Headercell.Border = 0;
                Headercell.Colspan = 2;

                Headercell.AddElement(table2);
                table1.AddCell(Headercell);

                cell = new PdfPCell();
                cell.BorderWidth = 0.5f;
                cell.AddElement(new Phrase("Name and Address of Customer:", f));
                cell.AddElement(new Phrase(CustomerName, fn));
                cell.AddElement(new Phrase(CustomerAddress, fn));
                cell.AddElement(new Phrase("GST No:", f));
                cell.AddElement(new Phrase(CustomerGstNo, f));
                table1.AddCell(cell);
                PdfPCell element = new PdfPCell();
                element.Padding = 0;
                table2 = new PdfPTable(2);
                table2.WidthPercentage = 100;

                cell = new PdfPCell();
                cell.AddElement(new Phrase("Receipt No:", f));
                cell.AddElement(new Phrase(ReceiptNo, fn));
                table2.AddCell(cell);

                cell = new PdfPCell();
                cell.AddElement(new Phrase("Receipt Date:", f));
                cell.AddElement(new Phrase(ReceiptDate.ToString("dd-MMM-yyyy"), fn));
                table2.AddCell(cell);

                cell = new PdfPCell();
                cell.AddElement(new Phrase("Project Code:", f));
                cell.AddElement(new Phrase(ProjectCode, fn));
                table2.AddCell(cell);

                cell = new PdfPCell();
                cell.AddElement(new Phrase("Mode of Payment:", f));
                cell.AddElement(new Phrase(ModeOfPayment, fn));
                table2.AddCell(cell);

                cell = new PdfPCell();
                cell.AddElement(new Phrase("Order No:", f));
                cell.AddElement(new Phrase(OrderNo, fn));
                table2.AddCell(cell);

                cell = new PdfPCell();
                cell.AddElement(new Phrase("Order Date:", f));
                cell.AddElement(new Phrase(OrderDate, fn));
                table2.AddCell(cell);

                element.AddElement(table2);
                table1.AddCell(element);

                cell = new PdfPCell();
                cell.AddElement(new Phrase("Name and Address of Consignee:", f));
                cell.AddElement(new Phrase(ConsigneeName, fn));
                cell.AddElement(new Phrase(ConsigneeAddress, fn));
                cell.AddElement(new Phrase("GST No:", f));
                cell.AddElement(new Phrase(ConsigneeGstNo, fn));
                table1.AddCell(cell);

                element = new PdfPCell();
                element.Padding = 0;
                table2 = new PdfPTable(2);
                table2.WidthPercentage = 100;

                cell = new PdfPCell();
                cell.AddElement(new Phrase("Transaction Date:", f));
                cell.AddElement(new Phrase(TransactionDate.ToString("dd-MMM-yyyy"), fn));
                table2.AddCell(cell);

                cell = new PdfPCell();
                cell.AddElement(new Phrase("Transaction Reference No:", f));
                cell.AddElement(new Phrase(TransactionNo, fn));
                table2.AddCell(cell);

                cell = new PdfPCell();
                cell.AddElement(new Phrase("Terms of Delivery:", f));
                cell.AddElement(new Phrase(TermsOfDelivery, fn));
                cell.Colspan = 2;
                cell.MinimumHeight = 45;
                table2.AddCell(cell);

                element.AddElement(table2);
                table1.AddCell(element);
                cell = new PdfPCell();
                cell.Colspan = 2;
                cell.Border = 0;
                cell.AddElement(new Phrase("\n"));
                table1.AddCell(cell);
                table2 = new PdfPTable(new float[] { 1f, 3f, 2f, 2f, 2f });
                table2.WidthPercentage = 100;
                cell.MinimumHeight = 40.0f;

                table2.AddCell(new Phrase("Sr. No.", f));
                table2.AddCell(new Phrase("Description", f));
                table2.AddCell(new Phrase("Batch ID", f));
                table2.AddCell(new Phrase("URN", f));
                table2.AddCell(new Phrase("Total Value (INR)", f));

                table2.AddCell(new Phrase("1", fn));
                phrase = new Phrase();
                normalChunk = new Chunk(Description, fn);
                phrase.Add(normalChunk);
                table2.AddCell(phrase);
                table2.AddCell(new Phrase(BatchId, fn));
                table2.AddCell(new Phrase(URN, fn));
                PdfPCell cell2 = new PdfPCell();
                cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell2.Phrase = new Phrase(Amount.ToString("0.00"), fn);
                table2.AddCell(cell2);
                table2.AddCell(new Phrase(" ", fn));
                cell2 = new PdfPCell();
                phrase = new Phrase();
                boldChunk = new Chunk("\nTransaction Narration: ", f);
                phrase.Add(boldChunk);
                normalChunk = new Chunk(TransactionNarration, fn);
                phrase.Add(normalChunk);
                cell2.Phrase = new Phrase(phrase);

                cell2.Colspan = 4;
                table2.AddCell(cell2);
                element = new PdfPCell();
                element.Padding = 0;
                element.AddElement(table2);
                element.Colspan = 2;
                element.BorderWidthBottom = 0;
                table1.AddCell(element);
                cell = new PdfPCell();
                cell.Colspan = 3;
                cell.Border = 0;
                table2.AddCell(cell);
                PdfPTable table3 = new PdfPTable(1);
                table3 = new PdfPTable(1);
                table3.WidthPercentage = 100;
                table3.AddCell(new Phrase("Total", f));
                table3.AddCell(new Phrase("CGST " + CgstPer + "%", f));
                table3.AddCell(new Phrase("SGST " + SgstPer + "%", f));
                table3.AddCell(new Phrase("IGST " + IgstPer + "%", f));
                table3.AddCell(new Phrase("Cess " + CessPer + "%", f));
                table3.AddCell(new Phrase("Total Receipt Amount", f));
                element = new PdfPCell();
                element.Padding = 0;
                element.AddElement(table3);
                table2.AddCell(element);
                table3 = new PdfPTable(1);
                table3.WidthPercentage = 100;
                cell2 = new PdfPCell();
                cell2.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell2.Phrase = new Phrase(Amount.ToString("0.00"), fn);
                table3.AddCell(cell2);
                cell2.Phrase = new Phrase(CgstAmt.ToString("0.00"), fn);
                table3.AddCell(cell2);
                cell2.Phrase = new Phrase(SgstAmt.ToString("0.00"), fn);
                table3.AddCell(cell2);
                cell2.Phrase = new Phrase(IgstAmt.ToString("0.00"), fn);
                table3.AddCell(cell2);
                cell2.Phrase = new Phrase(CessAmt.ToString("0.00"), fn);
                table3.AddCell(cell2);
                cell2.Phrase = new Phrase(TotalAmount.ToString("0.00"), fn);
                table3.AddCell(cell2);
                element = new PdfPCell();
                element.Padding = 0;
                element.AddElement(table3);
                table2.AddCell(element);
                table3 = new PdfPTable(new float[] { 6f, 4f });
                table3.WidthPercentage = 100;
                cell = new PdfPCell();
                cell.Border = 0;
                cell.Colspan = 2;

                phrase = new Phrase();
                boldChunk = new Chunk("Amount (In Words): ", f);
                phrase.Add(boldChunk);
                normalChunk = new Chunk(" " + NumberToString(TotalAmount) + " \n", fn);
                phrase.Add(normalChunk);
                cell.AddElement(phrase);
                table3.AddCell(cell);

                cell = new PdfPCell();
                cell.Border = 0;
                cell.Colspan = 2;
                phrase = new Phrase();
                boldChunk = new Chunk("GST No: ", f);
                phrase.Add(boldChunk);
                normalChunk = new Chunk(" " + NseitGstNo + " \n", fn);
                phrase.Add(normalChunk);
                cell.AddElement(phrase);
                table3.AddCell(cell);

                cell = new PdfPCell();
                cell.Border = 0;
                cell.Colspan = 2;
                phrase = new Phrase();
                boldChunk = new Chunk("PAN No: ", f);
                phrase.Add(boldChunk);
                normalChunk = new Chunk(" " + NseitPanNo + " \n\n\n", fn);
                phrase.Add(normalChunk);
                cell.AddElement(phrase);
                table3.AddCell(cell);

                cell = new PdfPCell();
                cell.Border = 0;
                table2 = new PdfPTable(new float[] { 3f, 6f });
                table2.WidthPercentage = 100;
                cell2 = new PdfPCell();
                cell2.Border = 0;
                cell2.Phrase = new Phrase("NEFT Details:", f);
                table2.AddCell(cell2);
                cell2.Phrase = new Phrase("");
                table2.AddCell(cell2);
                cell2.Phrase = new Phrase("Beneficiary Name:", f);
                table2.AddCell(cell2);
                cell2.Phrase = new Phrase(NeftBeneficiaryName, fn);
                table2.AddCell(cell2);

                cell2.Phrase = new Phrase("Bank Name:", f);
                table2.AddCell(cell2);
                cell2.Phrase = new Phrase(NeftBankName, fn);
                table2.AddCell(cell2);

                cell2.Phrase = new Phrase("Bank A/c No:", f);
                table2.AddCell(cell2);
                cell2.Phrase = new Phrase(NeftBankAccountNo, fn);
                table2.AddCell(cell2);
                cell2.Phrase = new Phrase("Bank Address:", f);
                table2.AddCell(cell2);
                cell2.Phrase = new Phrase(NeftBankAddress, fn);
                table2.AddCell(cell2);
                cell2.Phrase = new Phrase("Bank Branch Code:", f);
                table2.AddCell(cell2);
                cell2.Phrase = new Phrase(NeftBranchCode, fn);
                table2.AddCell(cell2);
                cell2.Phrase = new Phrase("IFSC Code:", f);
                table2.AddCell(cell2);
                cell2.Phrase = new Phrase(NeftIfscCode, fn);
                table2.AddCell(cell2);
                cell2.Phrase = new Phrase("MICR Code:", f);
                table2.AddCell(cell2);
                cell2.Phrase = new Phrase(NeftMicrCode, fn);
                table2.AddCell(cell2);
                cell.AddElement(table2);
                table3.AddCell(cell);
                table2 = new PdfPTable(new float[] { 4f, 6f });
                cell2 = new PdfPCell();
                cell2.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right            
                cell2.Border = 0;
                cell2.Colspan = 0;
                cell2.Padding = 0;
                cell2.Phrase = new Phrase("");
                table2.AddCell(cell2);

                cell2.Phrase = new Phrase("For NSEIT Limited\n\n", f);
                table2.AddCell(cell2);
                cell2.Phrase = new Phrase("");
                table2.AddCell(cell2);
                i = iTextSharp.text.Image.GetInstance(SignatureImage);
                //i.ScalePercent(70f);
                i.ScaleAbsolute(100f, 50f);
                i.Alignment = Image.ALIGN_CENTER;

                cell2.AddElement(i);
                cell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table2.AddCell(cell2);
                cell2.Phrase = new Phrase("");
                table2.AddCell(cell2);
                cell2.Phrase = new Phrase("Authorized Signatory", f);
                table2.AddCell(cell2);
                cell2.Border = 0;
                cell2.Phrase = new Phrase("");
                cell2.Colspan = 2;
                table2.AddCell(cell2);
                element = new PdfPCell();
                element.Border = 0;
                element.AddElement(table2);
                table3.AddCell(element);
                cell2 = new PdfPCell();
                cell2.Border = 0;
                cell2.Padding = 0;
                cell2.Colspan = 2;
                cell2.Phrase = new Phrase("\n\nIf the transaction covered by this invoice/bill is subject to any other tax or levy," +
                    " the customer shall reimburse NSEIT for their liability for such tax or levy, including interest and/or any " +
                    "other amounts payable in respect thereof. \n\nA certificate for tax deducted at source, if applicable, should be" +
                    " dispatched within 30 days of the payment made by you. E. & O.E\n\n", fsn);
                table3.AddCell(cell2);
                element = new PdfPCell();
                element.Colspan = 2;
                element.AddElement(table3);
                element.Border = 0;
                element.BorderWidthLeft = 0.5f;
                element.BorderWidthRight = 0.5f;
                element.BorderWidthBottom = 0.5f;
                element.BorderWidthTop = 0;
                table1.AddCell(element);

                document.Add(table1);
                document.Close();

                // Close the writer instance 
                writer.Close();
                // Always close open filehandles explicity
                fs.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
                fs = null;
            }
        }

        public static void ZipTheDirectory(String DirectoryPathAndName, String ZipFilePathAndName)
        {
            ZipFile.CreateFromDirectory(DirectoryPathAndName, ZipFilePathAndName, CompressionLevel.Optimal, false);
        }
    }
}
