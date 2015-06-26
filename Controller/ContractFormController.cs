using MaintenanceContractFormsMgmt.Entity;
using MaintenanceContractFormsMgmt.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MaintenanceContractFormsMgmt.Controller
{
    public class ContractFormController
    {
        public static string CreateContractForm(string jsonString)
        {
            //convert jsonString to object
            /* {
	                "UserId" : "1",
	                "ContractTypeId" : 1,
	                "CustomerId" : 1,
	                "EffectiveDate" : "2015-06-21T18:24:18Z",
	                "ExpiryDate" : "2015-06-22T18:24:18Z",
	                "TotalContractAmt" : 200.36,
	                "Remarks" : "testing remark text"
	                "StatusFlg" : 0	
                }
             */
            ContractForm cf = null;
            try
            {
                cf = new ContractForm(jsonString);
            }
            catch (JsonException ex)
            {
                throw new Exception("Unable to convert jsonString to ContractForm object", ex);
            }

            try
            {
                string contractFormId = null;
                //validate user inputs
                string invalidType = "";

                if (IsValidCreateContractForm(cf, out invalidType))
                {
                    // save the contract form into DB
                    QueryHelper qh = new QueryHelper();

                    contractFormId = ContractFormManager.CreateContractForm(cf, qh);
                    // generate agreement no. based on DB's id
                    
                    
                    cf.AgreementNo = GenerateAgreementNo(cf, contractFormId, qh);
                    // save the agreement no. into DB

                    ContractFormManager.ModifyContractFormWithId(contractFormId,cf,qh);

                    //save everything
                    
                    qh.Commit();
                }
                // pass feedback message
                string feedbackMsg = "";

                if (!invalidType.Equals(""))
                {
                    feedbackMsg = "Fail to create contract form due to " + invalidType;
                    return feedbackMsg;
                }

                if (contractFormId == null)
                {
                    feedbackMsg = "Unable to create contract form";
                    return feedbackMsg;
                }
                feedbackMsg = "Success: contractForm Id - " + contractFormId;
                return feedbackMsg;

            }
            catch (ArgumentNullException ex)
            {
                return ex.Message;
            }
        }

        private static string GenerateAgreementNo(ContractForm cf , string cf_id, QueryHelper qh)
        {
            try
            {
                 ContractType contractType = ContractTypeManager.GetContractType(cf.ContractTypeId.ToString(), qh);
                User user = UserManager.GetUser(cf.UserId.ToString(), qh);
                Customer customer = CustomerManager.GetCustomer(cf.CustomerId.ToString(), qh);
                string temp = customer.customerName;

                // below block of code is to get the runningNo 
                return "" + contractType.contractTypeName + "/"
                    + user.userName + "/"
                    + customer.customerName + "/"
                    + "C" + cf.EffectiveDate.Year + "-"
                    + cf_id;
                
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
        }

        private static bool IsValidCreateContractForm(ContractForm cf, out string invalidType)
        {
            try
            {
                if (!isCustomerExist(cf.CustomerId))
                {
                    invalidType = "invalid customerId";
                    return false;
                }
                if (!isValidUser(cf.UserId))
                {
                    invalidType = "invalid UserId";
                    return false;
                }
                if (!isValidDatesInput(cf.EffectiveDate, cf.ExpiryDate))
                {
                    invalidType = "invalid EffectiveDate or ExpiryDate";
                    return false;
                }
                if (!isContractTypeExist(cf.ContractTypeId))
                {
                    invalidType = "invalid ContractTypeId";
                    return false;
                }
                invalidType = "";
                return true;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
        }

        public static bool isCustomerExist(int customerId) //check customerId against DB
        {
            try
            {
                var qh = new QueryHelper();
                Customer customer = null;
                customer = CustomerManager.GetCustomer(customerId.ToString(), qh);
                if (customer != null)
                {
                    return true;
                }
                return false;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }

        }

        public static bool isValidUser(int userId) //check customerId against DB
        {
            try
            {
                var qh = new QueryHelper();
                User user = null;
                user = UserManager.GetUser(userId.ToString(), qh);
                if (user != null)
                {
                    return true;
                }
                return false;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }

        }

        public static bool isValidDatesInput(DateTime effectiveDate, DateTime expiryDate) //check if expiryDate is after effectiveDate
        {
            if (effectiveDate.CompareTo(expiryDate) <= 0)
            {
                return true;
            }
            return false;
        }

        public static bool isContractTypeExist(int contractTypeId) //check contractTypeId against DB
        {
            try
            {
                var qh = new QueryHelper();
                ContractType contractType = null;
                contractType = ContractTypeManager.GetContractType(contractTypeId.ToString(), qh);
                if (contractType != null)
                {
                    return true;
                }
                return false;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }


        }
    }
}
