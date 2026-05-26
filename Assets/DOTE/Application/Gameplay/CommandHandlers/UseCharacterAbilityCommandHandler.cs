using Assets.DOTE.Application.Gameplay.Commands;
using DOTE.Gameplay.Domain.Character;
using DOTE.Gameplay.Domain.GameParty;
using DOTE.Gameplay.Domain.Player;
using DOTE.SharedKernel.Domain;

namespace Assets.DOTE.Application.Gameplay.CommandHandlers
{
    public class UseCharacterAbilityCommandHandler : ACommandHandler<UseCharacterAbilityCommand>
    {
        private IPlayerRepository playerRepository;
        private ICharacterRepository characterRepository;
        private IGamePartyRepository gamePartyRepository;

        public override void HandleHook(UseCharacterAbilityCommand command)
        {
            GamePartyPlayer player = playerRepository.GetPlayerById(command.PlayerId);
            PlayableCharacter character = characterRepository.GetCharacterById(command.CharacterId);
            IGameParty gameParty = gamePartyRepository.GetCurrentIGameParty();

            if (player != null && character != null && gameParty != null)
            {
                gameParty.CurrentState.UseCharacterAbility(player, character, command.ActiveAbilityType);
            }
        }
    }
}
