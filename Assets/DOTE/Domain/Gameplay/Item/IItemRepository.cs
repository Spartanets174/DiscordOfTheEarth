namespace DOTE.Gameplay.Domain.Item
{
    public interface IItemRepository
    {
        public void AddItem(IItem item);
        public void RemoveItem(string itemId);
        public IItem GetItemById(string characterId);
    }
}