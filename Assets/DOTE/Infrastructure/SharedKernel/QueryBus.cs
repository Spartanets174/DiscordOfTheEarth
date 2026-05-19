using DOTE.SharedKernel.Domain;
using System;
using System.Collections.Generic;

namespace DOTE.SharedKernel.Infrastructure
{
    public class QueryBus : IQueryBus
    {
        private Dictionary<Type, IQueryHandler> queryHandlersMap;

        public QueryBus(List<IQueryHandler> queryHandlers)
        {
            queryHandlersMap = new();
            foreach (var queryHandler in queryHandlers)
            {
                queryHandlersMap.TryAdd(queryHandler.GetType(), queryHandler);
            }
        }

        public TResponse Ask<TResponse>(IQuery request) where TResponse : class
        {
            var handler = GetHandler<TResponse>(request);

            if (handler == null)
            {
                return null;
            }

            return (TResponse)handler.Handle(request);
        }

        private IQueryHandler GetHandler<TResponse>(IQuery query)
        {
            Type[] typeArgs = { query.GetType(), typeof(TResponse) };
            Type handlerType = typeof(IQueryHandler<,>).MakeGenericType(typeArgs);
            IQueryHandler queryHandler = null;
            queryHandlersMap.TryGetValue(handlerType, out queryHandler);

            return queryHandler;
        }
    }
}
