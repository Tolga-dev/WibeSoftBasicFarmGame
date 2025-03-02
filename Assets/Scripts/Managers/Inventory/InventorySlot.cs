using UnityEngine;
using UnityEngine.EventSystems;

namespace Managers.Inventory
{
    public enum SlotTag { None}

    public class InventorySlot : MonoBehaviour, IDropHandler,IPointerClickHandler
    {
        public InventoryManager inventoryManager;
        public InventoryItem inventoryItem;
        public SlotTag myTag;
        
        public void OnDrop(PointerEventData eventData)
        {
            var currentItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            currentItem.activeSlot = this;
        }
        public bool IsSlotNone()
        {
            return myTag is SlotTag.None;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var descriptionPanelText = $"{inventoryItem.currentItem.name}: {inventoryItem.currentItem.GetFullDescription()}";
            inventoryManager.SetDescriptionPanel(this,descriptionPanelText);
        }
        
        public void HandleAmount()
        {
            inventoryItem.amount--;
            inventoryItem.SetAmount();

            if (inventoryItem.amount <= 0)
            {
                Destroy(inventoryItem.gameObject);
                inventoryItem = null;
            }
        }

    }
}