// Copyright (C) 2016 Fsoul Inc.

using System;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace Fsoul.Net {

public class CompatibilityIP
{
#if UNITY_IPHONE
    [DllImport("__Internal")]
    private static extern string getIPv6(string host, string port);
#endif

    private static string GetIPv6(string host, string port) {
#if UNITY_IPHONE && !UNITY_EDITOR
        return getIPv6(host, port);
#endif
        return host + "&&ipv4";
    }

    public static void GetIpType(string serverIp, string serverPort, out string newServerIp, out AddressFamily newServerAddressFamily) {
        newServerAddressFamily = AddressFamily.InterNetwork;  
        newServerIp = serverIp;
        try {
            string ipv6 = GetIPv6(serverIp, serverPort);
            if(!string.IsNullOrEmpty(ipv6)) {
                string[] strTemp = Regex.Split(ipv6, "&&");
                if(strTemp.Length >= 2) {
                    string type = strTemp[1];
                    if(type == "ipv6") {
                        newServerIp = strTemp[0];
                        newServerAddressFamily = AddressFamily.InterNetworkV6;
                    }
                }
            }
        } catch (Exception) {
        }
    }
}

} // namespace Fsoul.Net