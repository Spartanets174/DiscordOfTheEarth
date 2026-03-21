namespace DOTE.Domain.Gameplay.Character
{
    public class CharacterDamaged
    {
        private string damagedCharacterId;
        private string attackerName;
        private float damageAmount;

        public CharacterDamaged(string damagedCharacterId, string attackerName, float damageAmount)
        {
            this.damagedCharacterId = damagedCharacterId;
            this.attackerName = attackerName;
            this.damageAmount = damageAmount;
        }

        public string GetDamagedCharacterId() => damagedCharacterId;
        public string GetAttackerName() => attackerName;
        public float GetDamageAmount() => damageAmount;
    }
}