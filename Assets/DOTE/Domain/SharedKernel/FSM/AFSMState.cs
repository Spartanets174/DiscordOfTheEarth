namespace DOTE.SharedKernel.Domain
{
    public class AFSMState<TBlackboard>  where TBlackboard : class
    {
        protected TBlackboard bb { get; set; }
        public object GetBlackboard() => bb;
        public void SetBlackboard(object bb) => this.bb = (TBlackboard)bb;
        public virtual void Enter() { }
        public virtual void Exit() { }
    }
}
