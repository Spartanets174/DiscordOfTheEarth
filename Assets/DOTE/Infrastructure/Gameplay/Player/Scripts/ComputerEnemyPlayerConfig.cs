using DOTE.SharedKernel.Domain;
using System.Collections.Generic;
using UnityEngine;

namespace DOTE.Gameplay.Infrastructure
{
    [CreateAssetMenu(fileName = "ComputerPlayerConfig", menuName = "DOTE/Gameplay/ComputerEnemyPlayerConfig")]
    public class ComputerEnemyPlayerConfig : ScriptableObject
    {
        [Header("Data")]
        [SerializeField]
        private GuidProperty id;

        [Header("Deck")]
        [SerializeField]
        private List<CharacterConfig> characters;

        public string GetId() => id.guidString;
        public List<CharacterConfig> GetComputerPlayerCharacters() => characters;
    }
}