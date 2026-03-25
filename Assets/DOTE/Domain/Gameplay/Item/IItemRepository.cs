namespace DOTE.Gameplay.Domain.Item
{
    public interface IItemRepository
    {
        public void AddItem(AItem item);
        public void RemoveItem(string itemId);
        public AItem GetItemById(string characterId);
        public string GenerateItemId();
    }
}