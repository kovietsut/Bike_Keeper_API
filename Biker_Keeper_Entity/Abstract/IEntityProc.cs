using System;
using System.Data.SqlClient;

namespace Biker_Keeper_Data.Abstract
{
    public interface IEntityProc : IEntityProcView
    {
		SqlParameter[] GetParams();
		string GetQuery();
    }
}
