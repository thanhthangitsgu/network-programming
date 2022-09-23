using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
//Lấy IP
string localIP = string.Empty;
using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
    {
        socket.Connect("8.8.8.8", 65530);
        IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
        localIP = endPoint.Address.ToString();
    }
//Xử lí IP: 
string headIP = localIP.Substring(0,localIP.LastIndexOf("."));
string tailIP = localIP.Substring(localIP.LastIndexOf(".")+1);
int int_tailIP = Convert.ToInt32(tailIP)+1;
String clientIP = headIP + "." + int_tailIP.ToString();

IPEndPoint iep = new IPEndPoint(IPAddress.Parse(clientIP), 2008);
Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
ProtocolType.Tcp);
client.Connect(iep);
byte[] data = new byte[1024];
int recv = client.Receive(data);
string s = Encoding.ASCII.GetString(data, 0, recv);
Console.WriteLine("Server gui:{0}", s);
string input;
while (true)
{
    input = Console.ReadLine();
    //Chuyen input thanh mang byte gui len cho server 
    data = new byte[1024];
    data = Encoding.ASCII.GetBytes(input);
    client.Send(data, data.Length, SocketFlags.None);
    if (input.ToUpper().Equals("QUIT")) break;
    data = new byte[1024];
    recv = client.Receive(data);
    s = Encoding.ASCII.GetString(data, 0, recv);
    Console.WriteLine("Server gui:{0}", s);
}
client.Disconnect(true);
client.Close();