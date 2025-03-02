using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using Entity.InGameObject.Base;
using Entity.InGameObject.Controllers.UI;
using So;
using So.GameObjectsSo.Building;
using So.GameObjectsSo.Seed;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Entity.InGameObject.Buildings
{
    public class BasicHouse : BuildingBase
    {
        public InformationUIPanel informationUIPanel;
        public SpriteRenderer resultRender;
        public Recipe currentRecipe;

        public float buildingBooster = 0.5f; 
        public override bool CanClickToGameObject()
        {
            var canClick = base.CanClickToGameObject();

            if (canClick == false)
            {
                Debug.Log("On Construction");
                return false;
            }

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

            var craftSo = GameManager.saveManager.gameDataSo.craftSo;

            foreach (var recipe in craftSo.recipes)
            {
                CreateRecipe(recipe);
            }

            informationUIPanel.informationButton.gameObject.SetActive(false);
        }
        
        private void CreateRecipe(Recipe recipe)
        {
            var createdObject = InstantiateCraftButton();
            informationUIPanel.createdButtons.Add(createdObject.gameObject);

            float totalTime = GetTotalTimeOfRecipe(recipe);
            createdObject.totalTime.text = totalTime.ToString(CultureInfo.InvariantCulture);
            
            createdObject.startTheAction.onClick.AddListener(() =>
            {
                var inventorySo = GameManager.saveManager.gameDataSo.inventorySo;
                foreach (var currentItem in recipe.ingredient)
                {
                    if (ItHasEnoughItem(currentItem, inventorySo) == false)
                    {
                        Debug.Log("No Enough!");
                        return;
                    }
                }
                
                currentRecipe = recipe;
                HandleAmount();
                StartCooking(totalTime);
                informationUIPanel.ClosePanel(createdObject.startTheAction);
            });
            
            CreateIngredientObjects(recipe, createdObject);
            Instantiate(createdObject.equalIcon, createdObject.transform);
            CreateResultObject(recipe, createdObject);
            DisableUnusedIcons(createdObject);
        }

        public float GetTotalTimeOfRecipe(Recipe recipe)
        {
            float totalTime = 0;
            foreach (var craftItem in recipe.ingredient)
            {
                var item = (SeedSo)craftItem.itemSo;
                totalTime += item.finishForProduction;
            }

            totalTime *= buildingBooster;

            return totalTime;
        }

        private void HandleAmount()
        {
            var inventorySo = GameManager.saveManager.gameDataSo.inventorySo;

            foreach (var craftItem in currentRecipe.ingredient)
            {
                inventorySo.RemoveInventoryItemByItemSo(craftItem.itemSo, craftItem.amount);
            }
        }

        private void StartCooking(float totalTime)
        {
            SetState(ProductionState.OnProduction);
            SetLeftTime(totalTime);

            var currentSo = currentRecipe.result.itemSo;
            buildingInGameSaveSo.itemSo = currentSo;

            timerPanel.gameObject.SetActive(true);
            timerPanel.StartATime(currentRecipe.result.itemSo,totalTime, EndOfTimeAction,UpdateTimeAction);
        }
        
        private void UpdateTimeAction(float currentTime)
        {
            var growthTime = currentRecipe.result.itemSo.finishForProduction;
            SetLeftTime(growthTime - currentTime);
        }
        
        protected override void EndOfTimeAction()
        {
            base.EndOfTimeAction();
            
            if(currentState == ProductionState.Empty)
                return;
            
            SetLeftTime(0);
            resultRender.sprite = currentRecipe.result.itemSo.itemSprite;
            SetState(ProductionState.ReadyToHarvest);
        }

        private CraftInformationButton InstantiateCraftButton()
        {
            return (CraftInformationButton)Instantiate(informationUIPanel.informationButton,
                informationUIPanel.informationPanel);
        }

        private void CreateIngredientObjects(Recipe recipe, CraftInformationButton createdObject)
        {
            int countIngredient = recipe.ingredient.Count;
            foreach (var ingredient in recipe.ingredient)
            {
                var ingredientObject = Instantiate(createdObject.productItemSprite, createdObject.transform);
                ingredientObject.sprite = ingredient.itemSo.itemSprite;
                SetIngredientAmount(ingredientObject, ingredient.amount);
                if (countIngredient > 1)
                {
                    CreatePlusIcon(createdObject);
                    countIngredient--;
                }
            }
        }

        private void SetIngredientAmount(Image ingredientObject, int amount)
        {
            var amountText = ingredientObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            amountText.text = amount.ToString();
        }

        private void CreatePlusIcon(CraftInformationButton createdObject)
        {
            var plusIcon = Instantiate(createdObject.plusIcon, createdObject.transform);
            plusIcon.text = "+";
        }

        private void CreateResultObject(Recipe recipe, CraftInformationButton createdObject)
        {
            var resultObject = Instantiate(createdObject.productItemSprite, createdObject.transform);
            resultObject.sprite = recipe.result.itemSo.itemSprite;
            SetIngredientAmount(resultObject, recipe.result.amount);
            Debug.Log(recipe.result.itemSo.itemName + " " + recipe.result.amount);
        }

        private void DisableUnusedIcons(CraftInformationButton createdObject)
        {
            createdObject.plusIcon.gameObject.SetActive(false);
            createdObject.equalIcon.gameObject.SetActive(false);
            createdObject.productItemSprite.gameObject.SetActive(false);
            createdObject.productItemCount.gameObject.SetActive(false);
        }

        private bool ItHasEnoughItem(CraftItem ingredient, InventorySo inventorySo)
        {
            var item = inventorySo.GetInventoryItemByItemSo(ingredient.itemSo);
            if (item == null)
                return false;
            
            if (item.count >= ingredient.amount)
                return true;
            return false;
        }
        
        private void Harvest()
        {
            if (currentState == ProductionState.ReadyToHarvest)
            {
                UpdateInventory();
                SetLeftTime(0);
                resultRender.sprite = null;
                SetState(ProductionState.Empty);
            }
        }
        private void UpdateInventory()
        {
            var amountOfProduct = currentRecipe.result.amount;
            GameManager.saveManager.gameDataSo.inventorySo.AddInventoryItem(currentRecipe.result.itemSo, amountOfProduct);
            GameManager.inventoryManager.inventoryItemController.SpawnInventoryUIItem(currentRecipe.result.itemSo, amountOfProduct);
        }
        public override void SetActionBuilding()
        {
            base.SetActionBuilding();

            currentState = buildingInGameSaveSo.productionState;
            
            if(currentState == ProductionState.Empty)
                return;
            
            currentRecipe = new Recipe()
            {
                result = new CraftItem()
                {
                    amount = 1,
                    itemSo = buildingInGameSaveSo.itemSo
                }
            };
            
            StartCooking(buildingInGameSaveSo.leftTime);
        }
        
    }
}