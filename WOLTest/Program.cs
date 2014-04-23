using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network.Computer.Enumerate;

namespace WOLTest
{
    class Program
    {
        static void Main(string[] args)
        {
            NetworkBrowser browser = new NetworkBrowser();
            var comps =  browser.GetAvailableHostList();
            foreach (var comp in comps)
            {
                Trace.WriteLine(string.Format("{0}    : ", comp.Key));
                foreach (var ipMacPair in comp.Value)
                {
                     Trace.Write(string.Format("{0}    ",ipMacPair));
                }
               
            }
            Console.ReadLine();
        }
    }
}
