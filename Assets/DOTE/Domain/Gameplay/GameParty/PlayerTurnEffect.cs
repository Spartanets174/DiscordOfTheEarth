using System;
using System.Collections.Generic;

namespace DOTE.Gameplay.Domain.GameParty
{
    public class PlayerTurnEffect
    {
        private bool isBuff;
        private int turnsToFinish;
        private Action completeAction;

        public PlayerTurnEffect(bool isBuff, int turnCount, Action endAction)
        {
            this.isBuff = isBuff;
            this.turnsToFinish = turnCount;
            this.completeAction = endAction;
        }

        public bool GetIsBuff() => isBuff;

        public void DecreaseTurnCount()
        {
            turnsToFinish--;
        }

        public bool IsCounted()
        {
            return turnsToFinish <= 0;
        }

        public void Complete()
        {
            completeAction?.Invoke();
        }

        public override bool Equals(object obj)
        {
            return obj is PlayerTurnEffect countable &&
                   isBuff == countable.isBuff &&
                   turnsToFinish == countable.turnsToFinish &&
                   EqualityComparer<Action>.Default.Equals(completeAction, countable.completeAction);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(isBuff, turnsToFinish, completeAction);
        }
    }
}
