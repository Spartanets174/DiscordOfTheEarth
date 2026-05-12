using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Domain.Character
{
    public class CharacterAttacked : IDomainEvent
    {
        private string attackerId;
        private string damagedCharacterId;
        private int attackCost;

        public CharacterAttacked(string attackerId, string damagedCharacterId, int attackCost)
        {
            this.attackerId = attackerId;
            this.damagedCharacterId = damagedCharacterId;
            this.attackCost = attackCost;
        }

        public string GetDamagedCharacterId() => damagedCharacterId;
        public string GetAttackerId() => attackerId;
        public int GetAttackCost() => attackCost;
    }
}