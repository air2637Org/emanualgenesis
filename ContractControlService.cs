using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using log4net;
using System.Reflection;
using System.ServiceModel.Web;
using MaintenanceContractFormsMgmt.Controller;

namespace MaintenanceContractFormsMgmt
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ContractControlService" in both code and config file together.
    public class ContractControlService : IContractControlService
    {
        public Stream CreateContractForm(Stream jsonStream)
        {
            string jsonString = IOHelper.GetStringFromStream(jsonStream);
            string returnMsg = ContractFormController.CreateContractForm(jsonString);
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            return new MemoryStream(Encoding.UTF8.GetBytes(returnMsg));
        }
    }

   

   
}
