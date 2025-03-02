using System.Collections.Generic;
using So.GameObjectsSo.Base;
using UnityEngine;

namespace So.GameObjectsSo.Building
{
    public enum BuildingType
    {
        FarmField,
        BasicHouse
    }
    
    [CreateAssetMenu(fileName = "BuildingSo", menuName = "So/BuildingSo", order = 0)]
    public class BuildingSo : ItemSo
    {
        public Sprite itemConstructionSprite;
        public BuildingType buildingType;
        public float finishTimeToConstruction = 10f;
        public GameObject buildPrefab;
    }
}