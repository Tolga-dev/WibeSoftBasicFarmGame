using System;
using Controller.Base;
using Managers.Inventory;
using So;
using TMPro;

namespace Controller.InventoryControllers
{
    [Serializable]
    public class InventoryCurrencyController : ControllerBase
    {
        public void UpdateCurrencyUI()
        {
            var inventorySo = ManagerBase.gameManager.saveManager.gameDataSo.inventorySo;
            InventoryManager.silverText.text = "Silver:" + inventorySo.GetCurrencies(CurrencyEnum.Silver).currencyVal;
            InventoryManager.goldText.text = "Gold:" + inventorySo.GetCurrencies(CurrencyEnum.Gold).currencyVal;
        }

        public void SellCurrentItem(InventorySlot currentSlot)
        {
            var currencies = currentSlot.inventoryItem.currentItem.currencies;
            ManagerBase.gameManager.saveManager.gameDataSo.inventorySo.AddCurrencies(currencies);
            
            UpdateCurrencyUI();
            
            InventoryManager.informationPanel.gameObject.SetActive(false);
            InventoryManager.inventoryItemController.HandleAmount(currentSlot);
        }
        public InventoryManager InventoryManager => (InventoryManager)ManagerBase;

    }

}