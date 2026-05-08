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

        canWidth = collider.radius * 2f * canPrefab.transform.localScale.x;
        canHeight = collider.height * canPrefab.transform.localScale.y;

        horizontalSpace = canWidth * .25f;
        verticalSpace = canHeight * .2f;
    }

    public void PlaceLevel(LevelData levelData)
    {
        levelStacksPositions.Clear();

        foreach (CanStackData data in levelData.stacks)
        {
            int numberOfCans = 1;
            for (int row = data.rows - 1; row >= 0; row--)
            {
                float evenRowOffset = (horizontalSpace * numberOfCans) + (row % 2 == 0 ? horizontalSpace : 0);
                for (int column = 0; column < numberOfCans; column++)
                {
                    levelStacksPositions.Add(new Vector3(
                            0,
                            row * canHeight,
                            column * (canWidth + horizontalSpace) + -evenRowOffset
                        ) + transform.position);
                }

                numberOfCans++;
            }
        }

        levelStacksPositions.Reverse();

        StartCoroutine("SpawnPile");
    }

    IEnumerator SpawnPile()
    {
        foreach(Vector3 position in levelStacksPositions)
        {
            GameObject can = Instantiate(canPrefab, position,Quaternion.identity);

            cansInPile.Add(can);

            yield return new WaitForSeconds(0.2f);
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

