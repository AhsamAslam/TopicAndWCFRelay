using Azure.Messaging.ServiceBus;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFServiceClient.Topic;
using static WCFServiceClient.WCFContract;

namespace WCFServiceClient
{
    internal class Program
    {
        // connection string to your Service Bus namespace
        static string connectionString = "Endpoint=sb://wcfrelaysbpoc.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=envpjW4oe1ZQcbwF0/0ImUCW+BThLKAM2f4vmlZhorI=";
        static string topicName = "topicmessages";

        //WCF RELAY Configurations
        static string scheme = "sb";
        static string serviceNamespace = "sb-relay-migration";
        static string servicePath = "https://sb-relay-migration.servicebus.windows.net/wcfservicerelay";
        static string policy = "RootManageSharedAccessKey";
        static string accessKey = "t9IfXNCggOsLMEWJQ3xC1KTJwvDoIC4FnvJBtOh1E7k=";


        static void Main(string[] args)
        {
            // Registering and Sending Data to WCF Relay
            CallWCFContract();

            // Sending Data to Azure Topic
            Console.WriteLine("Please enter for sending message to your topic");
            Console.ReadKey();


            User user = new User()
            {
                UserID = 1,
                UserName = "Test User"
            };
            SendMessage(JsonConvert.SerializeObject(user));



            Console.WriteLine("Enter any Key to Close the Application");
            Console.ReadLine();

        }
        static void SendMessage(string message)
        {
            var topicClient = TopicClient.CreateFromConnectionString(connectionString, topicName);
            var msg = new BrokeredMessage(message);
            topicClient.Send(msg);
        }
        static void CallWCFContract()
        {
            ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.AutoDetect;

            var cf = new ChannelFactory<IWCFContractChannel>(new NetTcpRelayBinding(), 
                new EndpointAddress(ServiceBusEnvironment.
                CreateServiceUri(scheme, serviceNamespace, servicePath)));

            cf.Endpoint.Behaviors.Add(new TransportClientEndpointBehavior 
            { TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(policy, accessKey) });
            Console.WriteLine("Please enter message for sending it to your Relay");
            var input = Console.ReadLine();
            using (var ch = cf.CreateChannel())
            {
                Console.WriteLine(ch.DoAction(input));
            }
        }


    }
}
