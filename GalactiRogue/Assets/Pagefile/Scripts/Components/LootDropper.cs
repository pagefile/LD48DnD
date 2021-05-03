using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagefile.Components
{
    public class LootDropper : MonoBehaviour
    {
        [SerializeField]
        private Data.LootTable _lootTable = default;

        public void SpawnLoot(int rolls = 1)
        {
            List<GameObject> lootToDrop = _lootTable.RollOnTable(rolls);
            foreach(GameObject obj in lootToDrop)
            {
                // TODO: Randomize with serielized inspector variables
                Instantiate(obj, transform.position, Quaternion.identity);
            }
        }
    }
}
