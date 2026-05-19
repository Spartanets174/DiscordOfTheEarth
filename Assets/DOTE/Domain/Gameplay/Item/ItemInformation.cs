namespace DOTE.Gameplay.Domain.Item
{
    public class ItemInformation
    {
        public string Name { get; }
        public string Description { get; }
        public string EffectDescription { get; }

        public ItemInformation(string name, string description, string effectDescription)
        {
            Name = name;
            Description = description;
            EffectDescription = effectDescription;
        }

        public string GetName() => Name;
        public string GetDescription() => Description;
        public string GetEffectDescription() => EffectDescription;
    }
}