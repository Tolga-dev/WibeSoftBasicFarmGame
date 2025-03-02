using System;
using Controller.Base;
using Managers.Base;
using Managers.Inventory;
using So.GameObjectsSo.Base;
using So.GameObjectsSo.Building;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Controller.InventoryControllers
{
    [Serializable]
    public class InventoryItemController : ControllerBase
    {
        public override void Initialization(ManagerBase managerBaseVal)
        {
            base.Initialization(managerBaseVal);
            foreach (var inventoryCategory in ManagerBase.gameManager.saveManager.gameDataSo.inventorySo.inventoryCategories)
            {
                foreach (var inventoryItem in inventoryCategory.inventoryItems)
                {
                    InventoryManager.inventoryItemController.SpawnInventoryUIItem(inventoryItem.itemSo, inventoryItem.count);
                }
            }
        }

        public void UseCurrentItem(InventorySlot currentSlot)
        {
            var buildingSo = (BuildingSo)currentSlot.inventoryItem.currentItem;
            ManagerBase.gameManager.buildingManager.InitializeNewBuilding(buildingSo.buildPrefab);
            InventoryManager.informationPanel.gameObject.SetActive(false);
            HandleAmount(currentSlot);
            InventoryManager.draggableItemsParent.gameObject.SetActive(false);
        }

        public void HandleAmount(InventorySlot currentSlot)
        {
            ManagerBase.gameManager.saveManager.gameDataSo.inventorySo.RemoveInventoryItemByItemSo(currentSlot.inventoryItem.currentItem, 1);
            currentSlot.HandleAmount();
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
        private int SearchForEmptyItem(InventorySlots categorySlots,ItemSo inventoryItem, int amount)
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
            var createdItem= Object.Instantiate(InventoryManager.itemPrefab, inventorySlot.transform);
            createdItem.SetItemData(item, inventorySlot);
                
            createdItem.amount = amountToAdd;
            createdItem.SetAmount();
            
            createdItem.inventoryManager = InventoryManager;
            return createdItem;
        }
        
        private int SearchForSameItem(InventorySlots categorySlots,ItemSo inventoryItem, int amount)
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

        public InventorySlots GetSlot(ItemSo inventoryItem)
        {
            InventorySlots categorySlots = null;
            foreach (var inventorySlot in InventoryManager.categorySlotsList)
            {
                if (inventorySlot.category == inventoryItem.inventoryCategory)
                {
                    categorySlots = inventorySlot;
                    break;
                }
            }

            return categorySlots;
        }

        public InventoryManager InventoryManager => (InventoryManager)ManagerBase;
    }
}