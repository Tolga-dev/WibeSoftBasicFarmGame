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
        }

        protected void SetState(ProductionState state)
        {
            currentState = state;
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