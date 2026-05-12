namespace DOTE.SharedKernel.Domain
{
    public interface IState
    {
        public string Name { get; }
        public void Enter();
        public void Exit();
    }
}