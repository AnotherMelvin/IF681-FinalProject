using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyDebug : MonoBehaviour
{
    [Header("Add Money")]
    [SerializeField]
    private int _addAmount;
    [SerializeField]
    private Button _addButton;

    [Header("Remove Money")]
    [SerializeField]
    private int _removeAmount;
    [SerializeField]
    private Button _removeButton;

    void Awake()
    {
        _addButton.onClick.AddListener(DebugAddMoney);
        _removeButton.onClick.AddListener(DebugRemoveMoney);
    }

    private void DebugAddMoney()
    {
        if (MoneyManager.Instance == null)
        {
            Debug.LogError("MoneyManager instance is not found in scene");
            return;
        }

        MoneyManager.Instance.AddAmount(_addAmount);
    }

    private void DebugRemoveMoney()
    {
        if (MoneyManager.Instance == null)
        {
            Debug.LogError("MoneyManager instance is not found in scene");
            return;
        }

        MoneyManager.Instance.RemoveAmount(_removeAmount);
    }
}
