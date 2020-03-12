using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Security.Cryptography;

namespace Tectransit.Service.Library
{
    public static class Tools
    {
        public static void writeLog(string strTemp)
        {
            writeLog(strTemp, "Main");
        }

        public static void writeLog(string strTemp, string MissionType)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string destPath = $@"{path}Log\" + DateTime.Now.ToString("yyyyMM") + "\\";
            if (!Directory.Exists(destPath))
                Directory.CreateDirectory(destPath);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(destPath + $@"{DateTime.Now.ToString("dd")}_{MissionType}_log.txt", true))
            {
                //file.WriteLine("=======" + DateTime.Now.ToString() + "=======");
                file.WriteLine(strTemp);
            }
        }

        public static int ConvertInt(object obj)
        {
            int Reslut = 0;
            if (obj != null)
            {
                if (!String.IsNullOrEmpty(obj?.ToString()))
                    Reslut = Convert.ToInt32(obj);
            }
            return Reslut;
        }

        public static int? ConvertIntNull(object obj)
        {
            int? Reslut = null;
            if (obj != null)
            {
                if (!String.IsNullOrEmpty(obj?.ToString()))
                    Reslut = Convert.ToInt32(obj);
            }
            return Reslut;
        }

        public static Int16 ConvertShort(object obj)
        {
            Int16 Reslut = 0;
            if (obj != null)
            {
                if (!String.IsNullOrEmpty(obj?.ToString()))
                    Reslut = Convert.ToInt16(obj);
            }
            return Reslut;
        }

        public static Int16? ConvertShortNull(object obj)
        {
            Int16? Reslut = null;
            if (obj != null)
            {
                if (!String.IsNullOrEmpty(obj?.ToString()))
                    Reslut = Convert.ToInt16(obj);
            }
            return Reslut;
        }

        public static Int64 ConvertLong(object obj)
        {
            Int64 Reslut = 0;
            if (obj != null)
            {
                if (!String.IsNullOrEmpty(obj?.ToString()))
                    Reslut = Convert.ToInt64(obj);
            }
            return Reslut;
        }

        public static Int64? ConvertLongNull(object obj)
        {
            Int64? Reslut = null;
            if (obj != null)
            {
                if (!String.IsNullOrEmpty(obj?.ToString()))
                    Reslut = Convert.ToInt64(obj);
            }
            return Reslut;
        }

        public static decimal ConvertDecimal(object obj)
        {
            decimal Reslut = 0;
            if (obj != null)
            {
                if (!String.IsNullOrEmpty(obj?.ToString()))
                    Reslut = Convert.ToDecimal(obj);
            }
            return Reslut;
        }

        public static decimal? ConvertDecimalNull(object obj)
        {
            decimal? Reslut = null;
            if (obj != null)
            {
                if (!String.IsNullOrEmpty(obj?.ToString()))
                    Reslut = Convert.ToDecimal(obj);
            }
            return Reslut;
        }

        public static DateTime? ConvertDateTimeNull(object obj)
        {
            DateTime? Reslut = null;
            if (obj != null)
            {
                DateTime dt = new DateTime();
                if (DateTime.TryParse(obj.ToString(), out dt))
                {
                    Reslut = dt;
                }
                else
                {
                    string[] dateFormats = { "yyyy/MM/dd", "yyyy/MM/dd HH:mm:ss", "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss", "yyyyMMdd", "yyyyMMdd HHmmss", };
                    if (DateTime.TryParseExact(obj.ToString(), dateFormats, null, DateTimeStyles.AllowWhiteSpaces, out dt))
                    {
                        Reslut = dt;
                    }
                }
            }
            return Reslut;
        }

        public static DateTime ConvertDateTime(object obj)
        {
            DateTime Reslut = DateTime.MinValue;
            if (obj != null)
            {
                DateTime dt = new DateTime();
                if (DateTime.TryParse(obj.ToString(), out dt))
                {
                    Reslut = dt;
                }
                else
                {
                    string[] dateFormats = { "yyyy/MM/dd", "yyyy/MM/dd HH:mm:ss", "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss", "yyyyMMdd", "yyyyMMdd HHmmss", };
                    if (DateTime.TryParseExact(obj.ToString(), dateFormats, null, DateTimeStyles.AllowWhiteSpaces, out dt))
                    {
                        Reslut = dt;
                    }
                }
            }
            return Reslut;
        }

        public static string ConvertDateFormat(string val)
        {
            if (val.Length == 8)
            {
                return val.Substring(0, 4) + "-" + val.Substring(4, 2) + "-" + val.Substring(6, 2);
            }
            else
                return "";
        }

        public static string ConvertTimeFormat(string val)
        {
            if (val.Length == 6)
            {
                return val.Substring(0, 2) + ":" + val.Substring(2, 2) + ":" + val.Substring(4, 2);
            }
            else
                return "";
        }

        //MD5加密字串
        public static string GetMD5(string sData)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(Encoding.Default.GetBytes(sData));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < keys.Length; i++)
                {
                    sBuilder.Append(keys[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }
    }
}
