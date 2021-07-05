using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/InventoryItemData", fileName = "NewInventoryItemData")]
public class InventoryItemData : ScriptableObject
{
    [SerializeField]
    private string _name = "NewItem";
    [SerializeField]
    private int _maxStackSize = 999;
    [SerializeField]
    private SGUID _itemID = default;

    public string Name => _name;
    public int MaxStackSize => _maxStackSize;

    public static bool IsSameItem(InventoryItemData first, InventoryItemData second)
    {
        return first._itemID == second._itemID;
    }

    // This is so stupid. There needs to be a callback for creation in editor...
    private void OnEnable()
    {
#if UNITY_EDITOR
        // This shouldn't happen in a release build of the game
        if(_itemID == default)
        {
            _itemID = Guid.NewGuid();
            Debug.Log($"InventoryItemData.OnEnable: Creating GUID for item {_name} GUID: {_itemID}");
        }
#endif
    }
}
