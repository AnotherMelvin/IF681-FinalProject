using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD_controller : MonoBehaviour
{
    public static HUD_controller instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] TMP_Text interactiontext;

    public void EnableInteractionText(string text)
    {
        interactiontext.text = text + "  (E)";
        interactiontext.gameObject.SetActive(true);
    }

    public void DisableInteractionText()
    {
        interactiontext.gameObject.SetActive(false);
    }
}
