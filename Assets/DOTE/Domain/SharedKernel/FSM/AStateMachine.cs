using System;
using System.Collections.Generic;
using System.Linq;

namespace DOTE.SharedKernel.Domain
{
    public class AStateMachine
    {
        public IState state { get; protected set; }
        public List<IState> states { get; protected set; } = new List<IState>();

        public event Action OnStateChanged;

        public void AddState(IState state)
        {
            if (!states.Any(x => x.Name == state.Name))
            {
                states.Add(state);
            }
        }

        public void SetState(string name)
        {
            SetState(states.First(x => x.Name == name));
        }

        public void SetState(Type type)
        {
            SetState(states.First(x => x.GetType() == type));
        }

        public void SetState(IState state)
        {
            if (state == null)
            {
                return;
            }
            this.state?.Exit();
            this.state = state;
            this.state?.Enter();
            OnStateChanged?.Invoke();
        }
    }
}
