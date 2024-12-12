using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [SerializeField]
    private Transform _inventorySlotContainer;
    [SerializeField]
    private InventorySlot _inventorySlotPrefab;

    private List<InventorySlot> inventorySlots = new();

    void Start()
    {
        InventoryManager.Instance.OnInventoryItemsChanged += UpdateInventorySlots;
        LoadInventorySlots();
    }

    void OnDisable()
    {
        InventoryManager.Instance.OnInventoryItemsChanged -= UpdateInventorySlots;
    }

    private void LoadInventorySlots()
    {
        for (int i = 0; i < InventoryManager.Instance.InventorySize; i++)
        {
            InventorySlot slot = Instantiate(_inventorySlotPrefab, _inventorySlotContainer);
            slot.UpdateSlot(null);
            inventorySlots.Add(slot);
        }
    }

    private void UpdateInventorySlots(List<ItemSO> inventoryItems)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            ItemSO item = i < inventoryItems.Count ? inventoryItems[i] : null;
            inventorySlots[i].UpdateSlot(item);
        }
    }
}
