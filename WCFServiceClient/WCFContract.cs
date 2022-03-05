using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFServiceClient
{
    internal class WCFContract
    {
        [ServiceContract(Namespace = "https://sb-relay-migration.servicebus.windows.net/wcfservicerelay")]
        public interface IWCFContract
        {
            [OperationContract]
            string DoAction(string userName);
        }

        public interface IWCFContractChannel : IWCFContract, IClientChannel { }
    }
}
