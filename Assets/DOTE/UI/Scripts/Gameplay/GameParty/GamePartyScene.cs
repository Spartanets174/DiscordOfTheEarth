using System.Collections.Generic;
using UnityEngine;

namespace DOTE.Gameplay.UI
{
    public class GamePartyScene : MonoBehaviour
    {
        public Grid Grid => grid;
        public List<ACellView> CellPrefabs => cellPrefabs;
        public List<AObstacleView> ObstaclePrefabs => obstaclePrefabs;
        public List<CharacterView> CharacterPrefabs => characterPrefabs;

        [SerializeField]
        private Grid grid;

        [SerializeField]
        private List<ACellView> cellPrefabs = new();

        [SerializeField]
        private List<AObstacleView> obstaclePrefabs = new();

        [SerializeField]
        private List<CharacterView> characterPrefabs = new();
    }
}