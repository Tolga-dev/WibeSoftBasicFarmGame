using System;
using System.Collections.Generic;
using Controller.Base;
using Managers.Base;
using Managers.Inventory;

namespace Controller.InventoryControllers
{
    [Serializable]
    public class InventorySlotsController : ControllerBase
    {
        public override void Initialization(ManagerBase managerBaseVal)
        {
            base.Initialization(managerBaseVal);
            foreach (var categorySlots in InventoryManager.categorySlotsList)
            {
                categorySlots.categoryPanelOpenButton.onClick.AddListener(() =>
                {
                    foreach (var slots in InventoryManager.categorySlotsList)
                    {
                        slots.categoryPanel.gameObject.SetActive(false);
                    }
                    categorySlots.categoryPanel.gameObject.SetActive(true);
                });
            }
        }
        public InventoryManager InventoryManager => (InventoryManager)ManagerBase;

    }
}