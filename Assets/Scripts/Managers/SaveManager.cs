using System;
using System.Collections.Generic;
using Entity.InGameObject.Buildings;
using Managers.Base;
using Second.Scripts.Managers;
using So;
using So.GameObjectsSo.Building;
using UnityEngine;

namespace Managers
{
    public class SaveManager : ManagerBase
    {
        public GameDataSo gameDataSo;
    
        public void LoadDataFromSave()
        {
            // var buildings = gameDataSo.buildingsSo.buildingSaveSos;
            // CreateBuildings(buildings);
        }
        public void SaveDataFromGame()
        {
            
        }
        private void CreateBuildings(List<BuildingInGameSaveSo> buildings)
        {
            foreach (var building in buildings)
            {
                var currentBuilding = (BuildingBase)gameManager.buildingManager.CreateNewInstanceInPosition(building.buildingSo.buildPrefab,
                    building.position);
                currentBuilding.SetActionBuilding(building);
            }
        }
    }
}