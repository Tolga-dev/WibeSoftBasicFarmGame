using System.Collections.Generic;
using So.GameObjectsSo.Base;
using UnityEngine;

namespace So.GameObjectsSo.Seed
{
    public enum SeedType
    {
        Wheat,
        Corn,
        Potato
    }
    
    [CreateAssetMenu(fileName = "SeedSo", menuName = "So/SeedSo", order = 0)]
    public class SeedSo : ItemSo
    {
        public List<Sprite> seedGrowSprites = new List<Sprite>();
        
        public Sprite finalProductSprite;
        public SeedType seedType;
        public int amountOfProduct;
    }
    
    
}