using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CanData
{
    public GameObject can;
    public Transform originalTransform;

    public CanData(GameObject can, Transform originalTransform)
    {
        this.can = can;
        this.originalTransform = originalTransform;
    }
}

public class PileOfCans : MonoBehaviour
{
    List<CanData> cans = new();

    public void Setup()
    {
        // Find all GameObjects with Tag 'Can' inside PileOfCans
        // Save their transforms
    }

    public void Reset()
    {
        // Loop through the list and reset their Transforms.
    }


}
