using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.ServiceModel;

namespace WCFServiceHost
{
    internal class Program
    {
        //Azure Service Bus Attributes for Topics
        
        static string connectionString = "Endpoint=sb://wcfrelaysbpoc.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=envpjW4oe1ZQcbwF0/0ImUCW+BThLKAM2f4vmlZhorI=";
        static string topicName = "topicmessages";


        //Azure Service Bus Relay attribues for WCF Relay

        static string scheme = "sb";
        static string serviceNamespace = "sb-relay-migration";
        static string servicePath = "https://sb-relay-migration.servicebus.windows.net/wcfservicerelay";
        static string policy = "RootManageSharedAccessKey";
        static string accessKey = "t9IfXNCggOsLMEWJQ3xC1KTJwvDoIC4FnvJBtOh1E7k=";


        static void Main(string[] args)
        {

            var sh = RegisterWCFRelay();
            RegisterReadTopic();
            Console.WriteLine("Enter any Key to Close the Application");
            Console.ReadKey();
            sh.Close();
        }
        static void RegisterReadTopic()
        {
            var subClient = SubscriptionClient.
                CreateFromConnectionString(connectionString, 
                topicName, "messageSubscription");

            subClient.OnMessage(m =>
            {
                Console.WriteLine(m.GetBody<string>());
            });
        }
        static ServiceHost RegisterWCFRelay()
        {
            Uri address = ServiceBusEnvironment.CreateServiceUri(scheme, serviceNamespace, servicePath);

            ServiceHost sh = new ServiceHost(typeof(WCFContract), address);

            sh.AddServiceEndpoint(typeof(IWCFContract), new NetTcpRelayBinding(), address)
                .Behaviors.Add(new TransportClientEndpointBehavior
                {
                    TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(policy, accessKey)
                });

            sh.Open();
            return sh;
        }
    }
}
