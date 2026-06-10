using DOTE.SharedKernel.Domain;
using DOTE.SharedKernel.Infrastructure;
using System.Collections.Generic;
using UnityEngine;


namespace DOTE.Gameplay.Infrastructure
{
    [CreateAssetMenu(fileName = "ComputerPlayerConfig", menuName = "DOTE/Gameplay/ComputerPlayerConfig")]
    public class ComputerPlayerConfig : ScriptableObject
    {
        [Header("Data")]
        [SerializeField]
        private GuidProperty id;
        [SerializeField]
        private string computerPlayerName;
        [SerializeField]
        private Sprite computerPlayerImage;

        [Header("Deck")]
        [SerializeField]
        private List<CharacterConfig> characters;
        [SerializeField]
        private List<ASupportCardConfig> supportCards;

        [Header("AI")]
        [SerializeField]
        private GameObject behaviorAlgorithmPrefab;
    }
}