using System;
using System.Collections.Generic;
using Managers.Base;
using Managers.Inventory;
using So;
using So.GameObjectsSo.Base;
using So.GameObjectsSo.Building;
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
                shopSlots.categoryPanelOpenButton.onClick.AddListener(() =>
                {
                    foreach (var slots in shopSlotsList)
                    {
                        slots.categoryPanel.gameObject.SetActive(false);
                    }
                    shopSlots.categoryPanel.gameObject.SetActive(true);
                });

                if (shopSlots.category == InventoryCategories.Buildings)
                {
                    foreach (var buildingSo in buildingsSo.buildingSo)
                    {
                        var createdBuy = Instantiate(buy, shopSlots.buyButtonSpawnPoint);
                        var createdBuyText = createdBuy.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                        var createdBuyIcon = createdBuy.transform.GetChild(1).GetComponent<Image>();
                        var currency = buildingSo.currencies[0];

                        createdBuyIcon.sprite = buildingSo.itemSprite;
                        createdBuyText.text = buildingSo.itemName + " " +currency.currencyVal + " " + currency.currencyEnum;
                        
                        createdBuy.onClick.AddListener(
                            (() =>
                            {
                                BuyThisItem(buildingSo);
                            }));
                    }
                }
                else if (shopSlots.category == InventoryCategories.Products)
                {
                    foreach (var seedSo in productsSo.seedSo)
                    {
                        var createdBuy = Instantiate(buy, shopSlots.buyButtonSpawnPoint);
                        var createdBuyText = createdBuy.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                        var createdBuyIcon = createdBuy.transform.GetChild(1).GetComponent<Image>();
                        var currency = seedSo.currencies[0];
                        
                        createdBuyIcon.sprite = seedSo.itemSprite;
                        createdBuyText.text = seedSo.itemName + " " +currency.currencyVal + " " + currency.currencyEnum;
                        
                        createdBuy.onClick.AddListener(
                            (() =>
                            {
                                BuyThisItem(seedSo);
                            }));
                    }
                }
            }
            buy.gameObject.SetActive(false);
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

            inventoryManager.SpawnInventoryUIItem(itemSo,1);
            inventorySo.AddInventoryItem(itemSo,1);
            inventoryManager.UpdateCurrencyUI();
        }
    }
}