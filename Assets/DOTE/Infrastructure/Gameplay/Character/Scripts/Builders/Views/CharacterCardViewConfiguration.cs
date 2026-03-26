using DOTE.SharedKernel.Infrastructure.Character;
using UnityEngine;

namespace DOTE.Gameplay.Infrastructure.Character
{
    [CreateAssetMenu(fileName = "CharacterCardViewConfiguration", menuName = "DOTE/Gameplay/Character/CharacterCardViewConfiguration")]
    public class CharacterCardViewConfiguration : ScriptableObject
    {
        [SerializeField]
        private CharacterConfig characterConfig;
        [SerializeField]
        private Sprite characterImage;
        [SerializeField]
        private GameObject viewPrefab;
    }
}