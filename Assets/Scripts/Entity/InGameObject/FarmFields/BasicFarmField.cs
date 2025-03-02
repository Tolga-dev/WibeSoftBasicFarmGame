using System;
using System.Collections;
using System.Collections.Generic;
using Entity.InGameObject.Base;
using Entity.InGameObject.Buildings;
using Entity.InGameObject.Controllers.UI;
using So;
using So.GameObjectsSo.Seed;
using So.Seed;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Entity.InGameObject.FarmFields
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
                var createdObject = Instantiate(informationUIPanel.informationButton, informationUIPanel.informationPanel);

                var button = createdObject.startTheAction;
                var icon = createdObject.productItemSprite;
                var itemCount = createdObject.productItemCount;
                
                icon.sprite = inventoryItem.itemSo.itemSprite;
                itemCount.text = inventoryItem.count.ToString();
                
                informationUIPanel.createdButtons.Add(createdObject.gameObject);
                
                button.onClick.AddListener(() =>
                {
                    plantItemSo = (SeedSo) inventoryItem.itemSo;
                    PlantSeed();
                    informationUIPanel.ClosePanel(button);
                });
            }
            informationUIPanel.informationButton.gameObject.SetActive(false);
        }
 
        private void PlantSeed()
        {
            SetState(ProductionState.Started);
            StartCoroutine(GrowPlant());
        }

        private IEnumerator GrowPlant()
        {
            timerPanel.gameObject.SetActive(true);
            timerPanel.StartATime(plantItemSo,plantItemSo.finishTimeToFinish, EndOfTimeAction);
            
            SetState(ProductionState.OnProduction);

            var growthTime = plantItemSo.finishTimeToFinish;
            SetLeftTime(growthTime);
            
            var seedGrowSprites = plantItemSo.seedGrowSprites;
            var interval = growthTime / seedGrowSprites.Count;
            foreach (var t in seedGrowSprites)
            {
                plantItemRenderer.sprite = t;
                SetLeftTimeReduce(interval);
                yield return new WaitForSeconds(interval);
            }
            SetState(ProductionState.ReadyToHarvest);
        }
        private void Harvest()
        {
            if (currentState == ProductionState.ReadyToHarvest)
            {
                UpdateInventory();
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