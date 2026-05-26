using DOTE.SharedKernel.Domain;


namespace DOTE.Gameplay.Application
{
    public class SelectCharacterCommand : ICommand
    {
        public string PlayerId { get; private set; }
        public string CharacterId { get; private set; }

        public SelectCharacterCommand(string playerId, string characterId)
        {
            PlayerId = playerId;
            CharacterId = characterId;
        }
    }
}