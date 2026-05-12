using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEditor.Rendering.Universal.ShaderGUI;
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

    private void Start()
    {
        CapsuleCollider collider = canPrefab.GetComponent<CapsuleCollider>();

        canWidth = collider.radius * 2f * canPrefab.transform.localScale.x;
        canHeight = collider.height * canPrefab.transform.localScale.y;

        horizontalSpace = canWidth * .25f;
    }

    public void PlaceLevel(LevelData levelData)
    {
        levelStacksPositions.Clear();


        
        float firstRowOffset = -GetFirstRowOffset(levelData.stacks);
        foreach (CanStackData data in levelData.stacks)
        {
            int numberOfCans = 1;
            for (int row = data.rows - 1; row >= 0; row--)
            {
                for (int column = 0; column < numberOfCans; column++)
                {
                    levelStacksPositions.Add(new Vector3(
                            0,
                            row * canHeight,
                            column * (canWidth + horizontalSpace) + -(((canWidth + horizontalSpace) * numberOfCans) / 2) + firstRowOffset
                        ) + transform.position);
                }

                numberOfCans++;
            }

            firstRowOffset += (data.rows * (canWidth + horizontalSpace)) + 0.5f;
        }

        levelStacksPositions.Reverse();

        StopCoroutine("SpawnPile");
        StartCoroutine("SpawnPile");
    }

    float GetFirstRowOffset(List<CanStackData> stacks)
    {
        if (stacks.Count == 1) return 0;

        float offset = 0;
        
        foreach(CanStackData data in stacks)
        {
            offset += data.rows * (canWidth + horizontalSpace);
        }

        return offset / 2;
    }

    IEnumerator SpawnPile()
    {
        List<GameObject> cansAdded = new List<GameObject>();
        int cansInPileIndex = 0;

        foreach(Vector3 position in levelStacksPositions)
        {
            GameObject can = null;
            if(cansInPileIndex < cansInPile.Count)
            {
                can = cansInPile[cansInPileIndex];
                can.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                can.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                can.transform.SetPositionAndRotation(position, transform.rotation);
                can.GetComponent<Can>().Enable();

                cansInPileIndex++;
            }
            else
            {
                can = Instantiate(canPrefab, position, Quaternion.identity);
                cansAdded.Add(can);
            }

            yield return new WaitForSeconds(0.1f);
        }

        cansInPile.AddRange(cansAdded);
    }

    internal EndLevelResult EndLevel()
    {
        GameObject[] cans = cansInPile.ToArray();
        foreach (GameObject can in cans) can.GetComponent<Can>().Disable();

        return new EndLevelResult();
    }

    internal void AddCanToInactiveList(GameObject can)
    {
        //todo
    }
}

