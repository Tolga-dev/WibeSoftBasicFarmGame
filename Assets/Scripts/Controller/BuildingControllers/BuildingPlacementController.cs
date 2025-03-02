using System;
using Controller.Base;
using Entity.InGameObject.Base;
using Entity.InGameObject.Buildings;
using Managers;
using Managers.Base;
using So.GameObjectsSo.Building;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace Controller
{
    [Serializable]
    public class BuildingCreationController : ControllerBase
    {
        public void InitializeNewBuilding(GameObject building, ref InGameObjectBase inGameObject, GridLayout gridLayout, BuildingPlacementController buildingPlacementController)
        {
            if (inGameObject != null && inGameObject.isPlaced == false)
                buildingPlacementController.CleanObjects(inGameObject, BuildingManager.tileMapController);
            
            inGameObject = Object.Instantiate(building, Vector3.zero, Quaternion.identity).GetComponent<InGameObjectBase>();
            buildingPlacementController.FollowBuilding(inGameObject, gridLayout, BuildingManager.tileMapController);
        }

        public InGameObjectBase CreateNewInstanceInPosition(BuildingInGameSaveSo building, Vector3 position)
        {
            var created = Object.Instantiate(building.buildingSo.buildPrefab, position, Quaternion.identity).GetComponent<BuildingBase>();
            created.buildingInGameSaveSo = building;
            
            created.Place();
            return created;
        }
        
        public BuildingManager BuildingManager => (BuildingManager)ManagerBase;
    }

}