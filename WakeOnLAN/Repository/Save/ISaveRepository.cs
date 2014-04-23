using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WakeOnLAN.Repository.Save
{
    public interface ISaveRepository
    {
        bool HostModify(string name, string ip, string mac);
    }
}
