using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    Outline outline;
    public string message;
    public UnityEvent onInteraction;
    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    public void Interact(){
        onInteraction.Invoke();
    }

    public void DisableOutLine(){
        outline.enabled = false;
    }

    public void EnableOutLine(){
        outline.enabled = true;
    }
}
