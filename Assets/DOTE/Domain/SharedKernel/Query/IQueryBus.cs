namespace DOTE.SharedKernel.Domain
{
    public interface IQueryBus
    {
        public TResponse Ask<TResponse>(IQuery request) where TResponse : class;
    }
}
