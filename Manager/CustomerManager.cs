using MaintenanceContractFormsMgmt.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceContractFormsMgmt.Manager
{
    public class CustomerManager
    {
        public static Customer GetCustomer(string customerId, QueryHelper qh)
        {
            try
            {
                string query = QueryHelper.BuildSelectQuery(Customer.DB_TABLE_NAME,
                        new[] { "*" }.ToList(),
                        new[] { Customer.DB_ID }.ToList(),
                        new[] { customerId }.ToList());
                var queryResult = qh.QuerySelect(query);
                return queryResult.Select(Customer.Parse).FirstOrDefault();
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception("Can't GetCustomer", ex);
            }

        }
    }
}
