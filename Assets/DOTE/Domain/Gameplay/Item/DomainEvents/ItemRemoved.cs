using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Domain.Item
{
    public class ItemRemoved : IDomainEvent
    {
        public string ItemId { get; private set; }
        public string CharacterId { get; private set; }

        public ItemRemoved(string itemId, string characterId)
        {
            ItemId = itemId;
            CharacterId = characterId;
        }

        public string GetItemId() => ItemId;
        public string GetCharacterId() => CharacterId;
    }
}