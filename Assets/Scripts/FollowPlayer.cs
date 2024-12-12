using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform _cameraPosition;
    public bool GameOver;
    public bool OnMaze;

    private void Update()
    {
        if (!GameOver || !OnMaze)
        {
            transform.position = _cameraPosition.position;
        }
    }
}
