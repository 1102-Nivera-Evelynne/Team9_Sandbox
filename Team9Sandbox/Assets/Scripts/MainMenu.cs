using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menu, envMenu;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        SceneManager.UnloadSceneAsync(0);
    }

    public void ShowEnvMenu()
    {
        menu.SetActive(false);
        envMenu.SetActive(true);
    }
}
