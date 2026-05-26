using DOTE.Gameplay.Domain.Character;
using DOTE.Gameplay.Domain.GameParty;
using DOTE.Gameplay.Domain.Player;
using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Application
{
    public class DeselectCharacterCommandHandler : ACommandHandler<DeselectCharacterCommand>
    {
        private IPlayerRepository playerRepository;
        private ICharacterRepository characterRepository;
        private IGamePartyRepository gamePartyRepository;

        public DeselectCharacterCommandHandler(IPlayerRepository playerRepository, ICharacterRepository characterRepository, IGamePartyRepository gamePartyRepository)
        {
            this.playerRepository = playerRepository;
            this.characterRepository = characterRepository;
            this.gamePartyRepository = gamePartyRepository;
        }

        public override void HandleHook(DeselectCharacterCommand command)
        {
            GamePartyPlayer player = playerRepository.GetPlayerById(command.PlayerId);
            PlayableCharacter character = characterRepository.GetCharacterById(command.CharacterId);
            IGameParty gameParty = gamePartyRepository.GetCurrentIGameParty();

            if (player != null && character != null && gameParty != null)
            {
                gameParty.CurrentState.DeselectCharacter(player, character);
            }
        }
    }
}
