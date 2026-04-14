using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ShootController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference triggerAction;

    [Header("Animación")]
    public Transform objeto;
    public float distancia = 0.22f;
    public float velocidad = 0.33f;

    private Vector3 posicionInicialLocal;
    private bool enMovimiento = false;
    private bool hasFired = false;

    private void Start()
    {
        if (objeto != null)
        {
            posicionInicialLocal = objeto.localPosition;
        }
    }

    private void OnEnable()
    {
        triggerAction.action.Enable();
    }

    private void OnDisable()
    {
        triggerAction.action.Disable();
    }

    private void Update()
    {
        float triggerValue = triggerAction.action.ReadValue<float>();
        Debug.Log(triggerValue);

        if (triggerValue > 0.8f && !hasFired)
        {
            Mover();
            hasFired = true;
        }

        if (triggerValue < 0.2f)
        {
            hasFired = false;
        }
    }

    public void Mover()
    {
        if (!enMovimiento && objeto != null)
        {
            StartCoroutine(MoverRutina());
        }
    }

    private IEnumerator MoverRutina()
    {
        enMovimiento = true;

        Vector3 destino = posicionInicialLocal + Vector3.right * distancia;

        while (Vector3.Distance(objeto.localPosition, destino) > 0.01f)
        {
            objeto.localPosition = Vector3.MoveTowards(objeto.localPosition, destino, velocidad * Time.deltaTime);
            yield return null;
        }

        while (Vector3.Distance(objeto.localPosition, posicionInicialLocal) > 0.01f)
        {
            objeto.localPosition = Vector3.MoveTowards(objeto.localPosition, posicionInicialLocal, velocidad * Time.deltaTime);
            yield return null;
        }

        enMovimiento = false;
    }
}