using System;

namespace DOTE.SharedKernel.Domain
{
    public abstract class AState : IState
    {
        public abstract string Name { get; }

        public event Action OnStateEnter;
        public event Action OnStateExit;

        public void Enter()
        {
            OnEnterHook();
            OnStateEnter?.Invoke();
        }

        public void Exit()
        {
            OnExitHook();
            OnStateExit?.Invoke();
        }

        protected virtual void OnEnterHook() { }

        protected virtual void OnExitHook() { }
    }
}