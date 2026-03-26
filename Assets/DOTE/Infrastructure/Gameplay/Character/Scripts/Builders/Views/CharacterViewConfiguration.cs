using DOTE.SharedKernel.Infrastructure.Character;
using UnityEngine;

namespace DOTE.Gameplay.Infrastructure.Character
{
    [CreateAssetMenu(fileName = "CharacterViewConfiguration", menuName = "DOTE/Gameplay/Character/CharacterViewConfiguration")]
    public class CharacterViewConfiguration : ScriptableObject
    {
        [SerializeField]
        private CharacterConfig characterConfig;
        [SerializeField]
        private GameObject characterPrefab;
    }
}