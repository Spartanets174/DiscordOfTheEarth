namespace DOTE.Gameplay.Domain.SupportCard
{
    public class EmptySupportCard : ASupportCard
    {
        public EmptySupportCard(string supportCardId, SupportCardInformation supportCardInformation) : base(supportCardId, supportCardInformation)
        {
        }

        protected override void CancelUsingSupportCardHook()
        {

        }

        protected override void UseSupportCardHook()
        {
            CompleteUsing();
        }
    }
}