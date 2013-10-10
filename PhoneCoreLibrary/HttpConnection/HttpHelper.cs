using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PhoneCoreLibrary.HttpConnection
{
    public class HttpHelper
    {
        #region 私有成员

        private readonly IDictionary<string, string> _parameter;
        private readonly IDictionary<string, Stream> _fileParameter;

        /// <summary>
        /// 服务器端无响应事件 
        /// </summary>
        /// <remarks>通常为网络超时(HTTP CONNECTION TIMED OUT)或内部服务器错误(HTTP 500)</remarks>
        public event Action ServerNoResponse;

        #endregion

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <remarks>
        /// 默认的请求方式为GET
        /// </remarks>
        public HttpHelper()
        {
            RequestUrl = "";

            _parameter = new Dictionary<string, string>();
            _fileParameter = new Dictionary<string, Stream>();

            RequestMethod = RequestType.Get;    // 默认请求方式为GET
            TimeOutMilliSeconds = 20000;        // 默认超时时间20秒
            AcceptLanguage = "zh-CN";           // 默认语言中文
            DataContentType = DataContentType.Urlencoded;// 默认Urlencode表单数据方式
        }

        /// <summary>
        /// 表单数据类型，默认Urlencoded类型
        /// </summary>
        public DataContentType DataContentType { get; set; }

        /// <summary>
        /// 请求语言，默认中文（zh-CN）
        /// </summary>
        public string AcceptLanguage { get; set; }

        /// <summary>
        /// 超时时间，单位毫秒，默认20000毫秒
        /// </summary>
        public int TimeOutMilliSeconds { get; set; }

        /// <summary>
        /// 请求URL地址
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public RequestType RequestMethod { get; set; }

        /// <summary>
        /// 是否有文件上传
        /// </summary>
        public bool HasUploadFile
        {
            get
            {
                return _fileParameter.Any();
            }
        }

        /// <summary>
        /// 上传文件名列表
        /// </summary>
        public IList<string> UploadFileNames
        {
            get
            {
                return _fileParameter.Keys.ToList();
            }
        }

        private string _headerContentDispositionName = "file";
        /// <summary>
        /// 自定义Content-Disposition的name=\"{0}\"
        /// 格式："Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n"
        /// </summary>
        public string HeaderContentDispositionName
        {
            get
            {
                return _headerContentDispositionName;
            }
            set
            {
                _headerContentDispositionName = value;
            }

        }

        /// <summary>
        /// 追加参数
        /// </summary>
        /// <param name="key">进行追加的键</param>
        /// <param name="value">键对应的值</param>
        public HttpHelper AppendParameter(string key, string value)
        {
            if (_fileParameter.ContainsKey(key))
                throw new ArgumentException("key already exists!");

            _parameter.Add(key, value);
            return this;
        }

        /// <summary>
        /// 获取请求的完整URL
        /// </summary>
        /// <returns></returns>
        public string GetFullUrl()
        {
            var strRequestUrl = RequestUrl;
            var paramString = GetParameterString();
            if (paramString.Length > 0)
            {
                strRequestUrl += "?" + paramString;
            }

            return strRequestUrl;
        }

        /// <summary>
        /// 追加文件参数
        /// </summary>
        /// <param name="fileName">文件名，完整路径，不可重复</param>
        /// <param name="fileStream">文件流</param>
        /// <returns></returns>
        public HttpHelper AppendFileParameter(string fileName, Stream fileStream)
        {
            if (_fileParameter.ContainsKey(fileName))
                throw new ArgumentException("fileName already exists!");

            _fileParameter.Add(fileName, fileStream);
            return this;
        }

        /// <summary>
        /// 进行HTTP请求
        /// </summary>
        /// <remarks>
        /// POST上传流数据，需手动关闭
        /// </remarks>
        public async Task<string> Request()
        {
            try
            {
                if (RequestMethod == RequestType.Get && !HasUploadFile)
                {
                    return await GetRequest();
                }
                return await PostRequest();
            }
            catch (WebException ex)
            {
                return "[{'FrameworkException':'网络异常！错误信息:" + ex.Message + "'}]";
            }
            catch (Exception ex)
            {
                return "[{'FrameworkException':'未知异常！错误信息:" + ex.Message + "'}]";
            }
        }

        /// <summary>
        /// 下载文件（请求方式：GET）
        /// </summary>
        /// <param name="path">文件保存完整路径</param>
        /// <returns>True：下载成功，False：下载失败</returns>
        /// <remarks>无需设置RequestMethod属性</remarks>
        public async Task<bool> DownloadFile(string path)
        {
            var strRequestUrl = RequestUrl;
            var paramString = GetParameterString();
            if (paramString.Length > 0)
            {
                strRequestUrl += "?" + paramString;
            }

            var url = new Uri(strRequestUrl);
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "GET";

            var webResponse = await webRequest.GetResponseAsync().WithTimeout(TimeOutMilliSeconds);
            
            if (webResponse == null)
            {
                if (ServerNoResponse != null)
                    ServerNoResponse();

                return false;
            }

            using (var streamResult = webResponse.GetResponseStream()) // 获取响应流
            {
                using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (isolatedStorage.FileExists(path))
                    {
                        isolatedStorage.DeleteFile(path);
                    }

                    using (var fileStream = new IsolatedStorageFileStream(path, FileMode.Create, isolatedStorage))
                    {
                        var buffer = new byte[streamResult.Length];
                        streamResult.Read(buffer, 0, buffer.Length);
                        fileStream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
            ClearParameters(); // 清空参数列表

            return true;
        }

        /// <summary>
        /// HTTP方式的GET请求
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetRequest()
        {
            var strRequestUrl = RequestUrl;
            var paramString = GetParameterString();
            if (paramString.Length > 0)
            {
                strRequestUrl += "?" + paramString;
            }

            var url = new Uri(strRequestUrl);

            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "GET";
            webRequest.Headers[HttpRequestHeader.AcceptLanguage] = AcceptLanguage;

            var webResponse = await webRequest.GetResponseAsync().WithTimeout(TimeOutMilliSeconds);
            // 超时检查
            if (webResponse == null)
            {
                if (ServerNoResponse != null)
                    ServerNoResponse();

                return "[{'FrameworkException':'服务器返回响应流WebResponse为NULL，参考网络错误：超时或500'}]";
            }

            string result;
            using (var streamResult = webResponse.GetResponseStream()) // 获取响应流
            {
                using (var reader = new StreamReader(streamResult))
                {
                    result = reader.ReadToEnd();
                }
            }
            ClearParameters(); // 清空参数列表

            return result;
        }

        /// <summary>
        /// HTTP的POST请求
        /// </summary>
        /// <returns></returns>
        private async Task<string> PostRequest()
        {
            if (DataContentType != DataContentType.Standard && HasUploadFile)
            {
                throw new ArgumentException("DataContentType参数错误：上传文件的DataContentType必须为Standard类型！");
            }

            string result;

            var url = new Uri(RequestUrl);
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.Headers[HttpRequestHeader.AcceptLanguage] = AcceptLanguage;

            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");// 表单数据使用

            // 开始异步请求
            using (var requestStream = await webRequest.GetRequestStreamAsync())
            {
                // 数据类型为表单
                switch (DataContentType)
                {
                    case DataContentType.Standard:
                        {
                            // 1、开始边界字符串
                            var boundarybytes = Encoding.UTF8.GetBytes("--" + boundary + "\r\n");

                            // 2、添加ContentType头
                            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;

                            // 3、向请求流中写入参数
                            const string formdataTemplate =
                                "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}\r\n";
                            foreach (string key in _parameter.Keys)
                            {
                                requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                                var formitem = string.Format(formdataTemplate, key, _parameter[key]);
                                var formitembytes = Encoding.UTF8.GetBytes(formitem);
                                requestStream.Write(formitembytes, 0, formitembytes.Length);
                            }
                            requestStream.Write(boundarybytes, 0, boundarybytes.Length);

                            // 4、如果上传文件，向请求流写入文件流
                            const string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                            foreach (var fileItem in _fileParameter)
                            {
                                var fileName = fileItem.Key;
                                var fileStream = fileItem.Value;
                                var cntentType = GetContentType(fileName); // 文件类型

                                var header = string.Format(headerTemplate, HeaderContentDispositionName, fileName, cntentType);
                                var headerbytes = Encoding.UTF8.GetBytes(header);
                                requestStream.Write(headerbytes, 0, headerbytes.Length);

                                var buffer = new byte[4096];
                                int bytesRead;
                                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                                {
                                    requestStream.Write(buffer, 0, bytesRead);
                                }
                            }

                            // 5、向请求流中写入尾部边界（标准表单提交）
                            byte[] trailer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                            requestStream.Write(trailer, 0, trailer.Length);

                        }
                        break;
                    case DataContentType.Json:
                        // 1、添加ContentType头
                        webRequest.ContentType = "application/json";

                        // 2、向请求流中写入参数
                        foreach (string val in _parameter.Values)
                        {
                            var data = Encoding.UTF8.GetBytes(val);
                            requestStream.Write(data, 0, data.Length);
                        }
                        break;
                    case DataContentType.None:
                        // 向请求流中“直接”写入参数
                        foreach (string val in _parameter.Values)
                        {
                            var data = Encoding.UTF8.GetBytes(val);
                            requestStream.Write(data, 0, data.Length);
                        }
                        break;
                    case DataContentType.Urlencoded:// 默认
                        // 1、添加ContentType头
                        webRequest.ContentType = "application/x-www-form-urlencoded";

                        // 2、向请求流中写入参数
                        var parameterstring = GetParameterString();
                        if (parameterstring.Length > 0)
                        {
                            byte[] data = Encoding.UTF8.GetBytes(parameterstring);
                            requestStream.Write(data, 0, data.Length);
                        }
                        break;
                }

                #region 测试使用
                //requestStream.Position = 0;
                //var buf = new byte[requestStream.Length];
                //requestStream.Read(buf, 0, buf.Length);

                //var s = Encoding.UTF8.GetString(buf, 0, buf.Length);
                //if (s != "")
                //    s = string.Empty;
                #endregion
            }

            using (var webResponse = await webRequest.GetResponseAsync().WithTimeout(TimeOutMilliSeconds))
            {
                // 超时检查
                if (webResponse == null)
                {
                    if (ServerNoResponse != null)
                        ServerNoResponse();

                    return "[{'FrameworkException':'服务器返回响应流WebResponse为NULL，参考网络错误：超时或500'}]";
                }

                // 获取响应流
                using (var streamResult = webResponse.GetResponseStream())
                {
                    using (var reader = new StreamReader(streamResult))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }

            ClearParameters();// 清空参数列表

            return result;
        }

        /// <summary>
        /// 获取传递参数的字符串
        /// </summary>
        /// <returns>字符串</returns>
        private string GetParameterString()
        {
            var result = "";
            var sb = new StringBuilder();
            var hasParameter = false;
            foreach (var item in _parameter)
            {
                if (!hasParameter)
                    hasParameter = true;
                var value = UrlEncoder.Encode(item.Value);
                sb.Append(string.Format("{0}={1}&", item.Key, value));
            }
            if (hasParameter)
            {
                result = sb.ToString();
                int len = result.Length;
                result = result.Substring(0, --len); // 将字符串尾的‘&’去掉
            }
            return result;

        }

        /// <summary>
        /// 根据文件后缀，返回协议指定内容类型
        /// 仅限.jpg、.png、.gif后缀（根据实际需要添加类型）
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetContentType(string fileName)
        {
            var extension = fileName.Substring(fileName.LastIndexOf('.'));

            switch (extension)
            {
                case ".jpg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                default:
                    return "image/jpeg";
            }
        }

        /// <summary>
        /// 清空请求参数
        /// </summary>
        private void ClearParameters()
        {
            if (_parameter != null)
            {
                _parameter.Clear();
            }

            if (_fileParameter != null)
            {
                _fileParameter.Clear();
            }
        }
    }

    /// <summary>
    /// 枚举请求类型
    /// </summary>
    public enum RequestType
    {
        /// <summary>
        /// GET请求
        /// </summary>
        Get,

        /// <summary>
        /// POST请求
        /// </summary>
        Post
    }

    /// <summary>
    /// POST表单数据类型
    /// </summary>
    public enum DataContentType
    {
        /// <summary>
        /// Urlencode数据表单
        /// </summary>
        Urlencoded,
        /// <summary>
        /// 标准数据表单
        /// </summary>
        Standard,
        /// <summary>
        /// Json数据表单
        /// </summary>
        Json,
        /// <summary>
        /// 无数据类型表单
        /// </summary>
        None
    }
}
