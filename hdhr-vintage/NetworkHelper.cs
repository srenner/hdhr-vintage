using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace hdhr_vintage
{
    public static class NetworkHelper
    {
        public static string GetLocalIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "";
        }

        public static int GetAvailablePort(string ip = null)
        {
            if(ip == null)
            {
                ip = GetLocalIP();
            }
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Parse(ip), 0));
            var endPoint = (IPEndPoint)socket.LocalEndPoint;
            return endPoint.Port;
        }
    }
}
