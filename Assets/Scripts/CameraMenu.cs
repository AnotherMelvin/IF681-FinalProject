using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenu : MonoBehaviour
{
    void Update()
    {
        Vector3 angle = transform.eulerAngles;
        angle.y += 1f * Time.deltaTime;
        transform.eulerAngles = angle;
    }
}
