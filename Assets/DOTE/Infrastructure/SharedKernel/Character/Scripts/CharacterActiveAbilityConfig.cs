using UnityEngine;

namespace DOTE.SharedKernel.Infrastructure.Character
{
    [CreateAssetMenu(fileName = "CharacterActiveAbilityConfig", menuName = "DOTE/SharedKernel/Character/CharacterActiveAbilityConfig")]
    public abstract class CharacterActiveAbilityConfig : ScriptableObject
    {
        [SerializeField]
        private string abilityName;
        [SerializeField]
        private string abilityDescription;

        public string GetAbilityName() => abilityName;
        public string GetAbilityDescription() => abilityDescription;
    }
}