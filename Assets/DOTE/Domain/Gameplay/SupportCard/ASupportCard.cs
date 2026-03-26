using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Domain.SupportCard
{
    public abstract class ASupportCard
    {
        public string SupportCardId { get; private set; }
        public SupportCardInformation SupportCardInformation { get; private set; }

        public bool IsUsed { get; private set; }
        public bool IsUsing { get; private set; }

        private IDomainEventBus domainEventBus;

        protected ASupportCard(string supportCardId, SupportCardInformation supportCardInformation, IDomainEventBus domainEventBus)
        {
            SupportCardId = supportCardId;
            SupportCardInformation = supportCardInformation;
            this.domainEventBus = domainEventBus;
        }

        public void UseSupportCard()
        {
            if (IsUsed)
            {
                return;
            }

            UseSupportCardHook();
            IsUsing = true;
            domainEventBus.Publish(new SupportCardUsingStarted(SupportCardId));
        }

        public void CancelUsingSupportCard()
        {
            if (IsUsed)
            {
                return;
            }

            CancelUsingSupportCardHook();
            IsUsing = false;
            domainEventBus.Publish(new SupportCardUsingCanceled(SupportCardId));
        }

        protected void CompleteUsing()
        {
            IsUsed = true;
            IsUsing = false;
            domainEventBus.Publish(new SupportCardUsed(SupportCardId));
        }

        protected abstract void UseSupportCardHook();
        protected abstract void CancelUsingSupportCardHook();
    }
}