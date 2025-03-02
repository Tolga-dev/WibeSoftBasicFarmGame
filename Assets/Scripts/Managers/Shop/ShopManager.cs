using System;
using System.Collections.Generic;
using Managers.Base;
using Managers.Inventory;
using So;
using So.GameObjectsSo.Base;
using TMPro;
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

        private void Start()
        {
            var buildingsSo = gameManager.saveManager.gameDataSo.buildingsSo;
            var productsSo = gameManager.saveManager.gameDataSo.seedSo;

            foreach (var shopSlots in shopSlotsList)
            {
                InitializeShopSlot(shopSlots);

                if (shopSlots.category == InventoryCategories.Buildings)
                    CreateShopItems(shopSlots, buildingsSo.buildingSo);
                else if (shopSlots.category == InventoryCategories.Products)
                    CreateShopItems(shopSlots, productsSo.seedSo);
            }

            buy.gameObject.SetActive(false);
        }

        private void InitializeShopSlot(ShopSlots shopSlots)
        {
            shopSlots.categoryPanelOpenButton.onClick.AddListener(() =>
            {
                foreach (var slots in shopSlotsList)
                    slots.categoryPanel.gameObject.SetActive(false);

                shopSlots.categoryPanel.gameObject.SetActive(true);
            });
        }

        private void CreateShopItems<T>(ShopSlots shopSlots, List<T> items) where T : ItemSo
        {
            foreach (var item in items)
            {
                var createdBuy = Instantiate(buy, shopSlots.buyButtonSpawnPoint);
                SetShopItemUI(createdBuy, item);

                createdBuy.onClick.AddListener(() => BuyThisItem(item));
            }
        }

        private void SetShopItemUI(Button createdBuy, ItemSo item)
        {
            var createdBuyText = createdBuy.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            var createdBuyIcon = createdBuy.transform.GetChild(1).GetComponent<Image>();
            var currency = item.currencies[0];

            createdBuyIcon.sprite = item.itemSprite;
            createdBuyText.text = $"{item.itemName} {currency.currencyVal} {currency.currencyEnum}";
        }

        private void BuyThisItem(ItemSo itemSo)
        {
            var inventoryManager = gameManager.inventoryManager;
            var inventorySo = gameManager.saveManager.gameDataSo.inventorySo;
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