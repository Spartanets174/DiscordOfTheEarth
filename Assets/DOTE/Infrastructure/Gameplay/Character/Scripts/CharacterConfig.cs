using DOTE.Gameplay.UI;
using DOTE.SharedKernel.Infrastructure;
using UnityEngine;

namespace DOTE.Gameplay.Infrastructure
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "DOTE/Gameplay/Character/CharacterConfig")]
    public class CharacterConfig : ScriptableObject
    {
        [SerializeField]
        private CharacterView characterPrefab;
        [SerializeField]
        private CharacterInformationConfig characterInformationConfig;

        [SerializeField]
        private ACharacterActiveAbilityConfig attackAbilityConfig;
        [SerializeField]
        private ACharacterActiveAbilityConfig protectiveAbilityConfig;
        [SerializeField]
        private ACharacterActiveAbilityConfig enhancingAbilityConfig;

        [SerializeField]
        private ACharacterPassiveAbilityConfig passiveAbilityConfig;

        [SerializeField]
        private int defaultAbilityUsingCost = 11;
        [SerializeField]
        private int defaultAttackCost = 2;

        public CharacterView GetCharacterPrefab() => characterPrefab;
        public CharacterInformationConfig GetCharacterInformationConfig() => characterInformationConfig;

        public ACharacterActiveAbilityConfig GetAttackAbilityConfig() => attackAbilityConfig;
        public ACharacterActiveAbilityConfig GetProtectiveAbilityConfig() => protectiveAbilityConfig;
        public ACharacterActiveAbilityConfig GetEnhancingAbilityConfig() => enhancingAbilityConfig;

        public ACharacterPassiveAbilityConfig GetPassiveAbilityConfig() => passiveAbilityConfig;

        public int GetDefaultAbilityUsingCost() => defaultAbilityUsingCost;
        public int GetDefaultAttackCost() => defaultAttackCost;
    }
}