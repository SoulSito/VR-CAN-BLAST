using UnityEngine;

public class Can : MonoBehaviour
{
    bool hasLeftPileOfCan = false;

    internal void Disable()
    {
        gameObject.SetActive(false);
    }

    internal void Enable()
    {
        gameObject.SetActive(true);
        hasLeftPileOfCan = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PileOfCans"))
        {
            if (!hasLeftPileOfCan)
            {
                hasLeftPileOfCan = true;

                GameMode.Instance.OnCanLeftTable(gameObject);
            }
        }
    }

    internal bool IsKnockedOver()
    {
        return Vector3.Angle(transform.up, Vector3.up) > 45f;
    }
}
