namespace DOTE.Domain.Gameplay.Character
{
    public class CharacterHealed
    {
        private string healedCharacterId;
        private float healAmount;

        public CharacterHealed(string healedCharacterId, float healAmount)
        {
            this.healedCharacterId = healedCharacterId;
            this.healAmount = healAmount;
        }

        public string GetHealedCharacterId() => healedCharacterId;
        public float GetHealAmount() => healAmount;
    }
}