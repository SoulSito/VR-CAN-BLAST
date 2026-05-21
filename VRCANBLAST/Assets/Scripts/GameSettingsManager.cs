using UnityEngine;


public class GameSettingsManager : MonoBehaviour
{

    public enum CantidadDeLatas
    {
        Baja,
        Media,
        Alta,
        Aleatorio
    }

    public static GameSettingsManager Instance;

    public GunSettings currentGunSettings = new GunSettings();
    public CantidadDeLatas cantidadDeLatas = CantidadDeLatas.Aleatorio;

    private void Awake()
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
    }
}