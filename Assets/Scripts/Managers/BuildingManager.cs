using System;
using System.Collections.Generic;
using Controller;
using Entity.InGameObject;
using Entity.InGameObject.Base;
using Entity.InGameObject.Buildings;
using Managers.Base;
using Second.Scripts.Managers;
using So;
using So.GameObjectsSo.Building;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace Managers
{
    public class BuildingManager : ManagerBase
    {
        public TileMapController tileMapController;
        public InputController inputController;
        public BuildingPlacementController buildingPlacementController;
        public BuildingCreationController buildingCreationController;
        
        public GridLayout gridLayout;
        
        public InGameObjectBase temp;
        public Vector3Int prevPos;

        public Camera cam;
        private void Start()
        {
            tileMapController.Initialization(this); 
            inputController.Initialization(this);
            buildingPlacementController.Initialization(this);
            buildingCreationController.Initialization(this);

            gameManager.saveManager.LoadDataFromSave();
        }

        private void Update() 
        {
            inputController.HandleInput(temp, gridLayout, cam, ref prevPos, buildingPlacementController);
        }
        
        // called from button
        public void InitializeNewBuilding(GameObject building)
        {
            buildingCreationController.InitializeNewBuilding(building, ref temp, gridLayout, buildingPlacementController);
        }

        public InGameObjectBase CreateNewInstanceInPosition(BuildingInGameSaveSo building, Vector3 position)
        {
            return buildingCreationController.CreateNewInstanceInPosition(building, position);
        }
 
        public void CleanObjects()
        {
            buildingPlacementController.CleanObjects(temp, tileMapController);
        }

        public GameDataSo GameDataSo => gameManager.saveManager.gameDataSo;
    }


}