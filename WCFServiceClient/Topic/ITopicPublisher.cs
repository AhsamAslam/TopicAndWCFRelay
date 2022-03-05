using System.Threading.Tasks;

namespace WCFServiceClient.Topic
{
    public interface ITopicPublisher
    {
        Task Publish<T>(T obj);
        Task Publish(string obj);
    }
}