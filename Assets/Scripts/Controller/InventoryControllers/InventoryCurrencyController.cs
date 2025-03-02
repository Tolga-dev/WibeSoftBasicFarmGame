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
        public TextMeshProUGUI goldText;
        public TextMeshProUGUI silverText;
        public void UpdateCurrencyUI()
        {
            var inventorySo = ManagerBase.gameManager.saveManager.gameDataSo.inventorySo;
            silverText.text = "Silver:" + inventorySo.GetCurrencies(CurrencyEnum.Silver).currencyVal;
            goldText.text = "Gold:" + inventorySo.GetCurrencies(CurrencyEnum.Gold).currencyVal;
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