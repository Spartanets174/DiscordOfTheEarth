using DOTE.SharedKernel.Domain;
using UnityEngine;

namespace DOTE.Gameplay.Infrastructure
{
    [CreateAssetMenu(fileName = "SingleplayerGamePartyConfig", menuName = "DOTE/Gameplay/SingleplayerGamePartyConfig")]
    public class SingleplayerGamePartyConfig : ScriptableObject
    {
        [SerializeField]
        private GuidProperty id;
        [SerializeField]
        private int defaultPointsOfActionValue = 20;
        [SerializeField]
        private ComputerEnemyPlayerConfig computerPlayerConfig;
        [SerializeField]
        private AIEnemyPlayerConfig enemyBotPlayerConfig;

        [SerializeField]
        private GameObject fieldPrefab;

        public string GetId() => id.guidString;
        public int GetDefaultPointsOfActionValue() => defaultPointsOfActionValue;
        public ComputerEnemyPlayerConfig GetComputerPlayerConfig() => computerPlayerConfig;
        public AIEnemyPlayerConfig GetAIEnemyPlayerConfig() => enemyBotPlayerConfig;
        public GameObject GetFieldPrefab() => fieldPrefab;
    }
}