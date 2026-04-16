using UnityEngine;

public class Can : MonoBehaviour
{
    bool hasTouchedFloor = false;

    internal void Disable()
    {
        gameObject.SetActive(false);
    }

    internal void Enable()
    {
        gameObject.SetActive(true);
        hasTouchedFloor = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            hasTouchedFloor = true;

            GameMode.Instance.OnCanTouchedFloor(gameObject);
        }
    }
}
