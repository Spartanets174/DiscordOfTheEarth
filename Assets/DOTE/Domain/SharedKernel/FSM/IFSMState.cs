namespace DOTE.SharedKernel.Domain
{
    public interface IFSMState
    {
        public object GetBlackboard();
        public void SetBlackboard(object value);

        public void Enter();
        public void Exit();
    }
}