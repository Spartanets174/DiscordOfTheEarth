using DOTE.SharedKernel.Domain;
using System;

namespace DOTE.Gameplay.Domain.GameParty
{
    public class StartGameStateChanged: IDomainEvent
    {
        private Type startGameStateType;

        public StartGameStateChanged(Type startGameStateType)
        {
            this.startGameStateType = startGameStateType;
        }

        public Type GetStartGameStateType() => startGameStateType;
    }
}
