using System;
using System.Collections.Generic;
using Controller.ShopController;
using Managers.Base;
using Managers.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Managers.Shop
{
    [Serializable]
    public class ShopSlots : CategorySlots
    {
        public Transform buyButtonSpawnPoint;
    }
    public class ShopManager : ManagerBase
    {
        public List<ShopSlots> shopSlotsList = new();
        public Button buy;
 
        public ShopSlotController shopSlotController;
        public ShopBuyController shopBuyController;

        private void Start()
        {
            shopSlotController.Initialization(this);
            shopBuyController.Initialization(this);
            shopSlotController.InitializeShopSlots();
            buy.gameObject.SetActive(false);
        }
    }
}