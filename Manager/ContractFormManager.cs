using MaintenanceContractFormsMgmt.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceContractFormsMgmt.Manager
{
    public class ContractFormManager
    {
        public static List<ContractForm> GetAllContractForms(QueryHelper qh)
        {
            try
            {
                var query = QueryHelper.BuildSelectQuery(ContractForm.DB_TABLE_NAME,
                new[] { "*" }.ToList());
                var queryResult = qh.QuerySelect(query);
                return queryResult.Select(ContractForm.Parse).ToList();

            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException("Can't GetAllContractForms", ex);
            }

        }

        public static string CreateContractForm(ContractForm contractForm, QueryHelper qh)
        {
            var head =
               new[]
                {
                    ContractForm.User_ID, ContractForm.CUSTOMER_ID, ContractForm.AGREEMENT_NO,
                    ContractForm.EFFECTIVE_DATE, ContractForm.EXPIRY_DATE, ContractForm.TOTAL_CONTRACT_AMT,
                    ContractForm.CONTRACT_TYPE_ID,ContractForm.REMARK, ContractForm.STATUS_FLG
                }.ToList();
            var data = new[]
            {
                new[]
                {
                    contractForm.UserId.ToString(),contractForm.CustomerId.ToString(),contractForm.AgreementNo,
                    contractForm.EffectiveDate.ToUniversalTime().ToString("yyyy-MM-dd H:mm:ss"), contractForm.ExpiryDate.ToUniversalTime().ToString("yyyy-MM-dd H:mm:ss"), contractForm.TotalContractAmt.ToString(),
                    contractForm.ContractTypeId.ToString(),contractForm.Remarks,contractForm.StatusFlg.ToString()

                }.ToList()
            }.ToList();

            if (!string.IsNullOrWhiteSpace(contractForm.id.ToString()))
            {
                head.Add(ContractForm.DB_ID);
                data[0].Add(contractForm.id.ToString());
            }
            var query = QueryHelper.BuildBulkInsertQuery(ContractForm.DB_TABLE_NAME, head, data, true, ContractForm.DB_ID);

            return qh.QueryInsertUpdateDelete(query).ToString();

        }

        public static string ModifyContractFormWithId(string contract_id, ContractForm contractForm, QueryHelper qh)
        {
            var head =
               new[]
                {
                    ContractForm.User_ID, ContractForm.CUSTOMER_ID, ContractForm.AGREEMENT_NO,
                    ContractForm.EFFECTIVE_DATE, ContractForm.EXPIRY_DATE, ContractForm.TOTAL_CONTRACT_AMT,
                    ContractForm.CONTRACT_TYPE_ID,ContractForm.REMARK, ContractForm.STATUS_FLG
                }.ToList();
            var data = new[]
            {
                    contractForm.UserId.ToString(),contractForm.CustomerId.ToString(),contractForm.AgreementNo,
                    contractForm.EffectiveDate.ToUniversalTime().ToString("yyyy-MM-dd H:mm:ss"), contractForm.ExpiryDate.ToUniversalTime().ToString("yyyy-MM-dd H:mm:ss"), contractForm.TotalContractAmt.ToString(),
                    contractForm.ContractTypeId.ToString(),contractForm.Remarks,contractForm.StatusFlg.ToString()

            }.ToList();

            var query = QueryHelper.BuildUpdateQuery(ContractForm.DB_TABLE_NAME,
                head,
                data,
                new[] { ContractForm.DB_ID }.ToList(),
                new[] { contract_id }.ToList());
            return qh.QueryInsertUpdateDelete(query).ToString();
        }
    }
}
