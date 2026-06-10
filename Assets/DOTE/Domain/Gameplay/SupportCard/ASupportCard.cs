using DOTE.SharedKernel.Domain;
using Zenject;

namespace DOTE.Gameplay.Domain.SupportCard
{
    public abstract class ASupportCard
    {
        public string SupportCardId { get; private set; }
        public SupportCardInformation SupportCardInformation { get; private set; }

        public bool IsUsed { get; private set; }
        public bool IsUsing { get; private set; }

        [Inject]
        private IDomainEventBus domainEventBus;

        protected ASupportCard(string supportCardId, SupportCardInformation supportCardInformation)
        {
            SupportCardId = supportCardId;
            SupportCardInformation = supportCardInformation;
        }

        public void UseSupportCard()
        {
            if (IsUsed || IsUsing)
            {
                return;
            }

            UseSupportCardHook();
            IsUsing = true;
            domainEventBus.Publish(new SupportCardUsingStarted(SupportCardId));
        }

        public void CancelUsingSupportCard()
        {
            if (IsUsed || !IsUsing)
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