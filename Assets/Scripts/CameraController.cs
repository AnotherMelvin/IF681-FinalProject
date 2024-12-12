using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _orientation;
    [SerializeField]
    private Transform _headCamera;

    public bool GameOver;
    public bool OnMaze;

    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (OnMaze)
        {
            return;
        }

        if (GameOver)
        {
            transform.SetPositionAndRotation(_headCamera.position, _headCamera.rotation);
            transform.SetParent(_headCamera);
            return;
        }

        float mouseX = Input.GetAxisRaw("Mouse X") * 200f * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * 200f * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -60f, 60f);

        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        _orientation.localRotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}
