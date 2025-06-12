using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.FileParser
{
    public class Field
    {
        private System.String _Name;
        public System.String Name
        {
            get
            {
                return _Name;
            }
        }

        private System.Int32 _Ordinal;
        public System.Int32 Ordinal
        {
            get
            {
                return _Ordinal;
            }

        }

        private System.Int32 _Length;
        public System.Int32 Length
        {
            get
            {
                return _Length;
            }
        }

        private System.Type _Datatype;
        public System.Type Datatype
        {
            get
            {
                return _Datatype;
            }
        }

        private System.String _Format;
        public System.String Format
        {
            get
            {
                return _Format;
            }
        }

        private System.Boolean _AllowNull;
        public System.Boolean AllowNull
        {
            get
            {
                return _AllowNull;
            }
        }


        private System.String _Pattern;
        public System.String Pattern
        {
            get
            {
                return _Pattern;
            }
        }


        private System.String _DisplayName;
        public System.String DisplayName
        {
            get
            {
                return _DisplayName;
            }
        }


        public Field(System.String sName, System.String sDisplayName, System.Int32 nOrdinal, System.Int32 nLength, System.Type tDataType, System.String sFormat, System.Boolean bAllowNull)
        {
            if (tDataType == typeof(System.DateTime) && sFormat == String.Empty)
            {
                throw new Exception("Date Time Fields Require The Format");
            }
            else
            {
                _Name = sName;
                _DisplayName = sDisplayName;
                _Ordinal = nOrdinal;
                _Length = nLength;
                _Datatype = tDataType;
                _Format = sFormat;
                _AllowNull = bAllowNull;

                if (tDataType == typeof(System.String))
                {
                    _Pattern = sFormat.Trim();
                }
                else
                {
                    _Pattern = TextValidator.GetPattern(_Datatype, sFormat.Trim());
                }

                System.Diagnostics.Trace.WriteLine(_Name + ":" + nOrdinal.ToString());
                System.Diagnostics.Trace.Flush();
            }
        }
    }
}
