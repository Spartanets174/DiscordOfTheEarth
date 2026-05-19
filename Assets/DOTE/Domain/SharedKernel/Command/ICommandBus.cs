namespace DOTE.SharedKernel.Domain
{
    public interface ICommandBus
    {
        public void Execute(ICommand command);
    }
}