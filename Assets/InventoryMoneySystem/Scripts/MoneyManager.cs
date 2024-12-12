using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    [SerializeField]
    private int _startingAmount;
    [SerializeField]
    private int _maxAmount;
    public int Amount { get; private set; }
    public Action<int> OnAmountChanged;

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

    void Start()
    {
        StartCoroutine(InitialAmount());
    }

    private IEnumerator InitialAmount()
    {
        yield return new WaitForEndOfFrame();
        Amount = _startingAmount;
        OnAmountChanged?.Invoke(Amount);
    }

    public bool AddAmount(int value)
    {
        if (Amount + value > _maxAmount)
        {
            Debug.LogError("Max amount reached");
            return false;
        }

        Amount += value;
        OnAmountChanged?.Invoke(Amount);
        return true;
    }

    public bool RemoveAmount(int value)
    {
        if (Amount - value < 0)
        {
            Debug.LogError("Not enough money");
            return false;
        }

        Amount -= value;
        OnAmountChanged?.Invoke(Amount);
        return true;
    }
}
