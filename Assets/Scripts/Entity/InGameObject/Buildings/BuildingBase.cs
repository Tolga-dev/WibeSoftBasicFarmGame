using System;
using Entity.InGameObject.Base;
using So.GameObjectsSo.Building;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entity.InGameObject.Buildings
{
    public class BuildingBase : InGameObjectBase
    {
        public BuildingInGameSaveSo buildingInGameSaveSo = null;
        
        public bool isOnConstruction = false;
        public ProductionState currentState = ProductionState.Empty;

        public override bool CanClickToGameObject()
        {
            if (isOnConstruction)
            {
                Debug.Log("On Construction");
                return false;
            }
            return base.CanClickToGameObject();
        }
        public override void Place()
        {
            base.Place();
            OnStartConstruction();
        }
        private void OnStartConstruction()
        {
            isOnConstruction = true;
            var buildingSo = ((BuildingSo)itemSo);
            
            itemSpriteRenderer.sprite = buildingSo.itemConstructionSprite;
            timerPanel.gameObject.SetActive(true);

            var constructionEndAction = new Action(() => { isOnConstruction = false; });
            timerPanel.StartATime(itemSo,buildingSo.finishTimeToConstruction, EndOfTimeAction + constructionEndAction);

            if (buildingInGameSaveSo == null || (buildingInGameSaveSo != null && buildingInGameSaveSo.buildingSo == null))
            {
                CreateNewInstance(buildingSo);
            }
        }

        private void CreateNewInstance(BuildingSo buildingSo)
        {
            buildingInGameSaveSo = new BuildingInGameSaveSo()
            {
                buildingSo = buildingSo,
                position = transform.position
            };
            
            var buildingSaveSos = GameManager.saveManager.gameDataSo.buildingsSo.buildingSaveSos;
            buildingSaveSos.Add(buildingInGameSaveSo);
        }
        protected void SetState(ProductionState state)
        {
            currentState = state;
            buildingInGameSaveSo.productionState = state;
        }
        protected void SetLeftTime(float leftTime)
        {
        }

        protected void SetLeftTimeReduce(float leftTime)
        {
        }
        public virtual void SetActionBuilding(BuildingInGameSaveSo building)
        {
            EndOfTimeAction();
            isOnConstruction = false;
        }
    }

}