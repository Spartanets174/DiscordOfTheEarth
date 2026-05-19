using DOTE.Gameplay.Domain.Character;
using DOTE.Gameplay.Domain.Item;
using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Application.Item
{
    public class ItemService
    {
        private IItemRepository itemRepository;
        private ICharacterRepository characterRepository;
        private IDomainEventBus domainEventBus;

        public ItemService(IItemRepository itemRepository, IDomainEventBus domainEventBus, ICharacterRepository characterRepository)
        {
            this.itemRepository = itemRepository;
            this.domainEventBus = domainEventBus;
            this.characterRepository = characterRepository;

            Subscribe();
        }

        ~ItemService()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            domainEventBus.Subscribe<ItemEquiped>(ApplyItemEffect);
            domainEventBus.Subscribe<ItemRemoved>(RemoveItemEffect);
        }

        private void Unsubscribe()
        {
            domainEventBus.Unsubscribe<ItemEquiped>(ApplyItemEffect);
            domainEventBus.Unsubscribe<ItemRemoved>(RemoveItemEffect);
        }

        private void ApplyItemEffect(ItemEquiped equiped)
        {
            PlayableCharacter character = characterRepository.GetCharacterById(equiped.CharacterId);
            IItem item = itemRepository.GetItemById(equiped.ItemId);

            //item.Equip(character);
        }

        private void RemoveItemEffect(ItemRemoved removed)
        {
            PlayableCharacter character = characterRepository.GetCharacterById(removed.CharacterId);
            IItem item = itemRepository.GetItemById(removed.ItemId);

            item.Remove(character);
        }
    }
}