using DOTE.SharedKernel.Domain;
using UnityEngine;


namespace DOTE.SharedKernel.Infrastructure
{
    public abstract class ASupportCardConfig : ScriptableObject
    {
        [SerializeField, TextArea(2, 10)]
        private string supportCardName;
        [SerializeField, TextArea(5, 10)]
        private string supportCardAbilityDescription;
        [SerializeField]
        private SupportCardType supportCardType;
        [SerializeField]
        private Rarity supportCardRarity;
        [SerializeField]
        private Sprite supportCardImage;

        public string GetSupportCardName() => supportCardName;
        public string GetSupportCardAbilityDescription() => supportCardAbilityDescription;
        public SupportCardType GetSupportCardType() => supportCardType;
        public Rarity GetSupportCardRarity() => supportCardRarity;
        public Sprite GetSupportCardImage() => supportCardImage;
    }
}