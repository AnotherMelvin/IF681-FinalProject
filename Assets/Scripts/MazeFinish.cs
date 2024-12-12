using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MazeFinish : MonoBehaviour, IPointerEnterHandler
{
    public MazePlayer player;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (player.isSelected)
        {
            player.isFinished = true;
        }
    }
}
