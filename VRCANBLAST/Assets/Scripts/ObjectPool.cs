using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // Crearemos una clase singleton para manejar el Pool de Objetos
    public static ObjectPool instance;
    // Generamos la lista de objetos
    private List<GameObject> pooledObjects = new List<GameObject>();
    [SerializeField] int amountToPool = 22;
    // Objeto que guardaremos en el pool
    [SerializeField] GameObject bulletPrefab;
    // Para convertirlo en Singleton
    void Awake()
    {
        // Garantizamos que solo exista una instancia del ObjectPool
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
    }
    // Método que se ejecuta antes del primer frame
    void Start()
    {
        // Generamos todos los objetos
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(bulletPrefab); // Instanciamos el objeto
            obj.SetActive(false); // Lo desactivamos
            pooledObjects.Add(obj); // Añadimos el objeto a la lista
        }
    }
    public GameObject GetPooledObject()
    {
        // Recorremos el pool
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            // Si el objeto no está activo en la jerarquía quiere decir
            // que está disponible para su uso
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i]; // Devolvemos el objeto disponible
            }
        }
        return null; // Si todos los objetos están ocupados devolvemos nulo
    }

}
