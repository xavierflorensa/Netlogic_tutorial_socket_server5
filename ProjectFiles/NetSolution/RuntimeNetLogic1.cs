#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.UI;
using FTOptix.NativeUI;
using FTOptix.Core;
using FTOptix.CoreBase;
using FTOptix.NetLogic;
using System.Net.Sockets;
using System.Threading;
using System.IO.Pipes;
using System.IO;
using System.Text;
using System.Diagnostics;
#endregion
public class RuntimeNetLogic1 : FTOptix.NetLogic.BaseNetLogic
{ 
    [ExportMethod]
    public void SocketServer_listen(NodeId textboxNodeId,NodeId textboxNodeId2)
    {
        var textbox = InformationModel.Get<TextBox>(textboxNodeId);
        var textbox2 = InformationModel.Get<TextBox>(textboxNodeId2);
        textbox.Text = "A button has been pressed";
        Log.Info("A button has been pressed");
        TcpListener listener = new TcpListener(System.Net.IPAddress.Any, 2000);
        listener.Start();
        while (true)
        {
            //Console.WriteLine("Waiting for a connection.");
            textbox.Text = "Waiting for a connection.";
            Log.Info("Waiting for a connection.");
            TcpClient client = listener.AcceptTcpClient();
            //Console.WriteLine("Client accepted.");
            textbox.Text = "Client accepted.";
            Log.Info("Client accepted.");
        
            NetworkStream stream = client.GetStream();
            StreamReader sr = new StreamReader(client.GetStream());
            StreamWriter sw = new StreamWriter(client.GetStream());
           
            try
            {
                byte[] buffer = new byte[1024];
                stream.Read(buffer, 0, buffer.Length);
                int recv = 0;
                foreach (byte b in buffer)
                {
                    if (b!=0)
                    {
                        recv++;
                    }
                }
                string request = Encoding.UTF8.GetString(buffer, 0, recv);
                //Console.WriteLine("request received: "+ request);
                textbox2.Text = request;
                Log.Info("request received: "+ request);
                sw.WriteLine("You rock!");
                sw.Flush();
            }
            catch(Exception e)
            {
                //Console.WriteLine("Something went wrong.");
                Log.Info("Something went wrong.");
                sw.WriteLine(e.ToString());
            }
            System.Threading.Thread.Sleep(1000);
        }
    }
 }
