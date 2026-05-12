using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Domain.GameParty
{
    public class PointsOfActionValueChanged: IDomainEvent
    {
        private int pointsOfAction;

        public PointsOfActionValueChanged( int pointsOfAction)
        {
            this.pointsOfAction = pointsOfAction;
        }

        public int GetPointsOfAction() => pointsOfAction;
    }
}