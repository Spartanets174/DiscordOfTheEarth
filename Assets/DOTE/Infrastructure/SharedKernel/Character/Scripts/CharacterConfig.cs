using DOTE.SharedKernel.Domain;
using UnityEngine;

namespace DOTE.SharedKernel.Infrastructure.Character
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "DOTE/SharedKernel/Character/CharacterConfig")]
    public class CharacterConfig : ScriptableObject
    {
        [Header("Infromation")]
        [SerializeField]
        private string characterName;
        [SerializeField]
        private string characterDescription;

        [SerializeField]
        private Race characterRace;
        [SerializeField]
        private Class characterClass;
        [SerializeField]
        private Rarity characterRarity;

        [SerializeField]
        private int characterPrice;

        [Header("Characteristics")]
        [SerializeField]
        private float health;
        [SerializeField]
        private int speed;

        [SerializeField]
        private float physicalAttack;
        [SerializeField]
        private float magicalAttack;

        [SerializeField]
        private float physicalDefence;
        [SerializeField]
        private float magicalDefence;

        [SerializeField]
        private float criticalDamageChance;
        [SerializeField]
        private float criticalDamageValue;

        [SerializeField]
        private int attackRange;

        public string GetCharacterName () => characterName;
        public string GetCharacterDescription() => characterDescription; 
        public Race GetCharacterRace() => characterRace; 
        public Class GetCharacterClass() => characterClass; 
        public Rarity GetCharacterRarity() => characterRarity; 
        public int GetCharacterPrice() => characterPrice; 
        public float GetHealth() => health; 
        public int GetSpeed() => speed;  
        public float GetPhysicalAttack() => physicalAttack; 
        public float GetMagicalAttack() => magicalAttack; 
        public float GetPhysicalDefence() => physicalDefence; 
        public float GetMagicalDefence() => magicalDefence; 
        public float GetCriticalDamageChance() => criticalDamageChance; 
        public float GetCriticalDamageValue() => criticalDamageValue; 
        public int GetAttackRange() => attackRange; 
    }
}