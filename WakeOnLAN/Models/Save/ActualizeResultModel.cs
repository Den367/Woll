using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WakeOnLAN.Models.Save
{
    public class ActualizeResultModel
    {
        public int Discovered { get; set; }
        public int New { get; set; }
        public int Modified { get; set; }
        public int Total { get; set; }
    }
}