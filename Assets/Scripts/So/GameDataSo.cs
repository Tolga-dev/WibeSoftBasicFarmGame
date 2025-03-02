using So.GameObjectsSo.Building;
using So.Seed;
using Unity.VisualScripting;
using UnityEngine;

namespace So
{
    [CreateAssetMenu(fileName = "GameDataSo", menuName = "So/GameDataSo", order = 0)]
    public class GameDataSo : ScriptableObject
    {
        public GameTileBaseSo gameTileBaseSo;
        public SeedsSo seedSo;
        public BuildingsSo buildingsSo; 
        public CraftSo craftSo;
        public InventorySo inventorySo;
    }
}