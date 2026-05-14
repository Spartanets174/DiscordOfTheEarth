using DOTE.SharedKernel.Domain;
using System;

namespace DOTE.Gameplay.Domain.GameParty
{
    public class PlayerTurnStateChanged: IDomainEvent
    {
        private Type playerTurnStateType;

        public PlayerTurnStateChanged(Type playerTurnStateType)
        {
            this.playerTurnStateType = playerTurnStateType;
        }

        public Type GetPlayerTurnStateType() => playerTurnStateType;
    }
}