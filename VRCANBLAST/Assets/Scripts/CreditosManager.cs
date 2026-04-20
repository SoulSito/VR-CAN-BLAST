using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditosManager : MonoBehaviour
{
    public string escenaASubir = "Game";

    public void VolverAlMenu()
    {
        if (!string.IsNullOrEmpty(escenaASubir))
        {
            SceneManager.LoadScene(escenaASubir);
        }
        else
        {
            Debug.LogError("Error: No has puesto el nombre de la escena en el componente CreditosManager.");
        }
    }
}