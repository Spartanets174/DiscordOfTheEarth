using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Application
{
    public class UseSupportCardCommand: ICommand
    {
        public string PlayerId { get; private set; }
        public string SupportCardId { get; private set; }

        public UseSupportCardCommand(string playerId, string supportCardId)
        {
            PlayerId = playerId;
            SupportCardId = supportCardId;
        }
    }
}
