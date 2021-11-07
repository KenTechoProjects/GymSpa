using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public class ResponseCodes
    {
        public static readonly string SUCCESS = "00";
        public static readonly string NOT_PROCESSED = "01";
        public static readonly string UNSUCCESSFUL = "02";
        public static readonly string INSUFFICIENT_BAL = "03";
        public static readonly string INSUFFICIENT_INFO = "04";
        public static readonly string INVALID_ENTRY = "05";
        public static readonly string HASH_MISMATCH = "09";
        public static readonly string ERROR = "99";
        public static readonly string EMPTYFIELD = "06";
    }
}
