using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Network.Computer.Enumerate;
using Network.Computer.Enumerate.DL;
using WakeOnLAN.DAL;
using WakeOnLAN.Models.Save;

namespace WakeOnLAN.Repository.Wol
{
    public class WolRepository : RepositoryBase, IWolRepository
    {
        public List<KeyValuePair<string, List<IpAddrMACPair>>> GetAllHostList()
        {
            List<KeyValuePair<string, List<IpAddrMACPair>>> result = new List<KeyValuePair<string, List<IpAddrMACPair>>>();

            using (var dbContext = new DBCntxt(GetConnectType()))
            {
                using (var command = dbContext.CreateCommand("wol.GetAllHostList"))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        string prevName = string.Empty; string name;
                        List<IpAddrMACPair> ipMacList = null;

                        while (!reader.Read())
                        {
                            name = reader.GetString(0);
                            if (name != prevName)
                            {
                                ipMacList = new List<IpAddrMACPair>();
                                result.Add(new KeyValuePair<string, List<IpAddrMACPair>>(name, ipMacList));
                                prevName = name;
                            }
                            if (null != ipMacList) ipMacList.Add(new IpAddrMACPair() { IpAddress = reader["IpAddress"].ToString(), MACAddress = reader["MACAddress"].ToString() });

                        }
                    }
                }
            }
            return result;
        }


        public List<KeyValuePair<string, List<IpAddrMACPair>>> GetHostListPaged(string hostName, int pageNo, int pagesize, out int total)
        {
            List<KeyValuePair<string, List<IpAddrMACPair>>> result = new List<KeyValuePair<string, List<IpAddrMACPair>>>();

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
                        List<IpAddrMACPair> ipMacList = null;

                        while (reader.Read())
                        {
                            name = reader.GetString(0);
                            if (name != prevName)
                            {
                                ipMacList = new List<IpAddrMACPair>();
                                result.Add(new KeyValuePair<string, List<IpAddrMACPair>>(name, ipMacList));
                                prevName = name;
                            }
                            if (null != ipMacList) ipMacList.Add(new IpAddrMACPair() { IpAddress = reader["IpAddress"].ToString(), MACAddress = reader["MACAddress"].ToString() });

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