using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher Instance { get; private set; }

    [SerializeField]
    private Image _panel;
    [SerializeField]
    private CanvasGroup _panelView;

    private bool firstBoot = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _panelView.alpha = 0;
        _panelView.blocksRaycasts = false;
    }

    public void ChangeScene(string name, Color32 panelColor, float duration)
    {
        _panel.color = panelColor;
        StartCoroutine(Transition(name, duration));
    }

    public void OpeningScene()
    {
        if (firstBoot)
        {
            StartCoroutine(Opening());
            firstBoot = false;
        }
    }

    public void ClosingScene()
    {
        StartCoroutine(Closing());
    }

    private IEnumerator Opening()
    {
        _panelView.alpha = 1;
        _panelView.blocksRaycasts = true;

        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            _panelView.alpha = Mathf.Lerp(1f, -0.1f, elapsedTime / 2f);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        _panelView.blocksRaycasts = false;
    }

    private IEnumerator Closing()
    {
        _panelView.alpha = 0;
        _panelView.blocksRaycasts = true;

        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            _panelView.alpha = Mathf.Lerp(0, 1.1f, elapsedTime / 2f);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        Application.Quit();
    }

    private IEnumerator Transition(string name, float duration)
    {
        _panelView.blocksRaycasts = true;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            _panelView.alpha = Mathf.Lerp(0, 1.1f, elapsedTime / duration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);

        while (!async.isDone)
        {
            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;

        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            _panelView.alpha = Mathf.Lerp(1f, -0.1f, elapsedTime / duration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        _panelView.blocksRaycasts = false;
    }
}
