using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _moneyAmount;

    void Start()
    {
        MoneyManager.Instance.OnAmountChanged += UpdateMoneyAmount;
    }

    void OnDisable()
    {
        MoneyManager.Instance.OnAmountChanged -= UpdateMoneyAmount;
    }

    private void UpdateMoneyAmount(int amount)
    {
        _moneyAmount.text = $"${amount}";
    }
}
