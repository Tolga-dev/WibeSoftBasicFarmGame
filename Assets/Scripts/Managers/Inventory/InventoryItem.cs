using So.GameObjectsSo.Base;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Managers.Inventory
{
    public class InventoryItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public InventoryManager inventoryManager;
        
        public Image itemIcon;
        public TextMeshProUGUI itemAmountText;

        public ItemSo currentItem;
        public int amount;

        public InventorySlot activeSlot;
        public InventorySlot oldActiveSlot;
        
        private InventoryItem _dividedInstance;
        public void SetItemData(ItemSo item, InventorySlot parent)
        {
            if (parent != null)
            {
                activeSlot = parent;
                oldActiveSlot = activeSlot;
                activeSlot.inventoryItem = this;
            }

            currentItem = item;
            SetUI();
        }

        private void SetUI()
        {
            itemIcon.sprite = currentItem.itemSprite;
        }

        #region Begin Drag

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (amount > 1)
            {
                var dividedAmount = 1;  
                _dividedInstance = inventoryManager.CreateNewInstance(activeSlot, currentItem, dividedAmount);
                amount -= dividedAmount;
                
                SetAmount();
            }
            else
            {
                activeSlot.inventoryItem = null;
            }
            
            StartDrag();
        }
 
        private void StartDrag()
        {
            SetIconTransparency(0.5f);
            itemIcon.raycastTarget = false;
            MoveToTopLayer();
        }

        private void MoveToTopLayer()
        {
            transform.SetParent(inventoryManager.draggableItemsParent);
            transform.SetAsLastSibling();
        }

        #endregion

        #region OnDrag

        public void OnDrag(PointerEventData eventData)
        {
            FollowPointer(eventData);
        }

        private void FollowPointer(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        #endregion

        #region EndDrag

        public void OnEndDrag(PointerEventData eventData)
        {
            if (activeSlot == null || activeSlot == oldActiveSlot)
            {
                Debug.Log("Dropped back to same slot");
                DestroyDividedInstance();
                EmptyNewPlaceDropped();
                activeSlot = oldActiveSlot;
            }
            else
            {
                Debug.Log("Dropped back to different slot");
                
                if (activeSlot.inventoryItem == null)
                {
                    EmptyNewPlaceDropped();
                }
                else if(activeSlot.inventoryItem.currentItem == currentItem)
                {
                    NewPlaceDroppedWithSameClass();
                }
                else
                {
                    NewPlaceDroppedDifferentClass();
                }
                oldActiveSlot = activeSlot;
            }
        }

        private void NewPlaceDroppedDifferentClass()
        {
            Debug.Log("NewPlaceDroppedDifferentClass");

            DestroyDividedInstance();
            activeSlot = oldActiveSlot;
            activeSlot.inventoryItem = this;
            SetDropped(oldActiveSlot);
        }
        private void NewPlaceDroppedWithSameClass()
        {
            Debug.Log("NewPlaceDroppedWithSameClass");
            activeSlot.inventoryItem.SetAmount(activeSlot.inventoryItem.amount + amount);
            Destroy(gameObject);
        }

        private void EmptyNewPlaceDropped()
        {
            Debug.Log("EmptyNewPlaceDropped");

            if (activeSlot.IsSlotNone())
            {
                activeSlot.inventoryItem = this;
                SetDropped(activeSlot);
            }
            else
            {
                NewPlaceDroppedDifferentClass();
            }
        }
        private void SetDropped(InventorySlot targetSlot)
        {
            transform.SetParent(targetSlot.transform);
            transform.localPosition = Vector3.zero;
            SetIconTransparency(1f);
            itemIcon.raycastTarget = true;
            _dividedInstance = null;
        } 
        private void SetIconTransparency(float alpha)
        {
            Color temp = itemIcon.color;
            temp.a = alpha;
            itemIcon.color = temp;
        }

        private void DestroyDividedInstance()
        {
            if (_dividedInstance != null)
            {
                Debug.Log("Remove Divided Instance");
                amount += _dividedInstance.amount;
                SetAmount();
                    
                Destroy(_dividedInstance.gameObject);
                _dividedInstance = null;
            }
        }
        #endregion

        public void SetAmount(int amountVal = 0)
        {
            if (amountVal != 0)
                amount = amountVal;

            itemAmountText.text = amount.ToString();
        }

    }
}
