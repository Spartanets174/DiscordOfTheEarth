using DOTE.Gameplay.Domain.Character;


namespace DOTE.Gameplay.Domain.Item
{
    public interface IItem
    {
        public string ItemId { get; }
        public string CharacterId { get; }
        public ItemInformation ItemInformation { get; }


        public void Equip(IItemEffectContext itemContext, PlayableCharacter character);
        public void Remove(PlayableCharacter character);
        public bool IsItemFree();
    }
}