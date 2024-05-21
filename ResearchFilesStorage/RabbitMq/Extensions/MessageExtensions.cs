using MediatR;
using RabbitMq.Messages;

namespace RabbitMq.Extensions;

public class HandleMessage<TMessage> : IRequest
     where TMessage : BaseQueueMessage
{
    public TMessage? Message { get; set; }
}

public static class HandleMessageExtensions
{
    public static dynamic CreateHandleMessageWrapper(this BaseQueueMessage message)
    {
        var wrapperType = typeof(HandleMessage<>).MakeGenericType(message.GetType());
        var request = Activator.CreateInstance(wrapperType);
        wrapperType.GetProperty(nameof(HandleMessage<BaseQueueMessage>.Message))!.SetValue(request, message);

        return request!;
    }
}