using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _itemPanel;
    [SerializeField]
    private Image _itemImage;
    [SerializeField]
    private TMP_Text _itemAmount;

    public void UpdateSlot(ItemSO item)
    {
        _itemPanel.alpha = (item == null) ? 0 : 1;
        _itemPanel.blocksRaycasts = item == null;
        _itemPanel.interactable = item == null;

        if (item != null)
        {
            _itemImage.sprite = item.ItemSprite;
            _itemAmount.text = $"x{item.Amount}";
        }
    }
}
