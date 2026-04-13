using UnityEngine;
using System.Collections;

public class GunAnimations : MonoBehaviour
{
    public Transform objeto;
    public float distancia = 0.22f;
    public float velocidad = 0.33f;

    private Vector3 posicionInicialLocal;
    private bool enMovimiento = false;

    void Start()
    {
        if (objeto != null)
        {
            posicionInicialLocal = objeto.localPosition;
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