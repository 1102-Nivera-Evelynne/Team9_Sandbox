using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menu;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        SceneManager.UnloadSceneAsync(0);
    }
}
