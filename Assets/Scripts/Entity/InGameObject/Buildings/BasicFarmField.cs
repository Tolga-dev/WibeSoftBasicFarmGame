using System;
using System.Collections;
using System.Collections.Generic;
using Entity.InGameObject.Controllers.UI;
using So;
using So.GameObjectsSo.Seed;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Entity.InGameObject.Buildings
{

    public enum ProductionState
    {
        Empty,
        Started,
        OnProduction,
        ReadyToHarvest
    }

    [Serializable]
    public class InformationUIPanel
    {
        [FormerlySerializedAs("seedPanel")] 
        public Transform informationPanel;

        public InformationButton informationButton;
        public List<GameObject> createdButtons = new List<GameObject>();

        public void ClosePanel(Button button)
        {
            foreach (var createdButton in createdButtons)
            {
                if(createdButton != button.gameObject)
                    Object.Destroy(createdButton.gameObject);
            }
                    
            Object.Destroy(button.gameObject, 1);
            informationPanel.gameObject.SetActive(false);
            informationButton.gameObject.SetActive(true);
        }
    }
    
    public class BasicFarmField : BuildingBase
    {
        public InformationUIPanel informationUIPanel;
        
        public SpriteRenderer plantItemRenderer;
        public SeedSo plantItemSo;
        
        public override bool CanClickToGameObject()
        {
            var canClick = base.CanClickToGameObject();

            if (canClick == false)
                return false;

            switch (currentState)
            {
                case ProductionState.ReadyToHarvest:
                    Harvest();
                    return false;
                case ProductionState.Empty:
                    OpenSeedPanel();
                    break;
            }
            
            return true;
        }

        private void OpenSeedPanel()
        {
            informationUIPanel.informationPanel.gameObject.SetActive(true);

            var category = GameManager.saveManager.gameDataSo.inventorySo.GetInventoryCategory(InventoryCategories.Products);
            foreach (var inventoryItem in category.inventoryItems)
            {
                if(inventoryItem.itemSo.GetType() != typeof(SeedSo))
                    continue;
                
                var createdObject = Instantiate(informationUIPanel.informationButton, informationUIPanel.informationPanel);

                var button = createdObject.startTheAction;
                var icon = createdObject.productItemSprite;
                var itemCount = createdObject.productItemCount;
                
                icon.sprite = inventoryItem.itemSo.itemSprite;
                itemCount.text = inventoryItem.count.ToString();
                
                informationUIPanel.createdButtons.Add(createdObject.gameObject);
                
                button.onClick.AddListener(() =>
                {
                    if(inventoryItem.count <= 0)
                        return;
                    
                    plantItemSo = (SeedSo) inventoryItem.itemSo;
                    PlantSeed();
                    informationUIPanel.ClosePanel(button);
                });
            }
            informationUIPanel.informationButton.gameObject.SetActive(false);
        }
 
        private void PlantSeed(float leftTime = 0)
        {
            SetState(ProductionState.Started);
            GrowPlant(leftTime);
        }
        
        private void GrowPlant(float leftTime = 0)
        {
            SetState(ProductionState.OnProduction);
            timerPanel.gameObject.SetActive(true);
            
            var growthTime = leftTime == 0 ? plantItemSo.finishForProduction : leftTime;
            SetLeftTime(growthTime);

            timerPanel.StartATime(plantItemSo, growthTime,
                EndOfTimeAction, UpdateTimeAction);
        }

        private void UpdateTimeAction(float currentTime)
        {
            var growthTime = plantItemSo.finishForProduction;
            buildingInGameSaveSo.itemSo = plantItemSo;
            var seedGrowSprites = plantItemSo.seedGrowSprites;
            var interval = growthTime / seedGrowSprites.Count;

            var spriteIndex = Mathf.Clamp(Mathf.FloorToInt(currentTime / interval), 0, seedGrowSprites.Count - 1);
            plantItemRenderer.sprite = seedGrowSprites[spriteIndex];

            SetLeftTime(growthTime - currentTime);
        }


        protected override void EndOfTimeAction()
        {
            base.EndOfTimeAction();
            
            if(currentState == ProductionState.Empty)
                return;
            
            SetState(ProductionState.ReadyToHarvest);
        }
        public override void SetActionBuilding()
        {
            base.SetActionBuilding();
            
            currentState = buildingInGameSaveSo.productionState;
            
            if(currentState == ProductionState.Empty)
                return;

            plantItemSo = (SeedSo)buildingInGameSaveSo.itemSo;
            PlantSeed(buildingInGameSaveSo.leftTime);
        }
        
        private void Harvest()
        {
            if (currentState == ProductionState.ReadyToHarvest)
            {
                UpdateInventory();
                SetLeftTime(0);
                plantItemRenderer.sprite = null;
                SetState(ProductionState.Empty);
            }
        }

        private void UpdateInventory()
        {
            var amountOfProduct = plantItemSo.amountOfProduct;
            GameManager.saveManager.gameDataSo.inventorySo.AddInventoryItem(plantItemSo, amountOfProduct);
            GameManager.inventoryManager.SpawnInventoryUIItem(plantItemSo, amountOfProduct);
        }
    }

}