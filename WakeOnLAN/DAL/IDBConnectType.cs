

namespace WakeOnLAN.DAL
{
    public interface IDBConnectType
    {
        string ConnectionString { get; }
        IConnect Connect { get; }
        IConnect CreateConnect();
        IConnect CreateConnect(string externalCS);
    }
}
