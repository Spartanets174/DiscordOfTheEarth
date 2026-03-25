using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Domain.SupportCard
{
    public class SupportCardUsingCanceled : IDomainEvent
    {
        private string supportCardId;

        public SupportCardUsingCanceled(string supportCardId)
        {
            this.supportCardId = supportCardId;
        }

        public string GetSupportCardId() => supportCardId;
    }
}