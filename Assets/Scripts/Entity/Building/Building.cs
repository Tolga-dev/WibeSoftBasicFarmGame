using Managers;
using UnityEngine;

namespace Entity.Building
{
    public class Building : MonoBehaviour
    {
        public bool isPlaced;
        public BoundsInt area;


        public bool CanBePlace()
        {
            var posInt = BuildingManager.gridLayout.WorldToCell(transform.position);
            var areaTemp = area;
            areaTemp.position = posInt;

            if (BuildingManager.tileMapController.CanTakeArea(areaTemp))
                return true;

            return false;
        }
        
        public void Place()
        {
            var posInt = BuildingManager.gridLayout.WorldToCell(transform.position);
            var areaTemp = area;
            areaTemp.position = posInt;
            isPlaced = true;
            BuildingManager.tileMapController.TakeArea(areaTemp);
        }

        private BuildingManager BuildingManager => GameManager.Instance.buildingManager;

    }
}