using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Domain.Item
{
    public abstract class AItem
    {
        public string ItemId { get; private set; }

        private IDomainEventBus eventBus;

        public void Equip(Character.PlayableCharacter character)
        {
            EquipHook(character);
            eventBus.Publish(new ItemEquiped(ItemId, character.CharacterId));
        }
        public void Remove(Character.PlayableCharacter character)
        {
            RemoveHook(character);
            eventBus.Publish(new ItemRemoved(ItemId, character.CharacterId));
        }

        protected abstract void EquipHook(Character.PlayableCharacter character);
        protected abstract void RemoveHook(Character.PlayableCharacter character);
    }
}