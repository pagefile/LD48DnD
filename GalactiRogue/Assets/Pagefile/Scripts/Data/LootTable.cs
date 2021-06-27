using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pagefile.Collections;

// Copy/paste GameObject implementation os WeightedTable because Unity inspector
// cannot serialize generics.
namespace Pagefile.Data
{
    [CreateAssetMenu(fileName = "NewLootTable", menuName = "Tables/Loot Table")]
    public class LootTable : ScriptableObject
    {
        [SerializeField]
        private List<LootTableItemData> _lootList = default;
        [SerializeField]
        private List<GameObject> _guaranteedDrops = default;

        private float _weightTotal;
        private LootTableItemData _dummy;   // used in the binary search

        [System.Serializable]
        public class LootTableItemData
        {
            // Probably should make this an SO at some point
            public GameObject Item;
            public float Weight;

            // Used for searching the table
            private float _minRange;
            private float _maxRange;

            public float MinRange { get { return _minRange; } set { _minRange = value; } }
            public float MaxRange { get { return _maxRange; } set { _maxRange = value; } }

            public class SortComparer : IComparer<LootTableItemData>
            {
                public int Compare(LootTableItemData left, LootTableItemData right)
                {
                    if(left.Weight < right.Weight)
                    {
                        return -1;
                    }
                    if(left.Weight > right.Weight)
                    {
                        return 1;
                    }
                    return 0;
                }
            }

            public class SearchComparer : IComparer<LootTableItemData>
            {
                // Would have been more straight foward to implement my own search...
                public int Compare(LootTableItemData left, LootTableItemData right)
                {
                    if(left.MinRange < right.Weight && left.MaxRange < right.Weight)
                    {
                        return -1;
                    }
                    if(left.MaxRange > right.Weight && left.MinRange > right.Weight)
                    {
                        return 1;
                    }
                    return 0;
                }
            }
        }

        public void OnEnable()
        {
            if(_lootList == null)
            {
                return;
            }

            // Set up min and max weight values
            float lastValue = 0f;
            foreach(LootTableItemData item in _lootList)
            {
                item.MinRange = lastValue;
                item.MaxRange = lastValue + item.Weight;
                lastValue = item.MaxRange;
            }
            _weightTotal = lastValue;

            // Instantiate static dummy
            // It might be technically better to implement a custom binary search
            // but I don't think the gains are worth the time it would take.
            _dummy = new LootTableItemData();
        }

        // Returns an empty list if there are no drops
        public List<GameObject> RollOnTable(int rolls = 1)
        {
            List<GameObject> lootRolled = _guaranteedDrops != null ? new List<GameObject>(_guaranteedDrops) :  new List<GameObject>();
            for(int i = 0; i < rolls; i++)
            {
                _dummy.Weight = Random.Range(0, _weightTotal);
                int searchIndex = _lootList.BinarySearch(_dummy, new LootTableItemData.SearchComparer());
                GameObject lootDrop = _lootList[searchIndex].Item;
                if(lootDrop != null)
                {
                    lootRolled.Add(_lootList[searchIndex].Item);
                }
            }
            return lootRolled;
        }
    }

}
