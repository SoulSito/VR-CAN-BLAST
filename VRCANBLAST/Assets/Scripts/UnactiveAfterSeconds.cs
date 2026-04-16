using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnactiveAfterSeconds : MonoBehaviour
{
    [SerializeField] int timeToDestroy = 7;


    private void OnEnable()
    {
        GetComponent<Rigidbody>().useGravity = false;
        StartCoroutine(DisableAfterNSeconds(timeToDestroy));
    }
    IEnumerator DisableAfterNSeconds(int secondsToDisable)
    {
        // Esperamos los segundos especificados
        yield return new WaitForSeconds(secondsToDisable);
        // Dormimos el objeto
        this.gameObject.SetActive(false);
    }
}
