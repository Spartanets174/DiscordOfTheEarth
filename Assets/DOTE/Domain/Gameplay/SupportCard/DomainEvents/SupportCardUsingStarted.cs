using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Domain.SupportCard
{
    public class SupportCardUsingStarted : IDomainEvent
    {
        private string supportCardId;

        public SupportCardUsingStarted(string supportCardId)
        {
            this.supportCardId = supportCardId;
        }

        public string GetSupportCardId() => supportCardId;
    }
}