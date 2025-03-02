using System;
using System.Collections.Generic;
using Controller;
using Entity.InGameObject;
using Entity.InGameObject.Base;
using Managers.Base;
using Second.Scripts.Managers;
using So;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace Managers
{
    public class BuildingManager : ManagerBase
    {
        public TileMapController tileMapController;
        
        public GridLayout gridLayout;
        
        public InGameObjectBase temp;
        public Vector3Int prevPos;

        private void Start()
        {
            tileMapController.Initialization(this);

            gameManager.saveManager.LoadDataFromSave();
        }

        private void Update() 
        {
            if(!temp)
                return;

            if (Input.GetMouseButton(0))
            {
                if(EventSystem.current.IsPointerOverGameObject(0))
                    return;

                if (!temp.isPlaced)
                {
                    var touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    touchPos.z = 0;
                    var cellPos = gridLayout.LocalToCell(touchPos);

                    if (prevPos != cellPos)
                    {
                        temp.transform.position = gridLayout.CellToLocalInterpolated(cellPos);
                        prevPos = cellPos;
                        FollowBuilding();
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if(temp.CanBePlace())
                    temp.Place();
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
              CleanObjects();
            }
        }

        private void FollowBuilding()
        {
            tileMapController.ClearArea();

            temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
            var buildingArea = temp.area;
            
            var baseArr = tileMapController.GetTilesBlock(buildingArea, tileMapController.mainTileMap);

            int size = baseArr.Length;
            var tileArray = new TileBase[size];
            
            for (int i = 0; i < size; i++)
            {
                if(baseArr[i] == tileMapController.TileBases[TileTypes.White])
                    tileArray[i] = tileMapController.TileBases[TileTypes.Green];
                else
                {
                    tileMapController.FillTiles(tileArray, TileTypes.Red);
                    break;                    
                }
            }
            tileMapController.tempTileMap.SetTilesBlock(buildingArea, tileArray);
            tileMapController.prevArea = buildingArea;
        }

        // called from button
        public void InitializeNewBuilding(GameObject building)
        {
            if (temp != null && temp.isPlaced == false)
                CleanObjects();
            
            temp = Instantiate(building, Vector3.zero, Quaternion.identity).GetComponent<InGameObjectBase>();
            FollowBuilding();
        }
        public InGameObjectBase CreateNewInstanceInPosition(GameObject building, Vector3 position)
        {
            var created = Instantiate(building, position, Quaternion.identity).GetComponent<InGameObjectBase>();
            created.Place();
            return created;
        }

        public void CleanObjects()
        {
            tileMapController.ClearArea();
            Destroy(temp.gameObject);
        }
        
         
        public GameDataSo GameDataSo => gameManager.saveManager.gameDataSo;
    }


}