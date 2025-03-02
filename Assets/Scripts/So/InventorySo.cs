using System;
using System.Collections.Generic;
using So.GameObjectsSo.Base;
using So.GameObjectsSo.Seed;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace So
{
    public enum InventoryCategories
    {
        Products,
        Buildings,
    }
    
    [CreateAssetMenu(fileName = "InventorySo", menuName = "So/InventorySo", order = 0)]
    public class InventorySo : ScriptableObject
    {
        public List<InventoryCategory> inventoryCategories;
        [FormerlySerializedAs("currencies")] 
        public List<Currency> inventoryCurrencies = new List<Currency>()
        {
            new Currency()
            {
                currencyEnum = CurrencyEnum.Gold,
                currencyVal = 0
            },
            new Currency()
            {
                currencyEnum = CurrencyEnum.Silver,
                currencyVal = 0
            }
        };
        public void RemoveInventoryItemByItemSo(ItemSo itemSo, int amount = 0)
        {
            var inventoryItem = GetInventoryItemByItemSo(itemSo);
            var currentCategory = GetInventoryCategory(itemSo);
            
            inventoryItem.count -= amount;
            if (inventoryItem.count <= 0)
            {
                inventoryItem.count = 50;
                // currentCategory.inventoryItems.Remove(inventoryItem);
            }
        }
        public InventoryItem GetInventoryItemByItemSo(ItemSo itemSo, int amount = 0)
        {
            var currentCategory = GetInventoryCategory(itemSo.inventoryCategory);
            
            foreach (var inventoryItem in currentCategory.inventoryItems)
            {
                if (inventoryItem.itemSo == itemSo)
                {
                    return inventoryItem;
                }
            }
            Debug.Log("No Found!");
            return null;
        }

        public InventoryCategory GetInventoryCategory(ItemSo inventoryCategory)
        {
            return GetInventoryCategory(inventoryCategory.inventoryCategory);
        }
        
        public InventoryCategory GetInventoryCategory(InventoryCategories inventoryCategory)
        {
            foreach (var category in inventoryCategories)
            {
                if (category.inventoryCategories == inventoryCategory)
                {
                    return category;
                }
            }
            Debug.Log("No Found!");
            return null;
        }

        public void AddCurrencies(List<Currency> currenciesVal)
        {
            if(currenciesVal.Count == 0) return;
            
            foreach (var currencyVal in currenciesVal)
            {
                foreach (var inventoryCurrency in inventoryCurrencies)
                {
                    if (inventoryCurrency.currencyEnum == currencyVal.currencyEnum)
                    {
                        inventoryCurrency.currencyVal += currencyVal.currencyVal;
                        break;
                    }
                }
            }
        }
        public Currency GetCurrencies(CurrencyEnum currencyType)
        {
            return inventoryCurrencies.Find(x => x.currencyEnum == currencyType);
        }

        public void AddInventoryItem(ItemSo itemSo, int amountOfProduct)
        {
            var inventoryItem = GetInventoryItemByItemSo(itemSo);

            if (inventoryItem == null)
            {
                var category = GetInventoryCategory(itemSo);
                category.inventoryItems.Add(
                    new InventoryItem()
                    {
                        itemSo = itemSo,
                        count = amountOfProduct
                    });
            }
            else
            {
                inventoryItem.count += amountOfProduct;
            }
            
        }
    }
    
    [Serializable]
    public class InventoryCategory
    {
        public InventoryCategories inventoryCategories;
        public List<InventoryItem> inventoryItems = new List<InventoryItem>();
    }

    [Serializable]
    public class InventoryItem
    {
        public ItemSo itemSo;
        public int count;
    }
    
    [Serializable]
    public class Currency
    {
        public CurrencyEnum currencyEnum;
        public int currencyVal;
    }

    public enum CurrencyEnum
    {
        Gold,
        Silver,
    }
}