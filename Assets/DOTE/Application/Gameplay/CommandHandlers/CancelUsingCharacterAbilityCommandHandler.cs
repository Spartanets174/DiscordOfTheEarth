using DOTE.Gameplay.Domain.Character;
using DOTE.Gameplay.Domain.GameParty;
using DOTE.Gameplay.Domain.Player;
using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Application
{
    internal class CancelUsingCharacterAbilityCommandHandler : ACommandHandler<CancelUsingCharacterAbilityCommand>
    {
        private IPlayerRepository playerRepository;
        private ICharacterRepository characterRepository;
        private IGamePartyRepository gamePartyRepository;

        public override void HandleHook(CancelUsingCharacterAbilityCommand command)
        {
            GamePartyPlayer player = playerRepository.GetPlayerById(command.PlayerId);
            PlayableCharacter character = characterRepository.GetCharacterById(command.CharacterId);
            IGameParty gameParty = gamePartyRepository.GetCurrentIGameParty();

            if (player != null && character != null && gameParty != null)
            {
                gameParty.CurrentState.CancelUsingCharacterAbility(player, character);
            }
        }
    }
}
