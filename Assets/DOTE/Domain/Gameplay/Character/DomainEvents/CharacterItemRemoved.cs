using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Domain.Character
{
    public class CharacterItemRemoved : IDomainEvent
    {
        public string ItemId { get; private set; }
        public string CharacterId { get; private set; }

        public CharacterItemRemoved(string itemId, string characterId)
        {
            ItemId = itemId;
            CharacterId = characterId;
        }

        public string GetItemId() => ItemId;
        public string GetCharacterId() => CharacterId;
    }
}