using System;
using System.Data.SqlClient;
using Biker_Keeper_Data.Abstract;

namespace Core.Entity.Procedures
{
    public class EntityProc : IEntityProc
    {
        private readonly string query;
        private readonly SqlParameter[] pars;

        public EntityProc(string Query, SqlParameter[] Pars)
        {
            query = Query;
            pars = Pars;
        }

        public SqlParameter[] GetParams()
        {
            return pars;
        }

        public string GetQuery()
        {
            return query;
        }
    }
}
