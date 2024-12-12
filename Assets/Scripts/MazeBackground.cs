using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MazeBackground : MonoBehaviour, IPointerExitHandler
{
    public MazePlayer player;

    public void OnPointerExit(PointerEventData eventData)
    {
        if (player.isSelected)
        {
            player.isSelected = false;
        }
    }
}
