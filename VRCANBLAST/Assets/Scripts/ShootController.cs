using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ShootController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference triggerAction;
    [SerializeField] private InputActionReference gripAction;

    [Header("Disparar")]
    public Transform tambor;
    public float distancia = 0.22f;
    public float velocidad = 0.33f;
    [SerializeField] private GameObject firePoint;
    private Transform transformFirePoint;
    private AudioSource sfxSource;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private float bulletSpeed = 20f;

    [Header("Recargar")]
    public Transform boton;
    public Transform puntoRecarga;
    public GameObject cargadorReal;
    public GameObject cargadorSimulado;
    [SerializeField] BeltManager beltManager;

    [Header("Configuracion")]
    [SerializeField] private GameObject miraLaser;
    [SerializeField] private int bullets;
    [SerializeField] private int maxBullets;

    public float recorridoMaxY = 0.03f;
    public float suavizado = 15f;

    private Vector3 posicionInicialLocal;
    private Vector3 botonPosInicial;

    private bool enMovimiento = false;
    private bool hasFired = false;
    public bool haRecargado = true;
    bool sinBalas = false;
    bool recarga = false;

    private void Start()
    {
        if (firePoint != null) 
        {
            transformFirePoint = firePoint.GetComponent<Transform>();
            sfxSource = firePoint.GetComponent<AudioSource>();
        }
        if (tambor != null)
        {
            posicionInicialLocal = tambor.localPosition;
        }

        if (boton != null)
        {
            botonPosInicial = boton.localPosition;
        }
        Setup(new GunSettings());
        bullets = maxBullets;
    }
    public void Setup(GunSettings gunSettings)
    {
        maxBullets = gunSettings.maxBullets;

        if (gunSettings.hasLaser)
        {
            miraLaser.SetActive(true);
        }
        else
        {
            miraLaser.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (triggerAction != null) triggerAction.action.Enable();
        if (gripAction != null) gripAction.action.Enable();
    }

    private void OnDisable()
    {
        if (triggerAction != null) triggerAction.action.Disable();
        if (gripAction != null) gripAction.action.Disable();
    }

    private void Update()
    {
        if (triggerAction == null || gripAction == null) return;

        float triggerValue = triggerAction.action.ReadValue<float>();
        float gripValue = gripAction.action.ReadValue<float>();


        if (triggerValue > 0.8f && !hasFired)
        {
            Disparar();
            hasFired = true;
        }

        if (triggerValue < 0.1f)
        {
            hasFired = false;
        }

        if (gripValue >= 0.9f && haRecargado)
        {
            Descargar();
            haRecargado = false;
        }

        if (boton != null)
        {
            float desplazamiento = gripValue * recorridoMaxY;

            Vector3 posicionObjetivo = botonPosInicial + Vector3.up * desplazamiento;

            boton.localPosition = Vector3.Lerp(
                boton.localPosition,
                posicionObjetivo,
                Time.deltaTime * suavizado
            );
        }
    }

    public void Disparar()
    {
        recarga = false;
        if (bullets > 0)
        {
            sfxSource.PlayOneShot(shootSound);
            bullets--;
            Debug.Log("Has disparado");
            sinBalas = false;
            GameObject bullet = ObjectPool.instance.GetPooledObject();

            if (bullet != null)
            {
                bullet.transform.position = transformFirePoint.position;
                bullet.transform.rotation = transformFirePoint.rotation * Quaternion.Euler(90f, 0f, 0f);
                bullet.SetActive(true);

                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.linearVelocity = transformFirePoint.forward * bulletSpeed;
                }
            }
        }
        else
        {
            Debug.Log("No tienes balas");
            sinBalas = true;
        }
        if (!enMovimiento && tambor != null)
        {
            StartCoroutine(MoverTambor(sinBalas, recarga));
        }
    }

    private IEnumerator MoverTambor(bool sinBalas, bool recarga)
    {
        enMovimiento = true;

        Vector3 destino = posicionInicialLocal + Vector3.right * distancia;
        if (!recarga)
        {
            while (Vector3.Distance(tambor.localPosition, destino) > 0.01f)
            {
                tambor.localPosition = Vector3.MoveTowards(
                    tambor.localPosition,
                    destino,
                    velocidad * Time.deltaTime
                );
                yield return null;
            }
        }
        
        if (!sinBalas || recarga)
        {
            while (Vector3.Distance(tambor.localPosition, posicionInicialLocal) > 0.01f)
            {
                tambor.localPosition = Vector3.MoveTowards(
                    tambor.localPosition,
                    posicionInicialLocal,
                    velocidad * Time.deltaTime
                );
            }
        }
        yield return null;

        enMovimiento = false;
    }

    void Descargar()
    {
        if (puntoRecarga == null || cargadorSimulado == null) return;

        GameObject nuevoCargador = Instantiate(
            cargadorSimulado,
            puntoRecarga.position,
            puntoRecarga.rotation
        );

        nuevoCargador.transform.SetParent(null);
        bullets = 0;

        if (cargadorReal != null)
        {
            cargadorReal.SetActive(false);
        }

        Rigidbody rb = nuevoCargador.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 direccion = (puntoRecarga.up + (-puntoRecarga.right * 0.3f)).normalized;

            float fuerza = 1f;

            rb.AddForce(direccion * fuerza, ForceMode.Impulse);
        }
    }
    public void Recargar()
    {
        if (puntoRecarga == null || cargadorReal == null || haRecargado) return;

        cargadorReal.SetActive(true);
        haRecargado = true;
        bullets = maxBullets;
        if (sinBalas)
        {
            recarga = true;
            sinBalas = false;
            StartCoroutine(MoverTambor(sinBalas, recarga));
        }
        GameMode.Instance.PlaceNextPile();
        beltManager.InstanciarCargador();
    }
    public void RecargarSinCargador()
    {
        bullets = maxBullets;
        StartCoroutine(MoverTambor(sinBalas, recarga));
    }

}