using Second.Scripts.Core;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public BuildingManager buildingManager;
        public SaveManager saveManager;
    }
}