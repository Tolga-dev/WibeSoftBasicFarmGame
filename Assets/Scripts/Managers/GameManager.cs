using Managers.Inventory;
using Managers.Shop;
using Second.Scripts.Core;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public BuildingManager buildingManager;
        public SaveManager saveManager;
        public InventoryManager inventoryManager;
        public ShopManager shopManager;
    }
}