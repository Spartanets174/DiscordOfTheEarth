using UnityEngine;

namespace DOTE.SharedKernel.Infrastructure
{
    public abstract class ACharacterPassiveAbilityConfig : ScriptableObject
    {
        [SerializeField, TextArea(2, 10)]
        private string abilityName;
        [SerializeField, TextArea(5, 10)]
        private string abilityDescription;

        public string GetAbilityName() => abilityName;
        public string GetAbilityDescription() => abilityDescription;
    }
}