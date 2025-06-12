using System;
using System.Data;

namespace Databases.SQLServer
{
    /// <summary>
    /// Use this structure to send in the details of parameter size
    /// </summary>
    public struct ParamLength
    {
        private Int32 _Size;
        /// <summary>
        /// Size of the data type
        /// </summary>
        public Int32 Size
        {
            get { return _Size; }
            set { _Size = value; }
        }

        private Int32 _Precision;
        /// <summary>
        /// Precision of the numeric data type
        /// </summary>
        public Int32 Precision
        {
            get { return _Precision; }
            set { _Precision = value; }
        }

        private Int32 _Scale;
        /// <summary>
        /// Scale of the numeric data type
        /// </summary>
        public Int32 Scale
        {
            get { return _Scale; }
            set { _Scale = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Size">Size of the data type</param>
        /// <param name="Precision">Precision of the numeric data type</param>
        /// <param name="Scale">Scale of the numeric data type</param>
        public ParamLength(Int32 Size, Int32 Precision, Int32 Scale)
        {
            _Size = Size;
            _Precision = Precision;
            _Scale = Scale;
        }
    }



}