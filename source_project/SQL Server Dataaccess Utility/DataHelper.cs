using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace Databases.SQLServer
{
    class DataHelper
    {
        public DataHelper() { }

        public static void WriteToFile( DataTable dt, String FilePathAndName, String Delimiter )
        {
            try
            {
                StreamWriter sw = new StreamWriter (FilePathAndName);
                for ( Int32 i = 0; i < dt.Rows.Count; i++)
                {
                    String.Join(Delimiter, dt.Rows[i].ItemArray);
                    sw.WriteLine(sw);
                }
                sw.Close();
            }
            catch(Exception ex)
            {
                
            }
        }
    }
}
