using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using Network.Computer.Enumerate;
using Network.Computer.Enumerate.DL;
using WakeOnLAN.DAL;
using WakeOnLAN.Models.Save;

namespace WakeOnLAN.Repository.Wol
{
    public class WolRepository : RepositoryBase, IWolRepository
    {
        public List<KeyValuePair<string, List<HostResult>>> GetAllHostList()
        {
            List<KeyValuePair<string, List<HostResult>>> result = new List<KeyValuePair<string, List<HostResult>>>();

            using (var dbContext = new DBCntxt(GetConnectType()))
            {
                using (var command = dbContext.CreateCommand("wol.GetAllHostList"))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        string prevName = string.Empty; string name;
                        List<HostResult> ipMacList = null;

                        while (!reader.Read())
                        {
                            name = reader.GetString(0);
                            if (name != prevName)
                            {
                                ipMacList = new List<HostResult>();
                                result.Add(new KeyValuePair<string, List<HostResult>>(name, ipMacList));
                                prevName = name;
                            }
                            if (null != ipMacList) ipMacList.Add(new HostResult() { IpAddress = reader["IpAddress"].ToString(), MACAddress = reader["MACAddress"].ToString() });

                        }
                    }
                }
            }
            return result;
        }


        public List<KeyValuePair<string, List<HostResult>>> GetHostListPaged(string hostName, int pageNo, int pagesize, out int total)
        {
            List<KeyValuePair<string, List<HostResult>>> result = new List<KeyValuePair<string, List<HostResult>>>();

            using (var dbContext = new DBCntxt(GetConnectType()))
            {
                using (var command = dbContext.CreateCommand("wol.GetHostListPaged"))
                {
                    command.AddParameter("hostName", SqlDbType.NVarChar, hostName);
                    command.AddParameter("pageNo", SqlDbType.Int, pageNo);
                    command.AddParameter("pageSize", SqlDbType.Int, pagesize);
                    command.Parameters.Add(new SqlParameter("TotalCount", SqlDbType.Int) { Direction = ParameterDirection.Output });
                    using (var reader = command.ExecuteReader())
                    {
                        string prevName = string.Empty; string name;
                        List<HostResult> ipMacList = null;
                          //var network =  new NetworkBrowser();
                        
                        while (reader.Read())
                        {
                            ipMacList = null;
                            name = reader.GetString(0);
                            PingResult pngResult = null;
                            bool online = false;
                            if (name != prevName)
                            {
                                //pngResult = network.GetPingResult(name);
                                // online = pngResult.Status == IPStatus.Success;
                                ipMacList = new List<HostResult>();
                                result.Add(new KeyValuePair<string, List<HostResult>>(name, ipMacList));
                                prevName = name;
                            }
                            // if (null != ipMacList) ipMacList.Add(new HostResult { IpAddress = reader["IpAddress"].ToString(), MACAddress = reader["MACAddress"].ToString(),Online = online,ReplyRoundtripTime = (online == true) ? pngResult.ReplyRoundtripTime : -1});
                            if (null != ipMacList) ipMacList.Add(new HostResult { IpAddress = reader["IpAddress"].ToString(), MACAddress = reader["MACAddress"].ToString()});

                        }
                    }
                    total = Convert.ToInt32(command.Parameters["TotalCount"].Value);
                }
            }
            return result;
        }


        public int GetTotalHostCount()
        {
            using (var dbContext = new DBCntxt(GetConnectType()))
            {
                using (var command = dbContext.CreateCommand("wol.GetTotalHostCount"))
                {
                    command.AddParameter("Total", SqlDbType.Bit, ParameterDirection.Output);
                    command.ExecuteNonQuery();
                    return Convert.ToInt32(command.Parameters["Total"].Value);
                }
            }
        }

      
    }
}