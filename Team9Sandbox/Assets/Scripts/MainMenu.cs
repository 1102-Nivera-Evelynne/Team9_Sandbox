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

    public void showMainMenu()
    {
        menu.SetActive(true);
        envMenu.SetActive(false);
    }

    public void StartGameDark()
    {
        //Uncomment these when scene is added
       // SceneManager.LoadScene(2);
       // SceneManager.UnloadSceneAsync(0);
    }

    public void startTutorial()
    {
        //Uncomment these when scene is added
        // SceneManager.LoadScene(3);
        // SceneManager.UnloadSceneAsync(0);
    }
}
