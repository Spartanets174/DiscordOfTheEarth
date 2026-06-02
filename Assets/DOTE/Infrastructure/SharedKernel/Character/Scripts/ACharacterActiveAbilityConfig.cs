using DOTE.SharedKernel.Domain;
using UnityEngine;

namespace DOTE.SharedKernel.Infrastructure
{
    public abstract class ACharacterActiveAbilityConfig : ScriptableObject
    {
        [SerializeField, TextArea(2, 10)]
        private string abilityName;
        [SerializeField, TextArea(5, 10)]
        private string abilityDescription;
        [SerializeField]
        private ActiveAbilityType activeAbilityType;

        public string GetAbilityName() => abilityName;
        public string GetAbilityDescription() => abilityDescription;
        public ActiveAbilityType GetActiveAbilityType() => activeAbilityType;
    }
}