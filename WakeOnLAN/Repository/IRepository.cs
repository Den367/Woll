using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WakeOnLAN.DAL;

namespace WakeOnLAN.Repository
{
    public interface IRepository
    {
        IDBConnectType GetConnectType();
    }
}