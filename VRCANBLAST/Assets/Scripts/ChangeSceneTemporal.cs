using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneTemporal : MonoBehaviour
{
    public void StartGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
