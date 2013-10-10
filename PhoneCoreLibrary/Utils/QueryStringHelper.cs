using System;
using System.Collections.Generic;

namespace PhoneCoreLibrary.Utils
{
    public class QueryStringHelper
    {
        /// <summary>
        /// 将URL解析转换为名值集合.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetQueryString(Uri uri)
        {
            var result = new Dictionary<string, string>();
            if (uri == null || uri.OriginalString.Split('?').Length != 2)
                return result;

            var queryString = uri.OriginalString.Split('?')[1];
            if (!string.IsNullOrEmpty(queryString))
            {
                int count = queryString.Length;
                for (int i = 0; i < count; i++)
                {
                    int startIndex = i;
                    int index = -1;
                    while (i < count)
                    {
                        char item = queryString[i];
                        if (item == '=')
                        {
                            if (index < 0)
                            {
                                index = i;
                            }
                        }
                        else if (item == '&')
                        {
                            break;
                        }
                        i++;
                    }
                    string key;
                    string value = null;
                    if (index >= 0)
                    {
                        key = queryString.Substring(startIndex, index - startIndex);
                        value = queryString.Substring(index + 1, (i - index) - 1);
                    }
                    else
                    {
                        key = queryString.Substring(startIndex, i - startIndex);
                    }

                    result.Add(key, value);

                    if ((i == (count - 1)) && (queryString[i] == '&'))
                    {
                        result.Add(key, string.Empty);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 将查询字符串解析转换为名值集合.
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetQueryString(string queryString)
        {
            var result = new Dictionary<string, string>();

            queryString = queryString.Replace("?", "");
            if (!string.IsNullOrEmpty(queryString))
            {
                int count = queryString.Length;
                for (int i = 0; i < count; i++)
                {
                    int startIndex = i;
                    int index = -1;
                    while (i < count)
                    {
                        char item = queryString[i];
                        if (item == '=')
                        {
                            if (index < 0)
                            {
                                index = i;
                            }
                        }
                        else if (item == '&')
                        {
                            break;
                        }
                        i++;
                    }
                    string key;
                    string value = null;
                    if (index >= 0)
                    {
                        key = queryString.Substring(startIndex, index - startIndex);
                        value = queryString.Substring(index + 1, (i - index) - 1);
                    }
                    else
                    {
                        key = queryString.Substring(startIndex, i - startIndex);
                    }

                    result.Add(key, value);

                    if ((i == (count - 1)) && (queryString[i] == '&'))
                    {
                        result.Add(key, string.Empty);
                    }
                }
            }
            return result;
        }
    }
}
