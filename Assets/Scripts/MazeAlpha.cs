using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MazeAlpha : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MazePlayer player;
    public float alpha;
    private Image maze;

    void Awake()
    {
        maze = GetComponent<Image>();
        maze.alphaHitTestMinimumThreshold = alpha;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!player.isSelected) return;

        player.isSelected = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!player.isSelected) return;

        player.isSelected = false;
    }
}
