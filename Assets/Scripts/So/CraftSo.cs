using System;
using System.Collections.Generic;
using So.GameObjectsSo.Base;
using UnityEngine;

namespace So
{
    [CreateAssetMenu(fileName = "CraftSo", menuName = "So/CraftSo", order = 0)]
    public class CraftSo : ScriptableObject
    {
        public List<Recipe> recipes = new List<Recipe>();
    }

    [Serializable]
    public class Recipe
    {
        public List<CraftItem> ingredient = new List<CraftItem>();
        public CraftItem result;
    }

    [Serializable]
    public class CraftItem
    {
        public ItemSo itemSo;
        public int amount;
    }
}