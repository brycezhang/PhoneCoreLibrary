using System;
using System.IO;
using System.Text;

namespace PhoneCoreLibrary.Utils
{
    /// <summary>
    /// 类型转换帮助类
    /// </summary>
    public class ConvertHelper
    {
        private static readonly string[] Unit = new [] { " B", " KB", " MB", " GB", " TB" };

        public static string ConvertEncode(string strSource)
        {
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(strSource));
            StreamReader reader = new StreamReader(stream, Encoding.Unicode);
            string str = reader.ReadToEnd();
            reader.Close();
            stream.Dispose();
            reader.Dispose();
            return str;
        }

        public static bool GetBoolean(int value)
        {
            return (value > 0);
        }

        public static bool GetBoolean(string str)
        {
            bool result = false;
            bool.TryParse(str, out result);
            return result;
        }

        public static byte GetByte(string str)
        {
            byte result = 0;
            byte.TryParse(str, out result);
            return result;
        }

        public static byte[] GetBytes(string str)
        {
            if (!string.IsNullOrEmpty(str) && (str.Trim().Length != 0))
            {
                return Encoding.Unicode.GetBytes(str);
            }
            return null;
        }

        public static DateTime GetDateTime(string str)
        {
            if (string.IsNullOrEmpty(str) || (str.Trim().Length == 0))
            {
                return DateTime.Now;
            }
            DateTime now = DateTime.Now;
            DateTime.TryParse(str, out now);
            return now;
        }

        public static decimal GetDecimal(string str)
        {
            decimal result = 0M;
            decimal.TryParse(str, out result);
            return result;
        }

        public static double GetDouble(string str)
        {
            double result = 0.0;
            double.TryParse(str, out result);
            return result;
        }

        public static float GetFloat(string str)
        {
            float result = 0f;
            float.TryParse(str, out result);
            return result;
        }

        public static Guid GetGuid(string str)
        {
            if (string.IsNullOrEmpty(str) || (str.Trim().Length == 0))
            {
                return Guid.Empty;
            }
            return new Guid(str);
        }

        public static short GetInt16(string str)
        {
            short result = 0;
            short.TryParse(str, out result);
            return result;
        }

        public static int GetInt32(string str)
        {
            int result = 0;
            int.TryParse(str, out result);
            return result;
        }

        public static long GetInt64(string str)
        {
            long result = 0L;
            long.TryParse(str, out result);
            return result;
        }

        public static float GetSingle(string str)
        {
            float result = 0f;
            float.TryParse(str, out result);
            return result;
        }

        public static string GetSize(long s)
        {
            if (s < 0L)
            {
                return "0 KB";
            }
            double num = 1024.0;
            int index = 0;
            while (s > num)
            {
                index++;
                num *= 1024.0;
            }
            double num3 = (((double)s) / num) * 1024.0;
            return (num3.ToString("F") + Unit[index]);
        }

        public static string GetSize(string size)
        {
            try
            {
                return GetSize(long.Parse(size));
            }
            catch (Exception)
            {
                return size;
            }
        }

        public static string GetTime(string time)
        {
            string str;
            try
            {
                TimeSpan span2;
                DateTime time3 = new DateTime(0x7b2, 1, 1, 0, 0, 0);
                DateTime time2 = time3.AddSeconds(double.Parse(time)).ToLocalTime();
                DateTime today = DateTime.Today;
                TimeSpan span = (TimeSpan)(DateTime.Now - time2.Date);
                switch (span.Days)
                {
                    case 0:
                        span2 = (TimeSpan)(DateTime.Now - time2);
                        if (span2.Hours != 0)
                        {
                            return ((span2.Hours < 0) ? (-span2.Hours).ToString() : (span2.Hours + "小时前"));
                        }
                        if (span2.Minutes != 0)
                        {
                            break;
                        }
                        return "刚刚";

                    case 1:
                        return ("昨天 " + time2.TimeOfDay.ToString());

                    case 2:
                        return ("前天 " + time2.TimeOfDay.ToString());

                    default:
                        goto Label_013A;
                }
                int num2 = (span2.Minutes < 0) ? -span2.Minutes : span2.Minutes;
                return (num2.ToString() + "分钟前");
            Label_013A:
                str = time2.ToString();
            }
            catch (Exception)
            {
                str = time;
            }
            return str;
        }

        public static int Int_GetSize(string size)
        {
            try
            {
                int.Parse(size);
                return int.Parse(size);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static long Long_GetSize(string size)
        {
            try
            {
                long.Parse(size);
                return long.Parse(size);
            }
            catch (Exception)
            {
                return 0L;
            }
        }
        
        /// <summary>
        /// HTML特殊字符转换
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string Unicode2HTML(string html)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < html.Length; i++)
            {
                char ch = html[i];
                if (Convert.ToInt32(ch) > 0x7f)
                {
                    builder.Append("&#" + Convert.ToInt32(ch) + ";");
                }
                else
                {
                    builder.Append(ch);
                }
            }
            return builder.ToString();
        }
    }
}
