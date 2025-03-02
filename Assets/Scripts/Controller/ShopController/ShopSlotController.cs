using System;
using System.Collections.Generic;
using Controller.Base;
using Managers.Shop;
using So;
using So.GameObjectsSo.Base;
using TMPro;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Controller.ShopController
{
    [Serializable]
    public class ShopSlotController : ControllerBase
    {
        public void InitializeShopSlots()
        {
            var shopManager = (ShopManager)ManagerBase;
            var buildingsSo = shopManager.gameManager.saveManager.gameDataSo.buildingsSo;
            var productsSo = shopManager.gameManager.saveManager.gameDataSo.seedSo;

            foreach (var shopSlots in shopManager.shopSlotsList)
            {
                shopSlots.categoryPanelOpenButton.onClick.AddListener(() =>
                {
                    CloseAllSlots();
                    shopSlots.categoryPanel.gameObject.SetActive(true);
                });

                if (shopSlots.category == InventoryCategories.Buildings)
                    PopulateShopSlots(buildingsSo.buildingSo, shopSlots);
                else if (shopSlots.category == InventoryCategories.Products)
                    PopulateShopSlots(productsSo.seedSo, shopSlots);
            }
        }

        private void PopulateShopSlots<T>(List<T> items, ShopSlots shopSlots) where T : ItemSo
        {
            var shopManager = (ShopManager)ManagerBase;
            foreach (var item in items)
            {
                var createdBuy = Object.Instantiate(shopManager.buy, shopSlots.buyButtonSpawnPoint);
                var createdBuyText = createdBuy.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                var createdBuyIcon = createdBuy.transform.GetChild(1).GetComponent<Image>();
                var currency = item.currencies[0];

                createdBuyIcon.sprite = item.itemSprite;
                createdBuyText.text = item.itemName + " " + currency.currencyVal + " " + currency.currencyEnum;

                createdBuy.onClick.AddListener(() =>
                {
                    shopManager.shopBuyController.BuyThisItem(item);
                });
            }
        }

        private void CloseAllSlots()
        {
            var shopManager = (ShopManager)ManagerBase;
            foreach (var slots in shopManager.shopSlotsList)
            {
                slots.categoryPanel.gameObject.SetActive(false);
            }
        }
    }
}