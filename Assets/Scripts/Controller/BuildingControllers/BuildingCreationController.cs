using System;
using Controller.Base;
using Entity.InGameObject.Base;
using Entity.InGameObject.Buildings;
using Managers;
using Managers.Base;
using So;
using So.GameObjectsSo.Building;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace Controller
{
    [Serializable]
    public class BuildingPlacementController : ControllerBase
    {
        public void FollowBuilding(InGameObjectBase temp, GridLayout gridLayout, TileMapController tileMapController)
        {
            tileMapController.ClearArea();
            temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
            
            var buildingArea = temp.area;
            
            var baseArr = tileMapController.GetTilesBlock(buildingArea, tileMapController.mainTileMap);
            
            int size = baseArr.Length;
            var tileArray = new TileBase[size];
            
            for (int i = 0; i < size; i++)
            {
                if (baseArr[i] == tileMapController.TileBases[TileTypes.White])
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

        public void CleanObjects(InGameObjectBase temp, TileMapController tileMapController)
        {
            tileMapController.ClearArea();
            if (temp != null)
            {
                Object.Destroy(temp.gameObject);
            }
        }
    }

}