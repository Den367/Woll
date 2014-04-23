using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace WakeOnLAN.NetworkStuff
{
    public class MagicPacketSender
    {
        public void SendMagicPacket(string macAddress)
        {
            using (UdpClient udpClient = new UdpClient())
            {
                byte[] mac = StrToMac(macAddress);
                udpClient.Send(mac, mac.Length, new IPEndPoint(IPAddress.Broadcast, 9));
            }
        }

        byte[] StrToMac(string s)
        {
            var arr = new List<byte>(102);

            string[] macs = s.Split(' ', ':', '-');

            for (int i = 0; i < 6; i++) arr.Add(0xff);

            for (int j = 0; j < 16; j++)
                for (int i = 0; i < 6; i++)
                    arr.Add(Convert.ToByte(macs[i], 16));

            return arr.ToArray();
        }
    }
}