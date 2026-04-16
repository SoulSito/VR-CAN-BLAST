using UnityEngine;

public class Cargador : MonoBehaviour
{
    private ShootController controller;
    public GameObject Padre;
    private void Start()
    {
        controller = GameObject.FindWithTag("GunController").GetComponent<ShootController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Recarga"))
        {
            if(controller.haRecargado == false)
            {
                controller.Recargar();
                Destroy(Padre);
            }
        }
    }
}