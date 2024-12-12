using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseManager : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Continue()
    {
        SceneSwitcher.Instance.ChangeScene("menu", new Color32(0, 0, 0, 255), 2f);
    }
}
