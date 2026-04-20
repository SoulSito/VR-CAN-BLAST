using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;


public class PileOfCans : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] GameObject canPrefab;
    [SerializeField] int maxInactiveCans = 20;
    [SerializeField] float spaceBetweenStacks = 1f;

    List<GameObject> inactiveCans = new();
    List<GameObject> cansInPile = new();

    List<Vector3> levelStacksPositions = new();
    float canWidth = 1f;
    float canHeight = 1f;

    float horizontalSpace = 0.5f;
    float verticalSpace = 0.2f;

    private void Start()
    {
        CapsuleCollider collider = canPrefab.GetComponent<CapsuleCollider>();

        canWidth = collider.radius * 2f;
        canHeight = collider.height;

        horizontalSpace = canWidth * .5f;
        verticalSpace = canHeight * .2f;
    }

    public void PlaceLevel(LevelData levelData)
    {
        foreach (CanStackData data in levelData.stacks)
        {
            int numberOfCans = 1;
            for (int row = data.rows - 1; row >= 0; row--)
            {
                for (int column = 0; column < numberOfCans; column++)
                {
                    GameObject can = Instantiate(canPrefab,
                        new Vector3(
                            0,
                            row * canHeight,
                            column * canWidth + horizontalSpace
                        ) + transform.position,
                        Quaternion.identity
                    );

                    cansInPile.Add(can);
                }

                numberOfCans++;
            }
        }
    }

    internal EndLevelResult EndLevel()
    {
        GameObject[] cans = cansInPile.ToArray();
        foreach (GameObject can in cans) Destroy(can);

        return new EndLevelResult();
    }

    internal void AddCanToInactiveList(GameObject can)
    {
        //todo
    }
}

