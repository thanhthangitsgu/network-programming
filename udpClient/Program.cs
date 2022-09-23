using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        string localIP = string.Empty;
        using (
            Socket socket =
                new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0)
        )
        {
            socket.Connect("8.8.8.8", 65530);
            IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
            localIP = endPoint.Address.ToString();
        }

        //Xử lí IP:
        string headIP = localIP.Substring(0, localIP.LastIndexOf("."));
        string tailIP = localIP.Substring(localIP.LastIndexOf(".") + 1);
        int int_tailIP = Convert.ToInt32(tailIP) + 1;
        String clientIP = headIP + "." + int_tailIP.ToString();

        IPEndPoint iep = new IPEndPoint(IPAddress.Parse(clientIP), 2008);
        Socket client =
            new Socket(AddressFamily.InterNetwork,
                SocketType.Dgram,
                ProtocolType.Udp);
        String s = "Chao server";
        byte[] data = new byte[1024];
        data = Encoding.ASCII.GetBytes(s);
        client.SendTo (data, iep);
        EndPoint remote = (EndPoint) iep;

        data = new byte[1024];
        int recv = client.ReceiveFrom(data, ref remote);
        s = Encoding.ASCII.GetString(data, 0, recv);
        Console.WriteLine("Nhan ve tu Server{0}", s);
        while (true)
        {
            s = Console.ReadLine();
            data = new byte[1024];
            data = Encoding.ASCII.GetBytes(s);
            client.SendTo (data, remote);
            if (s.ToUpper().Equals("QUIT")) break;
            data = new byte[1024];
            recv = client.ReceiveFrom(data, ref remote);
            s = Encoding.ASCII.GetString(data, 0, recv);
            Console.WriteLine (s);
        }
        client.Close();
    }
}
