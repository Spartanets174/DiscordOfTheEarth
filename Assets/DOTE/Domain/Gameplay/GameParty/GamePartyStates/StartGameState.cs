using DOTE.SharedKernel.Domain;
using System;

namespace DOTE.Gameplay.Domain.GameParty
{
    public class StartGameState : AGamePartyState
    {
        public override string Name => nameof(StartGameState);

        public AStartGameState CurrentState => ssm.state as AStartGameState;

        private SimpleStateMachine ssm;
        public event Action OnStartGameStateChanged;

        public StartGameState()
        {
            ssm = new();

            StartGameDefaultState startGameDefaultState = new StartGameDefaultState();
            StartGamePauseState startGamePauseState = new StartGamePauseState();

            ssm.AddState(startGameDefaultState);
            ssm.AddState(startGamePauseState);
            ssm.OnStateChanged += StartGameStateChanged;
        }

        private void StartGameStateChanged()
        {
            OnStartGameStateChanged?.Invoke();
        }
    }
}