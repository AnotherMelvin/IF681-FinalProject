using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    private List<ItemSO> items = new();

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

        LoadItems();
    }

    public ItemSO GetItem(string itemName)
    {
        return items.Find(item => item.ItemName.Equals(itemName));
    }

    private void LoadItems()
    {
        ItemSO[] itemSOs = Resources.LoadAll<ItemSO>("Items");
        foreach (ItemSO itemSO in itemSOs)
        {
            itemSO.Initialize();
            items.Add(itemSO);
        }
    }
}
