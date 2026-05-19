namespace DOTE.SharedKernel.Domain
{
    public interface IQueryHandler
    {
        public object Handle(IQuery query);
    }

    public interface IQueryHandler<TQuery, TResponse> : IQueryHandler where TQuery : IQuery
    {
        public TResponse Handle(TQuery query);
    }
}
