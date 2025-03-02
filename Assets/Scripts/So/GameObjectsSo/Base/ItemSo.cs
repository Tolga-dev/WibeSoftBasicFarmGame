using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace So.GameObjectsSo.Base
{
    [CreateAssetMenu(fileName = "ItemSo", menuName = "So/ItemSo", order = 0)]
    public class ItemSo : ScriptableObject
    {
        public List<Currency> currencies = new List<Currency>();
        public string itemName;
        public Sprite itemSprite;
        public string itemDescription;

        public InventoryCategories inventoryCategory;

        public string GetFullDescription()
        {
            return itemDescription;
        }
    }

}