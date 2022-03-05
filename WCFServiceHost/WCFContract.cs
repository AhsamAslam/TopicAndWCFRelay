using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFServiceHost
{
    [ServiceContract(Namespace = "https://sb-relay-migration.servicebus.windows.net/wcfservicerelay")]  
    public interface IWCFContract
    {
        [OperationContract]
        string DoAction(string userName);
    }

    public interface IWCFContractChannel : IWCFContract, IClientChannel { }

    public class WCFContract : IWCFContract
    {
        public string DoAction(string userName)
        {
            return "Message is recieved from: " + userName;
        }
    }
}
