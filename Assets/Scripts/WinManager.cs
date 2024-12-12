using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _endMonologue;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(EndSequence());
    }

    private IEnumerator EndSequence()
    {
        _endMonologue.text = "You have found the way out...";
        yield return new WaitForSeconds(6f);
        _endMonologue.text = "Thanks for playing!";
        yield return new WaitForSeconds(5f);
        SceneSwitcher.Instance.ChangeScene("menu", new Color32(0, 0, 0, 255), 2f);
    }
}
