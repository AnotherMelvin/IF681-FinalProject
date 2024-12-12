using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMaze : MonoBehaviour
{
    public float playerReach = 5f;
    private MazeSelect currentMaze;

    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        CheckInteraction();

        if (Input.GetMouseButtonDown(0) && currentMaze != null)
        {
            currentMaze.Interact();
        }
    }

    void CheckInteraction()
    {
        Ray ray = new(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, playerReach))
        {
            if (hit.collider.CompareTag("Maze"))
            {
                currentMaze = hit.collider.GetComponent<MazeSelect>();
                currentMaze.Select();
            }
        }
        else
        {
            if (currentMaze != null)
            {
                currentMaze.Deselect();
                currentMaze = null;
            }
        }
    }
}
