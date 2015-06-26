using MaintenanceContractFormsMgmt.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceContractFormsMgmt.Manager
{
    public class ContractTypeManager
    {
        public static ContractType GetContractType(string contractTypeId, QueryHelper qh)
        {
            var query = QueryHelper.BuildSelectQuery(ContractType.DB_TABLE_NAME,
               new[] { "*" }.ToList(),
               new[] { ContractType.DB_ID }.ToList(),
               new[] { contractTypeId }.ToList());
            var queryResult = qh.QuerySelect(query);
            return queryResult.Select(ContractType.Parse).FirstOrDefault();
        }
    }
}
