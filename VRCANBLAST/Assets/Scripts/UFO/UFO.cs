using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UFO : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    List<GameObject> cans = new List<GameObject>();

    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    internal void Activate()
    {
        StartCoroutine("LookForCans");
    }

    IEnumerator LookForCans()
    {
        GameObject can = null;

        while (true)
        {
            yield return new WaitForSeconds(1f);

            Debug.Log(can);

            if(can == null && cans.Count > 0)
            {
                can = cans[0];
            }

            if(can != null)
            {
                yield return MoveTo(can);

                yield return new WaitForSeconds(0.5f);

                if(can != null)
                {
                    PickUpCan(can);
                    can = null;
                }
            }
        }
    }

    IEnumerator MoveTo(GameObject can)
    {
        if (can == null) yield break;

        agent.SetDestination(can.transform.position);

        while (can != null &&
               agent.pathPending ||
               agent.remainingDistance > agent.stoppingDistance)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    internal void AddCanToPickUpList(GameObject can)
    {
        if(cans.Contains(can)) return;

        cans.Add(can);
    }

    private void PickUpCan(GameObject can)
    {
        GameMode.Instance.OnCanWasPickedUp(can);
        cans.Remove(can);
    }
}
