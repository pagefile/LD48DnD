using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public int Amount = 0;
    public InventoryItemData Item = default;

    public InventorySlot(InventoryItemData itemData, int amount)
    {
        Item = itemData;
        Amount = amount;
    }

    // Adds an amount to the item in the slot. If it goes over the stack limit,
    // the slot is set to the stack limit and the leftover amount is returned
    public int AddItem(int amount)
    {
        int leftOver = 0;
        Amount += amount;
        if(Amount > Item.MaxStackSize)
        {
            leftOver = Amount - Item.MaxStackSize;
            Amount = Item.MaxStackSize;
        }
        return leftOver;
    }

    public int SetItem(InventoryItemData item, int amount)
    {
        int leftOver = 0;
        Item = item;
        Amount = amount;
        if(Amount > item.MaxStackSize)
        {
            leftOver = Amount - Item.MaxStackSize;
            Amount = item.MaxStackSize;
        }
        return leftOver;
    }

    public static void TransferTo(InventorySlot from, InventorySlot to, int amount)
    {
        if(!InventoryItemData.IsSameItem(from.Item, to.Item))
        {
            return;
        }
        amount = Mathf.Max(from.Amount, from.Item.MaxStackSize);
        int leftover = to.AddItem(amount);
        from.Amount = leftover;
    }
}