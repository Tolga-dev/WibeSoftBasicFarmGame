using System;
using Entity.InGameObject.Base;
using Entity.InGameObject.FarmFields;
using So.GameObjectsSo.Building;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entity.InGameObject.Buildings
{
    public class BuildingBase : InGameObjectBase
    {
        public BuildingInGameSaveSo buildingInGameSaveSo;
        public bool isOnConstruction;
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
        }
        public void SetState(ProductionState state)
        {
            currentState = state;
            buildingInGameSaveSo.productionState = currentState;
        }

        public void SetLeftTime(float leftTime)
        {
            buildingInGameSaveSo.leftTime = leftTime;
        }
        public void SetLeftTimeReduce(float leftTime)
        {
            buildingInGameSaveSo.leftTime -= leftTime;

            if (buildingInGameSaveSo.leftTime < 0)
                buildingInGameSaveSo.leftTime = 0;
        }
        public virtual void SetActionBuilding(BuildingInGameSaveSo building)
        {
            EndOfTimeAction();
            isOnConstruction = false;
        }
    }

}