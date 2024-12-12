using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScene : MonoBehaviour
{
    public string sceneName;

    public void ChangeScene()
    {
        SceneSwitcher.Instance.ChangeScene(sceneName, new Color32(0, 0, 0, 255), 1f);
    }
}
