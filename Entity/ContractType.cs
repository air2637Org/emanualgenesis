using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceContractFormsMgmt.Entity
{
    public class ContractType
    {
        public const string DB_TABLE_NAME = "contract_type";
        public const string DB_ID = "id";
        public const string CONTRACT_TYPE_NAME = "contract_type_name";

        public int id;
        public string contractTypeName;

        public static ContractType Parse(Dictionary<string, string> databaseSelectDictionary)
        {
            var contractType = new ContractType();
            if (databaseSelectDictionary.ContainsKey(DB_ID))
            {
                contractType.id = Int32.Parse(databaseSelectDictionary[DB_ID]);
            }
            if (databaseSelectDictionary.ContainsKey(CONTRACT_TYPE_NAME))
            {
                contractType.contractTypeName = databaseSelectDictionary[CONTRACT_TYPE_NAME];
            }
            return contractType;
        }
    }
}
