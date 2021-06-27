using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pagefile.Components
{
    public class LootDropper : MonoBehaviour
    {
        [SerializeField]
        private Data.LootTable _lootTable = default;

        // HACK: Later this should be an option to set as a circle
        // or a sphere
        [SerializeField]
        private float _radius = 3f;

        public void SpawnLoot(int rolls = 1)
        {
            List<GameObject> lootToDrop = _lootTable.RollOnTable(rolls);
            Vector3 offset = Random.insideUnitCircle * _radius;
            offset.z = offset.y;
            offset.y = 0;
            foreach(GameObject obj in lootToDrop)
            {
                Instantiate(obj, transform.position + offset, Quaternion.identity);
            }
        }
    }
}
