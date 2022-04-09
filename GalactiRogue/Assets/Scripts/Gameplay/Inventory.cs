using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private int _inventorySlots = 0;
    private List<InventoryItemData> _inventory;


    private void Start()
    {
        _inventory = new List<InventoryItemData>(_inventorySlots);
    }
}