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
            hasLeftPileOfCan = true;

            GameMode.Instance.OnCanLeftTable(gameObject);
        }
    }
}
