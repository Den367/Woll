
using System.Configuration;
using System.Diagnostics;

using WakeOnLAN.DAL;

namespace WakeOnLAN.Repository
{
    public abstract class RepositoryBase : IRepository
    {
        private string _csWol;
        protected string Wol_CS
        {
            get
            {
                if (string.IsNullOrEmpty(_csWol))
                {
                    var css = ConfigurationManager.ConnectionStrings["WOL"];

                    if (css == null || string.IsNullOrEmpty(css.ConnectionString))
                    {
                        Trace.TraceError("Не найдена ConnectionString для БД.");
                    }
                    else
                    {
                        _csWol = css.ConnectionString;
                    }
                }
                return _csWol;
            }
        }

        public IDBConnectType GetConnectType()
        {
            return new DBConnectType(Wol_CS);
        }
    }
}