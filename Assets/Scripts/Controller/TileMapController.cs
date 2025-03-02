using System;
using System.Collections.Generic;
using Controller.Base;
using Managers;
using Managers.Base;
using So;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Controller
{
    [Serializable]
    public class TileMapController : ControllerBase
    {
        public Tilemap mainTileMap;
        public Tilemap tempTileMap;

        public Dictionary<TileTypes, TileBase> TileBases = new Dictionary<TileTypes, TileBase>();
        public BoundsInt prevArea;

        public override void Initialization(ManagerBase managerBaseVal)
        {
            base.Initialization(managerBaseVal);
            
            var pairs = BuildingManager.GameDataSo.gameTileBaseSo.tileBasePairs;
            foreach (var tileBasePair in pairs)
            {
                TileBases.Add(tileBasePair.tileType, tileBasePair.tileBase);
            }
        }

        public void SetTilesBlock(BoundsInt area, TileTypes tileTypes, Tilemap tilemap)
        {
            var currentSize = area.size.x * area.size.y * area.size.z;
            var arr = new TileBase[currentSize];
            FillTiles(arr, tileTypes);
            tilemap.SetTilesBlock(area, arr);
        }
        
        public TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
        {
            var currentSize = area.size.x * area.size.y * area.size.z;
            var arr = new TileBase[currentSize];
            var ind = 0;
            foreach (var pos in area.allPositionsWithin)
            {
                var currentPos = new Vector3Int(pos.x, pos.y, 0);
                arr[ind] = tilemap.GetTile(currentPos);
                ind++;
            }
            return arr;
        }
        public bool CanTakeArea(BoundsInt area)
        {
            var baseArr = GetTilesBlock(area, mainTileMap);
            foreach (var tileBase in baseArr)
            {
                if (tileBase != TileBases[TileTypes.White])
                {
                    Debug.Log("Cannot Place Here");
                    return false;
                }
            }

            return true;
        }

        public void TakeArea(BoundsInt area)
        {
            SetTilesBlock(area, TileTypes.Empty, tempTileMap);
            SetTilesBlock(area, TileTypes.Green, mainTileMap);
        }
        
        public void ClearArea()
        {
            var area = prevArea.size.x * prevArea.size.y * prevArea.size.z; 
            var toClear = new TileBase[area];
            FillTiles(toClear, TileTypes.Empty);
            tempTileMap.SetTilesBlock(prevArea, toClear);
        }
        public void FillTiles(TileBase[] arr, TileTypes tileTypes)
        {
            for (var i = 0; i < arr.Length; i++)
            {
                arr[i] = TileBases[tileTypes];
            }
        }
        private BuildingManager BuildingManager => (BuildingManager)ManagerBase;
    }
}