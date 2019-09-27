using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ProxyChecker
{
    public class Checker
    {
        private static readonly string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:68.0) Gecko/20100101 Firefox/68.0";

        public static bool ProxyCheck(string ipAddress, int port)
        {
            try
            {
                ICredentials credentials = CredentialCache.DefaultCredentials;
                IWebProxy proxy = new WebProxy(ipAddress, port);
                proxy.Credentials = credentials;

                using (var wc = new WebClient())
                {
                    wc.Proxy = proxy;
                    wc.Encoding = Encoding.UTF8;
                    wc.Headers.Add("User-Agent", UserAgent);
                    string result = wc.DownloadString("http://www.ipsorgu.com/");
                    return result.Contains(ipAddress);
                }
            }
            catch
            {
                return false;
            }

        }
    }
}
