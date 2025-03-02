using System;
using System.Collections.Generic;
using System.Linq;
using Controller.InventoryControllers;
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
        public Button categoryPanelOpenButton;
        public Transform categoryPanel;
    }
    
    [Serializable]
    public class InventorySlots : CategorySlots
    {
        public List<InventorySlot> slots = new List<InventorySlot>();
    }
    
    public class InventoryManager : ManagerBase
    {
        [Header("Controllers")] 
        public InventorySlotsController inventorySlotsController;
        public InventoryCurrencyController inventoryCurrencyController;
        public InventoryItemController inventoryItemController;
        
        [Header("Slots")] 
        public List<InventorySlots> categorySlotsList = new();
        public InventoryItem itemPrefab;
        public Transform draggableItemsParent;
        
        [Header("Info")] 
        public Transform informationPanel;
        public TextMeshProUGUI informationText;
        public InventorySlot currentSlot;
        
        [Header("Currency")] 
        public TextMeshProUGUI goldText;
        public TextMeshProUGUI silverText;
        public Button sellThisItemButton;
        public Button useThisItemButton;
        
        private void Start()
        {
            inventorySlotsController.Initialization(this);
            inventoryCurrencyController.Initialization(this);
            inventoryItemController.Initialization(this);
            
            sellThisItemButton.onClick.AddListener(() =>
            {
                Debug.Log("Sell");
                inventoryCurrencyController.SellCurrentItem(currentSlot);
            });

            useThisItemButton.onClick.AddListener(() =>
            {
                Debug.Log("Use");
                inventoryItemController.UseCurrentItem(currentSlot);
            });
            inventoryCurrencyController.UpdateCurrencyUI();
        }

        public void SetDescriptionPanel(InventorySlot currentSlotVal, string descriptionPanelText)
        {
            currentSlot = currentSlotVal;
            informationPanel.gameObject.SetActive(true);
            informationText.text = descriptionPanelText;

            useThisItemButton.gameObject.
                SetActive(currentSlotVal.inventoryItem.currentItem.GetType() == typeof(BuildingSo));
        }
   
    }
}
