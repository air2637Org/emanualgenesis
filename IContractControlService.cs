using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using System.IO;

namespace MaintenanceContractFormsMgmt
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IContractControlService" in both code and config file together.
    [ServiceContract]
    public interface IContractControlService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "CreateEvent")]
        Stream CreateContractForm(Stream jsonStream);
    }
}
