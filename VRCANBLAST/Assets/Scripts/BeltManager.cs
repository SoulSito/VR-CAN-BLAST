using UnityEngine;

public class BeltManager : MonoBehaviour
{
    [SerializeField] private GameObject Cargador;
    private GameObject CargadorActual;
    private void Start()
    {
        InstanciarCargador();
    }
    public void InstanciarCargador()
    {
        CargadorActual = Instantiate(Cargador, transform.position, Quaternion.identity, transform);
        CargadorActual.GetComponent<Rigidbody>().useGravity = false;
    }
    
}
