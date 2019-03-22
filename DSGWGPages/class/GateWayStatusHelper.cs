﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
namespace DSGWGPages
{
    public class GateWayStatusHelper
    {
        public int localPort, remotePort;
        public string remoteIp;
        private IPEndPoint remoteIpEndPoint, localIpEndPoint;
        private Socket socket;
        private Thread udpServerThread;
        private Thread udpClientThread;
        private int sleepTimes;
        private UdpClient udpClient;
        private readonly TssMsgHelper tmg = new TssMsgHelper();
       // public delegate void DlgOnUdpMsg(string msg);
      //  public event DlgOnUdpMsg OnUdpHeartBeat;
      //  public event DlgOnUdpMsg OnUdpVoltageInfo;
       // public event DlgOnUdpMsg OnLog;
        public delegate void DlgOnGateWayStatusInfo(GateWayStatusInfo gateWayStatusInfo);
        public event DlgOnGateWayStatusInfo OnGateWayStatusInfo;
        public class GateWayStatusInfo
        {
            public string net;
            public string power;
            public double lon;
            public double lat;
            public double voltage;
        }
        public GateWayStatusHelper(string remoteIp, int remotePort, int localPort = 4516)
        {
            this.localPort = localPort;
            this.remoteIp = remoteIp;
            this.remotePort = remotePort;
            remoteIpEndPoint = new IPEndPoint(IPAddress.Parse(remoteIp), remotePort);
            localIpEndPoint = new IPEndPoint(IPAddress.Any, localPort);
        }
        private void Log(string str)
        {
          //  str = DateTime.Now.ToString("[HH:mm:ss] ") + str;
            LogHelper.Log("GateWayStatusHelper", str);
           //  Console.WriteLine(str);
           // ConfigHelper.SetKey("NetTransStep", str);
           // OnLog(str);
        }

        public void StartWork(int sleepSecond = 5)
        {
            StopWork();
            sleepTimes = sleepSecond * 1000;
            udpClient = new UdpClient(localPort);
            udpServerThread = new Thread(StartUdpServer);
            udpServerThread.IsBackground = true;
            udpServerThread.Start();
            udpClientThread = new Thread(StartUdpRecive);
            udpClientThread.IsBackground = true;
            udpClientThread.Start();
        }
        public void StopWork()
        {
           if(udpClient!=null)   udpClient.Dispose();
            try
            {
               
                if (udpClientThread != null)
                {
                    udpClientThread.Abort();
                }
            }
            catch (Exception e)
            {

            }
            try
            {
                if (udpServerThread != null)
                {
                    udpServerThread.Abort();
                }
            }
            catch (Exception e)
            {

            }
        }
        private void StartUdpServer()
        {
          
            while (true)
            {
                try
                {
                    if (udpClient == null)
                    {
                        Thread.Sleep(sleepTimes);
                        udpClient = new UdpClient(localPort);
                        continue;
                    }
                    TssMsg tm = tmg.Msg2TssMsg(0, "task", "getheartbeat", "", null);
                    byte[] buffer = tmg.Tssmsg2byte(tm);
                    if (buffer == null)
                    {
                      //  Log("<Send> buffer is null");
                    }
                    udpClient.Send(buffer, buffer.Length, remoteIpEndPoint);
                  //  Log("<Send>" + buffer.Length + " bytes");
                }
                catch (Exception e)
                {
                  //  Log("<Send Err>" + e.ToString());
                }
                Thread.Sleep(sleepTimes);
            }
        }
        private void StartUdpRecive()
        {
            while (true)
            {
                try
                {
                    if (udpClient == null)
                    {
                        Thread.Sleep(sleepTimes);
                        continue;
                    }
                    IPEndPoint rp = new IPEndPoint(IPAddress.Any, 0);
                    byte[]  buffer = null;
                    buffer = udpClient.Receive(ref rp);
                    string thisRemoteIp = rp.Address.ToString();
                    if (thisRemoteIp == remoteIp)
                    {
                        TssMsg tm = tmg.Byte2tssmsg(buffer);
                       // Log(tm.datatype + "," + tm.functype + "," + tm.canshuqu);
                        if (tm.datatype == "Data" && tm.functype == "heartbeat")
                        {
                            string msg = tm.canshuqu;
                            string str = msg.Replace("<", "").Replace(">", "");
                            string headFunc = str.Split(':')[0];
                            if (headFunc == "heartbeat")
                            {
                                //  OnUdpHeartBeat(tm.canshuqu);
                                string[] paras = str.Split(';');
                                GateWayStatusInfo gateWayStatusInfo = new GateWayStatusInfo();
                                foreach (string kv in paras)
                                {
                                    string key = kv.Split('=')[0];
                                    string value = kv.Split('=')[1];
                                    if (key == "swnet") gateWayStatusInfo.net = value;
                                    if (key == "swpow") gateWayStatusInfo.power = value;
                                    if (key == "longitude") gateWayStatusInfo.lon = double.Parse(value);
                                    if (key == "latitude") gateWayStatusInfo.lat = double.Parse(value);
                                    if (key == "voltage")
                                    {
                                        // OnUdpVoltageInfo(value);
                                        gateWayStatusInfo.voltage = double.Parse(value);
                                    }
                                }
                                OnGateWayStatusInfo(gateWayStatusInfo);
                            }

                        }
                    }
                }
                catch (Exception e)
                {
                    Log("<Recive Err>" + e.ToString());
                }
            }

        }

        
       
    }
}
