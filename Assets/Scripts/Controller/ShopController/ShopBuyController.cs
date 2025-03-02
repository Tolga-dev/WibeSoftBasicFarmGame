using System;
using System.Collections.Generic;
using Controller.Base;
using Managers.Shop;
using So;
using So.GameObjectsSo.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.ShopController
{
   [Serializable]
    public class ShopBuyController : ControllerBase
    {
        public void BuyThisItem(ItemSo itemSo)
        {
            var shopManager = (ShopManager)ManagerBase;
            var inventoryManager = shopManager.gameManager.inventoryManager;
            var inventorySo = shopManager.gameManager.saveManager.gameDataSo.inventorySo;
            var itemCurrency = itemSo.currencies[0];
            var currencies = inventorySo.GetCurrencies(itemCurrency.currencyEnum);

            if (currencies.currencyVal < itemCurrency.currencyVal)
            {
                Debug.Log("Cannot buy!");
                return;
            }

            currencies.currencyVal -= itemCurrency.currencyVal;
            inventoryManager.inventoryItemController.SpawnInventoryUIItem(itemSo, 1);
            inventorySo.AddInventoryItem(itemSo, 1);
            inventoryManager.inventoryCurrencyController.UpdateCurrencyUI();
        }
    }
}