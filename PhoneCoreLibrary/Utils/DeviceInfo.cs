using System;
using System.Net;
using System.Windows;
using Microsoft.Phone.Info;
using Microsoft.Phone.Net.NetworkInformation;

namespace PhoneCoreLibrary.Utils
{
    public class DeviceInfo
    {
        /// <summary>
        /// 运行商名称
        /// </summary>
        public static string OperatorName
        {
            get
            {
                string strOperName = DeviceNetworkInformation.CellularMobileOperator;
                if (!string.IsNullOrEmpty(strOperName))
                {
                    return strOperName;
                }
                else
                {
                    return "未知";
                }
            }
        }
        /// <summary>
        /// 网络名称
        /// </summary>
        public static string Netname { get; set; }
        /// <summary>
        /// 获取设备ID
        /// </summary>
        /// <returns>设备ID</returns>
        public static string GetDeviceUniqueId()
        {
            byte[] result = null;
            object uniqueId;
            if (Microsoft.Phone.Info.DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))
            {
                result = (byte[])uniqueId;
            }
            return result != null ? Convert.ToBase64String(result) : null;
        }

        /// <summary>
        /// 网络状态是否可用
        /// </summary>
        public static bool NetworkIsAvailable
        {
            get { return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable(); }
        }

        /// <summary>
        /// 获取网络状态
        /// </summary>
        /// <returns>NetworkState</returns>
        public static NetworkState GetNetStates()
        {
            var info = NetworkInterface.NetworkInterfaceType;

            switch (info)
            {
                case NetworkInterfaceType.MobileBroadbandCdma:
                    return NetworkState.CDMA;
                case NetworkInterfaceType.MobileBroadbandGsm:
                    return NetworkState.GSM;
                case NetworkInterfaceType.Wireless80211:
                    return NetworkState.WiFi;
                case NetworkInterfaceType.Ethernet:
                    return NetworkState.Ethernet;
                case NetworkInterfaceType.None:
                    return NetworkState.None;
                default:
                    return NetworkState.Other;
            }
        }

        /// <summary>
        /// 获取网络名称
        /// </summary>
        /// <param name="a"></param>
        public static void GetNetName(Action a)
        {
            DeviceNetworkInformation.ResolveHostNameAsync(
                new DnsEndPoint("www.baidu.com", 80),
                handle =>
                {
                    NetworkInterfaceInfo info = handle.NetworkInterface;
                    if (info != null)
                    {
                        switch (info.InterfaceType)
                        {
                            case NetworkInterfaceType.Ethernet:
                                Netname = "Ethernet";
                                break;
                            case NetworkInterfaceType.MobileBroadbandCdma:
                            case NetworkInterfaceType.MobileBroadbandGsm:
                                switch (info.InterfaceSubtype)
                                {
                                    case NetworkInterfaceSubType.Cellular_3G:
                                    case NetworkInterfaceSubType.Cellular_EVDO:
                                    case NetworkInterfaceSubType.Cellular_EVDV:
                                    case NetworkInterfaceSubType.Cellular_HSPA:
                                        Netname = "3G";
                                        break;
                                    case NetworkInterfaceSubType.Cellular_GPRS:
                                    case NetworkInterfaceSubType.Cellular_EDGE:
                                    case NetworkInterfaceSubType.Cellular_1XRTT:
                                        Netname = "2G";
                                        break;
                                    default:
                                        Netname = "未知";
                                        break;
                                }
                                break;
                            case NetworkInterfaceType.Wireless80211:
                                Netname = "WiFi";
                                break;
                            default:
                                Netname = "None";
                                break;
                        }
                    }
                    else
                    {
                        Netname = "None";
                    }
                    Deployment.Current.Dispatcher.BeginInvoke(a);
                }, null);
        }

        /// <summary>
        /// 设备ID
        /// </summary>
        public static string Deviceid
        {
            get
            {
                byte[] result = null;
                object uniqueId;
                if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))
                {
                    result = (byte[])uniqueId;
                }
                return result != null ? Convert.ToBase64String(result) : null;
            }
        }
        /// <summary>
        /// 操作系统，0=iPhone，1=Android，2=WindowsPhone
        /// </summary>
        public static string Platform
        {
            get { return "2"; }
        }
        /// <summary>
        /// 操作系统
        /// </summary>
        public static string Version
        {
            get
            {
                string strVersion = System.Environment.OSVersion.Version.ToString().Substring(0, 3);
                //int compareResult = string.Compare(strVersion, "7.10.7720");
                //if (compareResult >= 0)
                //{
                //    strVersion = "7.5";
                //}
                //else
                //{
                //    strVersion = "7.0";
                //}
                return strVersion;
            }
        }
        /// <summary>
        /// 手机型号
        /// </summary>
        public static string Brand
        {
            get
            {
                return DeviceStatus.DeviceManufacturer + " " + DeviceStatus.DeviceName;
            }
        }
        /// <summary>
        /// 分辨率
        /// </summary>
        public static string Resolution
        {
            get
            {
                return Application.Current.Host.Content.ActualHeight + "*" + Application.Current.Host.Content.ActualWidth;
            }
        }
    }
}
