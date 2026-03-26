using UnityEngine;

namespace DOTE.SharedKernel.Infrastructure.Character
{
    [CreateAssetMenu(fileName = "CharacterPassiveAbilityConfig", menuName = "DOTE/SharedKernel/Character/CharacterPassiveAbilityConfig")]
    public abstract class CharacterPassiveAbilityConfig : ScriptableObject
    {
        [SerializeField]
        private string abilityName;
        [SerializeField]
        private string abilityDescription;

        public string GetAbilityName() => abilityName;
        public string GetAbilityDescription() => abilityDescription;
    }
}