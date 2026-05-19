using DOTE.Gameplay.Domain.Character;
using DOTE.SharedKernel.Domain;
using Zenject;

namespace DOTE.Gameplay.Domain.Item
{
    public abstract class AItem<T> : IItem where T : IItemEffectContext
    {
        public string ItemId { get; private set; }
        public string CharacterId { get; private set; }
        public ItemInformation ItemInformation { get; private set; }

        [Inject]
        private IDomainEventBus eventBus;
        protected AItem(string itemId, ItemInformation itemInformation)
        {
            ItemId = itemId;
            ItemInformation = itemInformation;
        }

        public void Equip(IItemEffectContext context, PlayableCharacter character)
        {
            if (!IsItemFree())
            {
                return;
            }

            CharacterId = character.CharacterId;
            EquipHook((T)context, character);
            eventBus.Publish(new ItemEquiped(ItemId, CharacterId));
        }
        public void Remove(PlayableCharacter character)
        {
            if (character.CharacterId != CharacterId)
            {
                return;
            }

            RemoveHook(character);
            eventBus.Publish(new ItemRemoved(ItemId, CharacterId));
        }

        public bool IsItemFree()
        {
            return string.IsNullOrEmpty(CharacterId);
        }

        protected abstract void EquipHook(T context, PlayableCharacter character);
        protected abstract void RemoveHook(PlayableCharacter character);
    }
}