using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    [SerializeField]
    private string itemName;
    public string ItemName => itemName;

    [SerializeField]
    private Sprite itemSprite;
    public Sprite ItemSprite => itemSprite;

    [SerializeField]
    private int maxAmount;
    public int MaxAmount => maxAmount;

    public int Amount { get; private set; }

    public void Initialize()
    {
        Amount = 0;
    }

    public bool AddAmount(int amount)
    {
        if (Amount + amount > maxAmount)
        {
            Debug.LogError("Max amount reached");
            return false;
        }

        Amount += amount;
        return true;
    }

    public bool RemoveAmount(int amount)
    {
        if (Amount - amount < 0)
        {
            Debug.LogError("Not enough items");
            return false;
        }

        Amount -= amount;
        return true;
    }
}
