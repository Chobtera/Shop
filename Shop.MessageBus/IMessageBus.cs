namespace Shop.MessageBus
{
    public interface IMessageBus
    {
        Task PublicMessge(BaseMessage message, string queueName);
    }
}
