using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pause : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _hud;
    public GameObject pausemenu;
    public GhostController ghost;
    bool isPaused;
    bool isMainMenu;
    bool isDelay;

    void Start()
    {
        pausemenu.SetActive(false);
        StartCoroutine(DelayPause());
    }

    void Update()
    {
        if (ghost.GameOver || isMainMenu || isDelay) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private IEnumerator DelayPause()
    {
        isDelay = true;
        yield return new WaitForSeconds(2f);
        isDelay = false;


    }

    public void Quit()
    {
        SceneSwitcher.Instance.ClosingScene();
    }

    public void Menu()
    {
        isMainMenu = true;
        SceneSwitcher.Instance.ChangeScene("menu", new Color32(0, 0, 0, 255), 2f);
    }

    public void Pause()
    {
        _hud.alpha = 0;
        pausemenu.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
        Debug.Log("time set to 0f");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        _hud.alpha = 1f;
        pausemenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
