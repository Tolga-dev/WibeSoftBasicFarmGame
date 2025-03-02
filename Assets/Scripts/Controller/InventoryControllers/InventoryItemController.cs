using System;
using Controller.Base;
using Managers.Inventory;
using So.GameObjectsSo.Building;
using UnityEngine;

namespace Controller.InventoryControllers
{
    [Serializable]
    public class InventoryItemController : ControllerBase
    {
        public InventoryItem itemPrefab;
        public Transform draggableItemsParent;

        public void UseCurrentItem(InventorySlot currentSlot)
        {
            var buildingSo = (BuildingSo)currentSlot.inventoryItem.currentItem;
            ManagerBase.gameManager.buildingManager.InitializeNewBuilding(buildingSo.buildPrefab);
            InventoryManager.informationPanel.gameObject.SetActive(false);
            HandleAmount(currentSlot);
            draggableItemsParent.gameObject.SetActive(false);
        }

        public void HandleAmount(InventorySlot currentSlot)
        {
            ManagerBase.gameManager.saveManager.gameDataSo.inventorySo.RemoveInventoryItemByItemSo(currentSlot.inventoryItem.currentItem, 1);
            currentSlot.HandleAmount();
        }

        public InventoryManager InventoryManager => (InventoryManager)ManagerBase;
    }
}