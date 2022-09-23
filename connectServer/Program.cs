using System.Data.Common;
using System.Reflection;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

string localIP = string.Empty;
using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
    {
        socket.Connect("8.8.8.8", 65530);
        IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
        localIP = endPoint.Address.ToString();
    }
IPEndPoint iep = new IPEndPoint(IPAddress.Parse(localIP), 2008);//Điểm kết nối đến dịch vụ bao gồm đ/c IP và cổng
Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); 
/**
Lớp Socket cung cấp một tập hợp các phương thức và thuộc tính phong phú cho truyền thông mạng. 
Lớp Socket cho phép bạn thực hiện cả truyền dữ liệu đồng bộ và không đồng bộ bằng cách sử dụng 
bất kỳ giao thức truyền thông nào được liệt kê trong kiểu liệt kê ProtocolType .
*/
server.Bind(iep); //Liên kết socket với một điểm liên kết
server.Listen(10); //Đặt socket ở trạng thái chờ (nghe)
Console.WriteLine("Cho ket noi tu client");
Socket client = server.Accept(); //Tạo 1 socket mới cho một kết nối mới. 
Console.WriteLine("Chap nhan ket noi tu:{0}",
client.RemoteEndPoint.ToString()); //Nhận điểm kết nối
string s = "Chao ban den voi Server";
//Chuyen chuoi s thanh mang byte 
byte[] data = new byte[1024];
data = Encoding.ASCII.GetBytes(s); 
//gui nhan du lieu theo giao thuc da thiet ke 
client.Send(data, data.Length, SocketFlags.None);
while (true)
{
    data = new byte[1024];
    int recv = client.Receive(data);
    if (recv == 0) break;
    //Chuyen mang byte Data thanh chuoi va in ra man hinh 
    s = Encoding.ASCII.GetString(data, 0, recv);
    Console.WriteLine("Clien gui len:{0}", s);
    //Neu chuoi nhan duoc la Quit thi thoat 
    if (s.ToUpper().Equals("QUIT")) break;
    //Gui tra lai cho client chuoi s 
    s = s.ToUpper();
    data = new byte[1024];
    data = Encoding.ASCII.GetBytes(s);
    client.Send(data, data.Length, SocketFlags.None);
}
client.Shutdown(SocketShutdown.Both);
client.Close();
server.Close();