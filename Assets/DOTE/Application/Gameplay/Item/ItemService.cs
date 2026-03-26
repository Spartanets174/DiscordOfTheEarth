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
            domainEventBus.Subscribe<CharacterItemEquiped>(ApplyItemEffect);
            domainEventBus.Subscribe<CharacterItemRemoved>(RemoveItemEffect);
        }

        private void Unsubscribe()
        {
            domainEventBus.Unsubscribe<CharacterItemEquiped>(ApplyItemEffect);
            domainEventBus.Unsubscribe<CharacterItemRemoved>(RemoveItemEffect);
        }

        private void ApplyItemEffect(CharacterItemEquiped equiped)
        {
            PlayableCharacter character = characterRepository.GetCharacterById(equiped.CharacterId);
            AItem item = itemRepository.GetItemById(equiped.ItemId);

            item.ApplyEffect(character);
        }

        private void RemoveItemEffect(CharacterItemRemoved removed)
        {
            PlayableCharacter character = characterRepository.GetCharacterById(removed.CharacterId);
            AItem item = itemRepository.GetItemById(removed.ItemId);

            item.RemoveEffect(character);
        }
    }
}