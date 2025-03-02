using System;
using System.Collections.Generic;
using Entity.InGameObject.FarmFields;
using UnityEngine;

namespace So.GameObjectsSo.Building
{
    [CreateAssetMenu(fileName = "BuildingsSo", menuName = "So/BuildingsSo", order = 0)]
    public class BuildingsSo : ScriptableObject
    {
        public List<BuildingSo> buildingSo = new List<BuildingSo>();
        public List<BuildingInGameSaveSo> buildingSaveSos = new List<BuildingInGameSaveSo>();
    }

    [Serializable]
    public class BuildingInGameSaveSo
    {
        public BuildingSo buildingSo;
        public Vector3 position;
        
        public ProductionState productionState;
        public float leftTime;
    }
}