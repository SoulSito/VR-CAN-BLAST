using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGravityIfImpact : MonoBehaviour {

    // Si el proyectil impacta con algo le añadimos la gravedad
    private void OnCollisionEnter(Collision collision) {
        // Si existe colisión se añade gravedad
        if (collision != null) {
            GetComponent<Rigidbody>().useGravity = true;
        }
    }

}
