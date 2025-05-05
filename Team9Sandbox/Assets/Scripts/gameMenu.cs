using UnityEngine;
using UnityEngine.SceneManagement;

public class gameMenu : MonoBehaviour
{
    public GameObject menu;
    public void ReturnToMain()
    {
        SceneManager.LoadScene(0);
        SceneManager.UnloadSceneAsync(1);
    }
}
