namespace DOTE.Domain.Gameplay.Item
{
    public abstract class AItem 
    {
        public abstract void ApplyEffect(DOTE.Domain.Gameplay.Character.Character character);
        public abstract void RemoveEffect(DOTE.Domain.Gameplay.Character.Character character);
    }
}