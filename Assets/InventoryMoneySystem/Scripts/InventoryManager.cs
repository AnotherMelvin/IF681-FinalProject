using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [SerializeField]
    private int _inventorySize;
    public int InventorySize => _inventorySize;

    private List<ItemSO> inventoryItems = new();
    public Action<List<ItemSO>> OnInventoryItemsChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public ItemSO GetInventoryItem(string itemName)
    {
        return inventoryItems.Find(i => i.ItemName.Equals(itemName));
    }

    public List<ItemSO> GetAllInventoryItems()
    {
        return inventoryItems;
    }

    public bool AddItem(string itemName, int amount)
    {
        ItemSO item = inventoryItems.Find(i => i.ItemName.Equals(itemName));
        if (item == null)
        {
            item = ItemManager.Instance.GetItem(itemName);
            if (item == null)
            {
                Debug.LogError("Item not found");
                return false;
            }

            if (inventoryItems.Count >= _inventorySize)
            {
                Debug.LogError("Inventory is full");
                return false;
            }

            inventoryItems.Add(item);
        }

        if (!item.AddAmount(amount))
        {
            return false;
        }

        OnInventoryItemsChanged?.Invoke(inventoryItems);
        return true;
    }

    public bool RemoveItem(string itemName, int amount)
    {
        ItemSO item = inventoryItems.Find(i => i.ItemName.Equals(itemName));
        if (item == null)
        {
            Debug.LogError("Item not found");
            return false;
        }

        if (!item.RemoveAmount(amount))
        {
            return false;
        }

        if (item.Amount == 0)
        {
            inventoryItems.Remove(item);
        }

        OnInventoryItemsChanged?.Invoke(inventoryItems);
        return true;
    }
}
