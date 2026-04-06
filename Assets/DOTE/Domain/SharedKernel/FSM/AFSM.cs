using System;
using System.Collections.Generic;

namespace DOTE.SharedKernel.Domain
{
    public class AFSM<TBlackboard> where TBlackboard : class
    {
        public event Action OnStateChanged;

        public TBlackboard bb { get; protected set; }
        public List<IFSMState> states { get; protected set; } = new List<IFSMState>();
        public IFSMState state { get; protected set; }

        public AFSM(TBlackboard blackboard)
        {
            this.bb = blackboard;
        }

        public object GetBlackboard() => bb;

        public void AddState(IFSMState state)
        {
            states.Add(state);

            state.SetBlackboard(bb);
        }

        public void SetState(IFSMState state)
        {
            this.state?.Exit();
            this.state = state;
            this.state?.Enter();
            OnStateChanged?.Invoke();
        }
    }
}
