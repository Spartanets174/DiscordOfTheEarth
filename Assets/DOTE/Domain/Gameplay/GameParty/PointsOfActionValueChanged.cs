using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Domain.GameParty
{
    public class PointsOfActionValueChanged: IDomainEvent
    {
        private string playerId;
        private int pointsOfAction;

        public PointsOfActionValueChanged(string playerId, int pointsOfAction)
        {
            this.playerId = playerId;
            this.pointsOfAction = pointsOfAction;
        }

        public string GetPlayerId() => playerId;
        public int GetPointsOfAction() => pointsOfAction;
    }
}