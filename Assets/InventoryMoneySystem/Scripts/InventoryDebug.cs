using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDebug : MonoBehaviour
{
    [Header("Add Item")]
    [SerializeField]
    private string _addItemName;
    [SerializeField]
    private int _addItemAmount;
    [SerializeField]
    private Button _addButton;

    [Header("Remove Item")]
    [SerializeField]
    private string _removeItemName;
    [SerializeField]
    private int _removeItemAmount;
    [SerializeField]
    private Button _removeButton;

    void Awake()
    {
        _addButton.onClick.AddListener(DebugAddItem);
        _removeButton.onClick.AddListener(DebugRemoveItem);
    }

    private void DebugAddItem()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager instance is not found in scene");
            return;
        }

        InventoryManager.Instance.AddItem(_addItemName, _addItemAmount);
    }

    private void DebugRemoveItem()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager instance is not found in scene");
            return;
        }

        InventoryManager.Instance.RemoveItem(_removeItemName, _removeItemAmount);
    }
}
