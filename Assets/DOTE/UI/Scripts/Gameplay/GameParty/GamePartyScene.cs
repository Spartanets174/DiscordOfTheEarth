using System.Collections.Generic;
using UnityEngine;

namespace DOTE.Gameplay.UI
{
    public class GamePartyScene : MonoBehaviour
    {
        public Grid Grid => grid;
        public List<CellView> CellPrefabs => cellPrefabs;

        [SerializeField]
        private Grid grid;

        [SerializeField]
        private List<CellView> cellPrefabs;
    }
}