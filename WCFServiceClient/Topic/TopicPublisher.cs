using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFServiceClient.Topic
{
    public class TopicPublisher : ITopicPublisher
    {
        private readonly ITopicClient _topicClient;

        public TopicPublisher(ITopicClient topicClient)
        {
            _topicClient = new TopicClient(
                    ConfigurationManager.AppSettings["ServiceBusConnectionString"],
                    ConfigurationManager.AppSettings["TopicName"]
                );
        }
        public Task Publish<T>(T obj)
        {
            var objAsText = JsonConvert.SerializeObject(obj);
            var message = new Message(Encoding.UTF8.GetBytes(objAsText));
            message.UserProperties["messageType"] = typeof(T).Name;
            return _topicClient.SendAsync(message);
        }

        public Task Publish(string raw)
        {
            var message = new Message(Encoding.UTF8.GetBytes(raw));
            message.UserProperties["messageType"] = "Raw";
            return _topicClient.SendAsync(message);
        }
    }
}
