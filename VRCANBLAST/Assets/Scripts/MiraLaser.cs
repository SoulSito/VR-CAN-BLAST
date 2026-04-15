using UnityEngine;

public class MiraLaser : MonoBehaviour
{
    public Transform origin;
    public GameObject laserDotPrefab;

    private GameObject laserDotInstance;

    public float maxDistance = 100f;

    void Start()
    {
        laserDotInstance = Instantiate(laserDotPrefab);
        laserDotInstance.SetActive(true);
    }
    void OnDisable()
    {
        if (laserDotInstance != null)
        {
            laserDotInstance.SetActive(false);
        }
    }
    void Update()
    {
        if (origin == null || laserDotInstance == null) return;

        RaycastHit hit;

        if (Physics.Raycast(origin.position, origin.forward, out hit, maxDistance))
        {
            laserDotInstance.SetActive(true);
            laserDotInstance.transform.position = hit.point + hit.normal * 0.001f;

            laserDotInstance.transform.LookAt(Camera.main.transform);
        }
        else
        {
            laserDotInstance.SetActive(false);
        }
    }
}