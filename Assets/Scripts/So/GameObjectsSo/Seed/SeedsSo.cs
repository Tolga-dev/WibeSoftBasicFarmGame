using System.Collections.Generic;
using So.GameObjectsSo.Seed;
using UnityEngine;

namespace So.Seed
{
    [CreateAssetMenu(fileName = "SeedsSo", menuName = "So/SeedsSo", order = 0)]
    public class SeedsSo : ScriptableObject
    {
        public List<SeedSo> seedSo = new List<SeedSo>();

        public SeedSo GetSeedById(SeedType seedType)
        {
            foreach (var seedSoIndex in seedSo)
            {
                if (seedSoIndex.seedType == seedType)
                    return seedSoIndex;
            }

            Debug.Log("No Product Found!");
            return null;
        }
    }
}