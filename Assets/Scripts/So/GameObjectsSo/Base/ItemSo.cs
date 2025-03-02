using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace So.GameObjectsSo.Base
{
    [CreateAssetMenu(fileName = "ItemSo", menuName = "So/ItemSo", order = 0)]
    public class ItemSo : ScriptableObject
    {
        public float finishForProduction = 10f;

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