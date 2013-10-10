using System;
using System.Text;

namespace PhoneCoreLibrary.Utils
{
    /// <summary>
    /// 字符串帮助类
    /// </summary>
    public class StringHelper
    {
        public static string BytesToString(byte[] b)
        {
            if ((b == null) || (b.Length <= 0))
            {
                return null;
            }
            StringBuilder builder = new StringBuilder();
            foreach (byte num in b)
            {
                builder.Append(IntToHexChar(num / 0x10));
                builder.Append(IntToHexChar(num % 0x10));
            }
            return builder.ToString(0, builder.Length);
        }

        public static string GetBase64String(string source)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(source);
                return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            }
            catch (Exception)
            {
                return "";
            }
            return "";
        }

        /// <summary>
        /// 获取后缀名
        /// </summary>
        /// <param name="s">xxxx.yy</param>
        /// <returns>yy</returns>
        public static string GetExtension(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                int num = s.LastIndexOf('.');
                if (num > 0)
                {
                    return s.Substring(num + 1);
                }
            }
            return "";
        }

        public static int HexCharToInt(char a)
        {
            switch (a)
            {
                case '0':
                    return 0;

                case '1':
                    return 1;

                case '2':
                    return 2;

                case '3':
                    return 3;

                case '4':
                    return 4;

                case '5':
                    return 5;

                case '6':
                    return 6;

                case '7':
                    return 7;

                case '8':
                    return 8;

                case '9':
                    return 9;

                case 'A':
                case 'a':
                    return 10;

                case 'B':
                case 'b':
                    return 11;

                case 'C':
                case 'c':
                    return 12;

                case 'D':
                case 'd':
                    return 13;

                case 'E':
                case 'e':
                    return 14;

                case 'F':
                case 'f':
                    return 15;
            }
            return 0;
        }

        public static char IntToHexChar(int n)
        {
            switch (n)
            {
                case 0:
                    return '0';

                case 1:
                    return '1';

                case 2:
                    return '2';

                case 3:
                    return '3';

                case 4:
                    return '4';

                case 5:
                    return '5';

                case 6:
                    return '6';

                case 7:
                    return '7';

                case 8:
                    return '8';

                case 9:
                    return '9';

                case 10:
                    return 'a';

                case 11:
                    return 'b';

                case 12:
                    return 'c';

                case 13:
                    return 'd';

                case 14:
                    return 'e';

                case 15:
                    return 'f';
            }
            return 'g';
        }

        public static byte[] StringTobytes(string b)
        {
            if ((b == null) || (b.Length <= 0))
            {
                return null;
            }
            int num = b.Length / 2;
            byte[] buffer = new byte[num];
            for (int i = 0; i < num; i++)
            {
                buffer[i] = (byte)((HexCharToInt(b[2 * i]) * 0x10) + HexCharToInt(b[(2 * i) + 1]));
            }
            return buffer;
        }

        public static string Unicode8ByteToString(byte[] str)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                builder.Append("&#");
                string str2 = Convert.ToString(str[i], 0x10);
                builder.Append(str2);
                builder.Append(";");
            }
            return builder.ToString();
        }

        public static string UnicodeByteToDecString(byte[] str)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < str.Length; i += 2)
            {
                builder.Append("&#");
                string str2 = str[i + 1].ToString();
                builder.Append(str2.Equals("0") ? "00" : str2);
                string str3 = str[i].ToString();
                builder.Append(str3.Equals("0") ? "00" : str3);
                builder.Append(";");
            }
            return builder.ToString();
        }

        public static string UnicodeByteToString(byte[] str)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < str.Length; i += 2)
            {
                builder.Append("&#x");
                string str2 = Convert.ToString(str[i + 1], 0x10);
                builder.Append(str2.Equals("0") ? "00" : str2);
                string str3 = Convert.ToString(str[i], 0x10);
                builder.Append(str3.Equals("0") ? "00" : str3);
                builder.Append(";");
            }
            return builder.ToString();
        }
    }
}
