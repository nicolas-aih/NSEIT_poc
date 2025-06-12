using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace IIIRegistrationPortal2
{
    public class XLXporter
    {
        public static void WriteExcelOld(String FilePathAndName, DataTable dt, String[] DisplayColumns, String[] DisplayHeaders, String[] DisplayFormat)
        {
            if (DisplayColumns.Length == DisplayHeaders.Length && DisplayColumns.Length == DisplayFormat.Length)
            {
                String s = String.Empty;
                for (Int32 i = 0; i < DisplayColumns.Length; i++)
                {
                    s = DisplayColumns[i];
                    if (!dt.Columns.Contains(s))
                    {
                        throw new Exception(String.Format("The column {0} not found in data table", s));
                    }
                    else
                    {
                        dt.Columns[s].Caption = DisplayHeaders[i].Trim() == String.Empty ? dt.Columns[s].Caption : DisplayHeaders[i].Trim();
                    }
                }
                for (Int32 i = dt.Columns.Count - 1; i >= 0; i--)
                {
                    if (!DisplayColumns.Contains<string>(dt.Columns[i].ColumnName))
                    {
                        dt.Columns.RemoveAt(i);
                    }
                }
                ClosedXML.Excel.XLWorkbook xLWorkbook = new XLWorkbook();

                var worksheet = xLWorkbook.Worksheets.Add(dt, "Sheet1");

                DataColumn dc = null;
                for (Int32 i = 0; i < dt.Columns.Count; i++)
                {
                    dc = dt.Columns[i];
                    if (DisplayFormat[i] == String.Empty)
                    {
                        worksheet.Column(i + 1).Style.SetIncludeQuotePrefix();
                    }
                    else
                    {
                        worksheet.Column(i + 1).Style.NumberFormat.Format = DisplayFormat[i];
                    }
                    worksheet.Column(i + 1).AdjustToContents();
                }
                //knock of for the header row...
                worksheet.Row(1).Style.SetIncludeQuotePrefix(false);

                xLWorkbook.SaveAs(FilePathAndName);
            }
            else
            {
                throw new Exception("The array lengths are not same");
            }
        }

        public static void WriteExcel(String FilePathAndName, DataTable dt, String[] DisplayColumns, String[] DisplayHeaders, String[] DisplayFormat)
        {
            if (DisplayColumns.Length == DisplayHeaders.Length && DisplayColumns.Length == DisplayFormat.Length)
            {
                String s = String.Empty;
                for (Int32 i = 0; i < DisplayColumns.Length; i++)
                {
                    DisplayColumns[i] = DisplayColumns[i].ToUpper();
                    s = DisplayColumns[i];
                    if (!dt.Columns.Contains(s))
                    {
                        throw new Exception(String.Format("The column {0} not found in data table", s));
                    }
                    else
                    {
                        dt.Columns[s].Caption = DisplayHeaders[i].Trim() == String.Empty ? dt.Columns[s].Caption : DisplayHeaders[i].Trim();
                    }
                }
                

                for (Int32 i = dt.Columns.Count - 1; i >= 0; i--)
                {
                    if ( DisplayColumns.Contains<string>(dt.Columns[i].ColumnName.ToUpper()))
                    {
                        //null;
                    }
                    else
                    { 
                        dt.Columns.RemoveAt(i);
                    }
                }

                ClosedXML.Excel.XLWorkbook xLWorkbook = new XLWorkbook();
                var xLWorksheet = xLWorkbook.Worksheets.Add("Sheet1");

                int j = 0;

                //Set the Headers
                foreach (String header in DisplayHeaders)
                {
                    j = j + 1;
                    xLWorksheet.Cell(1, j).Value = header;
                    xLWorksheet.Cell(1, j).Style.Font.Bold = true;
                    xLWorksheet.Cell(1, j).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    xLWorksheet.Cell(1, j).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                }

                //Set the Data...
                int row = 2;
                int col = 1;

                foreach (DataRow dr in dt.Rows)
                {
                    foreach (String fld in DisplayColumns)
                    {
                        xLWorksheet.Cell(row, col).Value = dr[fld];
                        col++;
                    }
                    row++;
                    col = 1;
                }

                //Set the Format
                DataColumn dc = null;
                for (Int32 i = 0; i < dt.Columns.Count; i++)
                {
                    dc = dt.Columns[i];
                    if (DisplayFormat[i] == String.Empty)
                    {
                        xLWorksheet.Column(i + 1).Style.SetIncludeQuotePrefix();
                    }
                    else
                    {
                        xLWorksheet.Column(i + 1).Style.NumberFormat.Format = DisplayFormat[i];
                    }
                    xLWorksheet.Column(i + 1).AdjustToContents();
                }
                //knock of for the header row...
                xLWorksheet.Row(1).Style.SetIncludeQuotePrefix(false);

                xLWorkbook.SaveAs(FilePathAndName);
            }
            else
            {
                throw new Exception("The array lengths are not same");
            }
        }

        public static void WriteExcel(String FilePathAndName, DataTable dt)
        {
                String s = String.Empty;
                ClosedXML.Excel.XLWorkbook xLWorkbook = new XLWorkbook();
                var worksheet = xLWorkbook.Worksheets.Add(dt, "Sheet1");

                //DataColumn dc = null;
                //for (Int32 i = 0; i < dt.Columns.Count; i++)
                //{
                //    worksheet.Column(i + 1).AdjustToContents();
                //}

                xLWorkbook.SaveAs(FilePathAndName);
        }
    }
}
