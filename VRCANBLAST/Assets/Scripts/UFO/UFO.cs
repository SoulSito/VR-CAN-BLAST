using System;
using UnityEngine;

public class UFO : MonoBehaviour
{
    internal void Activate()
    {
        // TODO
    }

    internal void AddCanToPickUpList(GameObject can)
    {
        // TODO
    }

    private void PickUpCan()
    {
        // TODO

        GameObject can = null;
        GameMode.Instance.OnCanWasPickedUp(can);
    }
}
