using DOTE.Gameplay.Domain.GameParty;
using DOTE.Gameplay.Domain.Player;
using DOTE.Gameplay.Domain.SupportCard;
using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Application
{
    public class CancelUsingSupportCardCommandHandler : ACommandHandler<CancelUsingSupportCardCommand>
    {
        private IPlayerRepository playerRepository;
        private ISupportCardRepository supportCardRepository;
        private IGamePartyRepository gamePartyRepository;

        public CancelUsingSupportCardCommandHandler(IPlayerRepository playerRepository, ISupportCardRepository supportCardRepository, IGamePartyRepository gamePartyRepository)
        {
            this.playerRepository = playerRepository;
            this.supportCardRepository = supportCardRepository;
            this.gamePartyRepository = gamePartyRepository;
        }

        public override void HandleHook(CancelUsingSupportCardCommand command)
        {
            GamePartyPlayer player = playerRepository.GetPlayerById(command.PlayerId);
            ASupportCard supportCard = supportCardRepository.GetSupportCardById(command.SupportCardId);
            IGameParty gameParty = gamePartyRepository.GetCurrentIGameParty();

            if (player != null && supportCard != null && gameParty != null)
            {
                gameParty.CurrentState.CancelUsingSupportCard(player, supportCard);
            }
        }
    }
}
