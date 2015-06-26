using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceContractFormsMgmt.Entity
{
    public class Customer
    {
        public const string DB_TABLE_NAME = "customer";
        public const string DB_ID = "id";
        public const string CUSTOMER_NAME = "customer_company_name";

        public int id;
        public string customerName;

        public static Customer Parse(Dictionary<string, string> databaseSelectDictionary)
        {
            var Customer = new Customer();
            if (databaseSelectDictionary.ContainsKey(DB_ID))
            {
                Customer.id = Int32.Parse(databaseSelectDictionary[DB_ID]);
            }
            if (databaseSelectDictionary.ContainsKey(CUSTOMER_NAME))
            {
                Customer.customerName = databaseSelectDictionary[CUSTOMER_NAME];
            }
            return Customer;
        }
    }
}
