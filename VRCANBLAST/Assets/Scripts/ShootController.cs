using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ShootController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference triggerAction;
    [SerializeField] private InputActionReference gripAction;

    [Header("Animación Disparar")]
    public Transform tambor;
    public float distancia = 0.22f;
    public float velocidad = 0.33f;

    [Header("Recargar")]
    public Transform boton;
    public Transform puntoRecarga;
    public GameObject cargadorReal;
    public GameObject cargadorSimulado;
    [SerializeField] private int bullets = 3;
    private int maxBullets = 3;

    public float recorridoMaxY = 0.03f;
    public float suavizado = 15f;

    private Vector3 posicionInicialLocal;
    private Vector3 botonPosInicial;

    private bool enMovimiento = false;
    private bool hasFired = false;
    public bool haRecargado = true;

    private void Start()
    {
        if (tambor != null)
        {
            posicionInicialLocal = tambor.localPosition;
        }

        if (boton != null)
        {
            botonPosInicial = boton.localPosition;
        }
        Setup(new GunSettings());
    }
    public void Setup(GunSettings gunSettings)
    {

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

        if (triggerValue < 0.2f)
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
        if (bullets > 0)
        {
            bullets--;
            Debug.Log("Has disparado");
        }
        else
        {
            Debug.Log("No tienes balas");
        }
        if (!enMovimiento && tambor != null)
        {
            StartCoroutine(MoverTambor());
        }
    }

    private IEnumerator MoverTambor()
    {
        enMovimiento = true;

        Vector3 destino = posicionInicialLocal + Vector3.right * distancia;

        while (Vector3.Distance(tambor.localPosition, destino) > 0.01f)
        {
            tambor.localPosition = Vector3.MoveTowards(
                tambor.localPosition,
                destino,
                velocidad * Time.deltaTime
            );
            yield return null;
        }

        while (Vector3.Distance(tambor.localPosition, posicionInicialLocal) > 0.01f)
        {
            tambor.localPosition = Vector3.MoveTowards(
                tambor.localPosition,
                posicionInicialLocal,
                velocidad * Time.deltaTime
            );
            yield return null;
        }

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
    }

}