using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace So
{
    [CreateAssetMenu(fileName = "GameTileBaseSo", menuName = "So/GameTileBaseSo", order = 0)]
    public class GameTileBaseSo : ScriptableObject
    {
        public List<TileBasePair> tileBasePairs = new List<TileBasePair>();
        
        public TileBasePair GetTileBasePair(TileTypes tileType)
        {
            return tileBasePairs.Find(x => x.tileType == tileType);
        }
    }
    
    [Serializable]
    public class TileBasePair
    {
        public TileTypes tileType;
        public TileBase tileBase;
    }
    public enum TileTypes
    {
        Empty,
        White,
        Green,
        Red
    }
}