using DOTE.SharedKernel.Domain;
using System;
using System.Collections.Generic;

namespace DOTE.SharedKernel.Infrastructure
{
    public class CommandBus : ICommandBus
    {
        private Dictionary<Type, ICommandHandler> commandHandlersMap;

        public CommandBus(List<ICommandHandler> commandHandlers)
        {
            commandHandlersMap = new();
            foreach (var commandHandler in commandHandlers)
            {
                commandHandlersMap.TryAdd(commandHandler.GetType(), commandHandler);
            }
        }

        public void Execute(ICommand command)
        {
            ICommandHandler commandHandler = GetHandler(command);

            if (commandHandler == null)
            {
                return;
            }

            commandHandler.Handle(command);
        }


        private ICommandHandler GetHandler(ICommand command)
        {
            Type handlerType = typeof(IQueryHandler<,>).MakeGenericType(command.GetType());
            ICommandHandler queryHandler = null;
            commandHandlersMap.TryGetValue(handlerType, out queryHandler);

            return queryHandler;
        }
    }
}
