using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Application
{
    public class CancelUsingCharacterAbilityCommand: ICommand
    {
        public string PlayerId { get; private set; }
        public string CharacterId { get; private set; }

        public CancelUsingCharacterAbilityCommand(string playerId, string characterId)
        {
            PlayerId = playerId;
            CharacterId = characterId;
        }
    }
}
