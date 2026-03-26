using DOTE.Gameplay.Domain.Item;
using System.Collections.Generic;

namespace DOTE.Gameplay.Infrastructure.Item
{
    public class ItemRepository : IItemRepository
    {
        private Dictionary<string, AItem> itemsMap;

        public ItemRepository()
        {
            itemsMap = new();
        }

        public void AddItem(AItem item)
        {
            if (!itemsMap.ContainsKey(item.ItemId))
            {
                itemsMap.Add(item.ItemId, item);
            }
        }

        public void RemoveItem(string itemId)
        {
            if (itemsMap.ContainsKey(itemId))
            {
                itemsMap.Remove(itemId);
            }
        }

        public AItem GetItemById(string characterId)
        {
            itemsMap.TryGetValue(characterId, out AItem item);
            return item;
        }
    }
}