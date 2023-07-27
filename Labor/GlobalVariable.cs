using System;
using System.Data;
using System.Linq;
using Epicor.Hosting;
using Erp.Tablesets;
#if USE_EF_CORE
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif

namespace GlobalVariables
{
    /// <summary>
    /// </summary>
    public static class GlobalVariable
    {
        private static string _sWhseCode;
        /// <summary>
        /// sWheseCode
        /// </summary>
        public static string sWhseCode
        {
            get
            {
                return _sWhseCode;
            }

            set
            {
                _sWhseCode = value;
            }
        }

        private static string _sBinNum;
        /// <summary>
        /// sBinNum
        /// </summary>
        public static string sBinNum
        {
            get
            {
                return _sBinNum;
            }

            set
            {
                _sBinNum = value;
            }
        }
    }
}