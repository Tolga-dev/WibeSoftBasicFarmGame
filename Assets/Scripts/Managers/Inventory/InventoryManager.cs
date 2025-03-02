using System;
using System.Collections.Generic;
using System.Linq;
using Managers.Base;
using So;
using So.GameObjectsSo.Base;
using So.GameObjectsSo.Building;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Managers.Inventory
{
    [Serializable]
    public class CategorySlots
    {
        public InventoryCategories category;
        public List<InventorySlot> slots = new List<InventorySlot>();
        public Button categoryPanelOpenButton;
        public Transform categoryPanel;

    }
    
    public class InventoryManager : ManagerBase
    {
        [Header("Slots")] 
        public List<CategorySlots> categorySlotsList = new();
        
        public InventoryItem itemPrefab;
        public Transform draggableItemsParent;

        public Transform informationPanel;
        public TextMeshProUGUI informationText;
        public Button sellThisItemButton;
        public Button useThisItemButton;
        public InventorySlot currentSlot;
        
        public TextMeshProUGUI goldText;
        public TextMeshProUGUI silverText;
        private void Start()
        {
            foreach (var categorySlots in categorySlotsList)
            {
                categorySlots.categoryPanelOpenButton.onClick.AddListener(() =>
                {
                    foreach (var slots in categorySlotsList)
                    {
                        slots.categoryPanel.gameObject.SetActive(false);
                    }
                    categorySlots.categoryPanel.gameObject.SetActive(true);
                });
            }

            foreach (var inventoryCategory in gameManager.saveManager.gameDataSo.inventorySo.inventoryCategories)
            {
                foreach (var inventoryItem in inventoryCategory.inventoryItems)
                {
                    SpawnInventoryUIItem(inventoryItem.itemSo, inventoryItem.count);
                }
            }
            
            sellThisItemButton.onClick.AddListener(() =>
            {
                Debug.Log("Sell");
                var currencies = currentSlot.inventoryItem.currentItem.currencies;
                gameManager.saveManager.gameDataSo.inventorySo.AddCurrencies(currencies);
                UpdateCurrencyUI();
                informationPanel.gameObject.SetActive(false);
                HandleAmount();
            });
            useThisItemButton.onClick.AddListener(() =>
            {
                Debug.Log("Use");
                var buildingSo = (BuildingSo)currentSlot.inventoryItem.currentItem;
                gameManager.buildingManager.InitializeNewBuilding(buildingSo.buildPrefab);
                informationPanel.gameObject.SetActive(false);
                HandleAmount();
                draggableItemsParent.gameObject.SetActive(false);
            });
            
            UpdateCurrencyUI();
        }

        private void HandleAmount()
        {
            gameManager.saveManager.gameDataSo.inventorySo.RemoveInventoryItemByItemSo(currentSlot.inventoryItem.currentItem, 1);
            currentSlot.HandleAmount();
        }

        private void UpdateCurrencyUI()
        {
            var inventorySo = gameManager.saveManager.gameDataSo.inventorySo;
            silverText.text = "Silver:" + inventorySo.GetCurrencies(CurrencyEnum.Silver).currencyVal;
            goldText.text = "Gold:" +inventorySo.GetCurrencies(CurrencyEnum.Gold).currencyVal;
        }
        public int SpawnInventoryUIItem(ItemSo item, int amount)
        {
            var categorySlots = GetSlot(item);
            if (categorySlots == null)
                return amount;
            
            var result = SearchForSameItem(categorySlots,item,amount);
            if (result == 0)
                return 0;

            result = SearchForEmptyItem(categorySlots,item, amount);
            if (result != 0)
            {
                Debug.Log("Inv Full!");
                return result;
            }
            return 0;
        }
        private int SearchForEmptyItem(CategorySlots categorySlots,ItemSo inventoryItem, int amount)
        {
            foreach (var inventorySlot in categorySlots.slots)
            {
                if (inventorySlot.inventoryItem != null) 
                    continue;

                var createdItem = CreateNewInstance(inventorySlot,inventoryItem, amount);
                inventorySlot.inventoryItem = createdItem;
                break;
            }
            return 0;
        }
        public InventoryItem CreateNewInstance(InventorySlot inventorySlot, ItemSo item, int amountToAdd)
        {
            var createdItem= Instantiate(itemPrefab, inventorySlot.transform);
            createdItem.SetItemData(item, inventorySlot);
                
            createdItem.amount = amountToAdd;
            createdItem.SetAmount();
            
            createdItem.inventoryManager = this;
            
            return createdItem;
        }
        
        private int SearchForSameItem(CategorySlots categorySlots,ItemSo inventoryItem, int amount)
        {
            foreach (var inventorySlot in categorySlots.slots)
            {
                var invItem = inventorySlot.inventoryItem;

                if(invItem == null) continue;
                
                if (invItem.currentItem == inventoryItem)
                {
                    invItem.amount += amount;
                    inventorySlot.inventoryItem.SetAmount();
                    return 0;
                }                
            }
            return amount;
        }

        public CategorySlots GetSlot(ItemSo inventoryItem)
        {
            CategorySlots categorySlots = null;
            foreach (var inventorySlot in categorySlotsList)
            {
                if (inventorySlot.category == inventoryItem.inventoryCategory)
                {
                    categorySlots = inventorySlot;
                    break;
                }
            }

            return categorySlots;
        }

        public void SetDescriptionPanel(InventorySlot currentSlotVal,string descriptionPanelText)
        {
            currentSlot = currentSlotVal;
            informationPanel.gameObject.SetActive(true);
            informationText.text = descriptionPanelText;

            if (currentSlotVal.inventoryItem.currentItem.GetType() != typeof(BuildingSo))
            {
                useThisItemButton.gameObject.SetActive(false);
            }
            else
            {
                useThisItemButton.gameObject.SetActive(true);
            }
        }
    }
}
