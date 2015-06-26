using MaintenanceContractFormsMgmt.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceContractFormsMgmt.Manager
{
    public class UserManager
    {
        public static User GetUser(string userid, QueryHelper qh)
        {
            var query = QueryHelper.BuildSelectQuery(User.DB_TABLE_NAME,
               new[] { "*" }.ToList(),
               new[] { User.DB_ID }.ToList(),
               new[] { userid }.ToList());
            var queryResult = qh.QuerySelect(query);
            return queryResult.Select(User.Parse).FirstOrDefault();
        }
    }
}
