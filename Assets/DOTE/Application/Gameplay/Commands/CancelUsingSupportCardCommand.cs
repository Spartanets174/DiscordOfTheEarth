using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Application
{
    public class CancelUsingSupportCardCommand: ICommand
    {
        public string PlayerId { get; private set; }
        public string SupportCardId { get; private set; }

        public CancelUsingSupportCardCommand(string playerId, string supportCardId)
        {
            PlayerId = playerId;
            SupportCardId = supportCardId;
        }
    }
}
