
using UnityEngine;
using UnityEngine.SceneManagement;

public class tomain : MonoBehaviour
{
    private void Start()
    {
        SceneSwitcher.Instance.OpeningScene();
    }

    public void gotomain()
    {
        SceneSwitcher.Instance.ChangeScene("GameScene", new Color32(0, 0, 0, 255), 2f);
    }

    public void quit()
    {
        SceneSwitcher.Instance.ClosingScene();
    }

    public void credits()
    {
        SceneSwitcher.Instance.ChangeScene("credits", new Color32(0, 0, 0, 255), 1f);
    }

    public void menu()
    {
        SceneSwitcher.Instance.ChangeScene("menu", new Color32(0, 0, 0, 255), 1f);
    }
}
