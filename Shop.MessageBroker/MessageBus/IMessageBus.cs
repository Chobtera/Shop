namespace Shop.MessageBroker.MessageBus
{
    public interface IMessageBus
    {
        Task PublicMessge(BaseMessage message, string queueName);
    }
}
