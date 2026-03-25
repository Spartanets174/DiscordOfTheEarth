using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Domain.Item
{
    public class ItemEquiped : IDomainEvent
    {
        public string ItemId { get; private set; }
        public string CharacterId { get; private set; }

        public ItemEquiped(string itemId, string characterId)
        {
            ItemId = itemId;
            CharacterId = characterId;
        }

        public string GetItemId() => ItemId;
        public string GetCharacterId() => CharacterId;
    }
}