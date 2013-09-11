using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fish.MovieManager.NetTest
{
    public class Class1
    {
        private static Class1 _instance = new Class1();
        public static Class1 Instance { get { return _instance; } }

        /// <summary>
        /// 网络连接状态测试
        /// </summary>
        /// <returns>网络连接是否畅通</returns>
        public bool NetTest()
        {
            string hostName = "www.baidu.com".Trim();
            try
            {
                System.Net.IPHostEntry host = System.Net.Dns.GetHostEntry(hostName);
                if (host.AddressList.GetValue(0).ToString() != string.Empty)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
