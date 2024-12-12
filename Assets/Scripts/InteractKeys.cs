using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractKeys : MonoBehaviour
{
    public float playerReach = 5f;
    private KeyScript currentKey;

    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        CheckInteraction();

        if (Input.GetMouseButtonDown(0) && currentKey != null)
        {
            currentKey.Interact();
        }
    }

    void CheckInteraction()
    {
        Ray ray = new(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, playerReach))
        {
            if (hit.collider.CompareTag("Key"))
            {
                currentKey = hit.collider.GetComponent<KeyScript>();
                currentKey.Select();
            }
        }
        else
        {
            if (currentKey != null)
            {
                currentKey.Deselect();
                currentKey = null;
            }
        }
    }
}
