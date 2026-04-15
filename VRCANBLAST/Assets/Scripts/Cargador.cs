using UnityEngine;

public class Cargador : MonoBehaviour
{
    public ShootController controller;
    public GameObject Padre;

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