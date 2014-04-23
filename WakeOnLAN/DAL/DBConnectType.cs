

namespace WakeOnLAN.DAL
{
    public class DBConnectType : IDBConnectType
    {
        public string ConnectionString { get; private set; }

        public IConnect Connect { get; private set; }

        public IConnect CreateConnect()
        {
            return CreateConnect(null);
        }

        public IConnect CreateConnect(string externalCS)
        {
            if (!string.IsNullOrWhiteSpace(externalCS))
            {
                return new DBConnect(externalCS);
            }
            else
            {
                if (!string.IsNullOrEmpty(ConnectionString))
                {
                    return new DBConnect(ConnectionString);
                }
                else
                {
                    return Connect;
                }
            }
        }

        public DBConnectType(string connectString)
        {
            ConnectionString = connectString;
        }

        public DBConnectType(DBConnect connect)
        {
            Connect = connect;
        }
    }
}
