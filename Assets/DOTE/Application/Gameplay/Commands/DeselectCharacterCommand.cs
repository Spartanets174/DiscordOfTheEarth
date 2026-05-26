
using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Application
{
    public class DeselectCharacterCommand : ICommand
    {
        public string PlayerId { get; private set; }
        public string CharacterId { get; private set; }

        public DeselectCharacterCommand(string playerId, string characterId)
        {
            PlayerId = playerId;
            CharacterId = characterId;
        }
    }
}