using DOTE.SharedKernel.Domain;

namespace Assets.DOTE.Application.Gameplay.Commands
{
    public class UseCharacterAbilityCommand : ICommand
    {
        public string PlayerId { get; private set; }
        public string CharacterId { get; private set; }
        public ActiveAbilityType ActiveAbilityType { get; private set; }

        public UseCharacterAbilityCommand(string playerId, string characterId, ActiveAbilityType activeAbilityType)
        {
            PlayerId = playerId;
            CharacterId = characterId;
            ActiveAbilityType = activeAbilityType;
        }
    }
}
