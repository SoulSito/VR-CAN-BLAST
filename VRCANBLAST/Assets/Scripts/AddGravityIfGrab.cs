using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGravityIfGrab : MonoBehaviour {

    public void ActivateGravity() {
        Debug.Log("Se ha activado");
        GetComponent<Rigidbody>().useGravity = true;   
    }

}
