using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Network.Computer.Enumerate;
using Network.Computer.Enumerate.DL;
using WakeOnLAN.DAL;
using WakeOnLAN.Models.Save;


namespace WakeOnLAN.Repository.Save
{
    public class SaveRepository:RepositoryBase, ISaveRepository 
    {

        public bool HostModify(string name, string ip, string mac)
        {
            using (var dbContext = new DBCntxt(GetConnectType()))
            {
                using (var command = dbContext.CreateCommand("wol.ActualizeHost"))
                {
                    command.AddParameter("Name", SqlDbType.NVarChar, name);
                    command.AddParameter("IpAddress", SqlDbType.NVarChar, ip);
                    command.AddParameter("MacAddress", SqlDbType.NVarChar, mac);
                    command.Parameters.Add(new SqlParameter("New", SqlDbType.Bit) { Direction = ParameterDirection.Output });
                              
                    command.ExecuteNonQuery();
                    return Convert.ToBoolean(command.Parameters["New"].Value);
                }
            }
        }

      

        public int GetTotalHostCount()
        {
            using (var dbContext = new DBCntxt(GetConnectType()))
            {
                using (var command = dbContext.CreateCommand("wol.GetTotalHostCount"))
                {
                    command.Parameters.Add(new SqlParameter("Total", SqlDbType.Int) { Direction = ParameterDirection.Output });                   
                    command.ExecuteNonQuery();
                    return Convert.ToInt32(command.Parameters["Total"].Value);
                }
            }
        }

        public ActualizeResultModel ActualizeHostList()
        {
           
           var hosts =  new NetworkBrowser().GetAvailableHostList();
            
            int newHostCount = 0;
            int updatedHostCount = 0;
            foreach (var host in hosts)
            {

                foreach (var keyValuePair in host.Value)
                {
                    var mac = keyValuePair.MACAddress;
                    var ip = keyValuePair.IpAddress;
                    if (mac != null && ip != null && ip.Length > 5)
                    {
                        if (HostModify(host.Key,ip , mac)) newHostCount++;
                        else updatedHostCount++;
                    }
                }
            }
            return new ActualizeResultModel() { Discovered = newHostCount + updatedHostCount,New = newHostCount, Modified = updatedHostCount, Total = GetTotalHostCount() };
        }
    }


}