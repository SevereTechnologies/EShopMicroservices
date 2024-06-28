using MediatR;

namespace BuildingBlocks.CQRS;

// use this hander when no response is required (Unit means no response)
public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit>
    where TCommand : ICommand<Unit>
{
}

// use this handler when reponse is required
public interface ICommandHandler<in TCommand, TResponse>
    : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
        where TResponse : notnull
{
}
