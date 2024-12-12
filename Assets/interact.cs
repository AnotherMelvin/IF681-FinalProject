using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interact : MonoBehaviour
{
    public float playerReach = 3f;
    Interactable currentInteractable;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInteraction();
        if(Input.GetKeyDown(KeyCode.E) && currentInteractable != null){
            currentInteractable.Interact();
        }
    }

    void CheckInteraction()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * playerReach, Color.red);

        if(Physics.Raycast( ray, out hit, playerReach))
        {
            if(hit.collider.tag == "Interactable")
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();

                if(currentInteractable && newInteractable != currentInteractable)
                {
                    currentInteractable.DisableOutLine();
                }

                if(newInteractable.enabled)
                {
                    SetNewCurrentInteractable(newInteractable);
                } else
                {
                    DisableCurrentInteractable();
                }
            }
        } else
        {
            DisableCurrentInteractable();
        }
    }

    void SetNewCurrentInteractable(Interactable newInteractable)
    {
        currentInteractable = newInteractable;
        currentInteractable.EnableOutLine();
        HUD_controller.instance.EnableInteractionText(currentInteractable.message);
    }

    void DisableCurrentInteractable()
    {
        HUD_controller.instance.DisableInteractionText();
        if(currentInteractable)
        {
            currentInteractable.DisableOutLine();
            currentInteractable = null;
        }
    }
}
