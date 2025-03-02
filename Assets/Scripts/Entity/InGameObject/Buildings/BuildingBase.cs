using System;
using Entity.InGameObject.Base;
using So;
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

        public override bool CanClickToGameObject() => !isOnConstruction && base.CanClickToGameObject();

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
            timerPanel.StartATime(itemSo,buildingSo.finishForProduction, EndOfTimeAction + constructionEndAction);

            if (IsItCreatedFromUser())
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
            
            var buildingSaveSos = BuildingsSo.buildingSaveSos;
            buildingSaveSos.Add(buildingInGameSaveSo);
        }
       
        public virtual void SetActionBuilding()
        {
            base.EndOfTimeAction();
            isOnConstruction = false;
        }
        
        private bool IsItCreatedFromUser() =>  buildingInGameSaveSo == null ||
        (buildingInGameSaveSo != null && buildingInGameSaveSo.buildingSo == null);
        protected void SetState(ProductionState state) => buildingInGameSaveSo.productionState = currentState = state;
        protected void SetLeftTime(float leftTime) => buildingInGameSaveSo.leftTime = leftTime;
        public BuildingsSo BuildingsSo => GameManager.saveManager.gameDataSo.buildingsSo;
        public InventorySo InventorySo => GameManager.saveManager.gameDataSo.inventorySo;
    }

}