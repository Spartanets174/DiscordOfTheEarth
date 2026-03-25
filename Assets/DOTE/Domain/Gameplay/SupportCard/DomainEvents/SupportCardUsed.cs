using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Domain.SupportCard
{
    public class SupportCardUsed : IDomainEvent
    {
        private string supportCardId;

        public SupportCardUsed(string supportCardId)
        {
            this.supportCardId = supportCardId;
        }

        public string GetSupportCardId() => supportCardId;
    }
}