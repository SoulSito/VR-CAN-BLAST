using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PileOfCans : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] GameObject canPrefab;
    [SerializeField] int maxInactiveCans = 20;
    [SerializeField] float spaceBetweenStacks = 1f;

    List<GameObject> inactiveCans = new();
    List<GameObject> cansInPile = new();

    Vector3[][] levelStacksPositions;
    float canWidth = 1f;
    float canHeight = 1f;

    private void Start()
    {
        BoxCollider collider = canPrefab.GetComponent<BoxCollider>();

        canWidth = collider.size.x * 1.5f;
        canHeight = collider.size.y * 1.5f;
    }

    internal void PlaceLevel(LevelData level)
    {
        int numberOfStacks = level.stacks.Count;
        int totalNumberOfRows = 0;

        levelStacksPositions = new Vector3[numberOfStacks][];

        for (int i = 0; i < numberOfStacks; i++)
        {
            CanStackData canStackData = level.stacks[i];

            int rowsInStack = CalculateRows(canStackData.cansAmount);
            levelStacksPositions[i] = new Vector3[canStackData.cansAmount];

            CalculateStackPositions(i, rowsInStack);

            totalNumberOfRows += rowsInStack;
        }

        float startingXPosition = CalculateStartingXPosition(totalNumberOfRows);
        float offsetX = startingXPosition;

        for(int i = 0; i < levelStacksPositions.Length; i++)
        {
            PlaceStack(i, offsetX);

            offsetX += canWidth * levelStacksPositions[i].Length;
        }
        
    }

    void CalculateStackPositions(int stackIndex, int rowsInStack)
    {
        int currentRowCans = rowsInStack;
        int currentCan = 1;
        int currentRow = 0;

        float stackOffsetZ = -((rowsInStack / 2) * canWidth) + rowsInStack % 2 == 0 ? canWidth / 2 : 0;

        for(int i = 0; i < levelStacksPositions[stackIndex].Length; i++)
        {
            levelStacksPositions[stackIndex][i] = new Vector3(
                0,
                canHeight * currentRow,
                stackOffsetZ + canWidth * currentCan
                );

            currentCan++;

            if(currentCan > currentRowCans)
            {
                currentCan = 1;
                currentRowCans--;
                currentRow++;

                stackOffsetZ = -((currentRowCans / 2) * canWidth) + currentRowCans % 2 == 0 ? canWidth / 2 : 0;

                if (currentRow > rowsInStack) return;
            }
        }




        /*for(int i = 0; i < rowsInStack; i++)
        {
            for (int can = 0; can < cansInRow; can++)
            {
                levelStacksPositions[stackIndex][i + can] = new Vector3(
                    stackOffsetX + canWidth * can,
                    i * canHeight, 
                    0
                    );
            }

            cansInRow--;
        }*/

        StopAllCoroutines();
        StartCoroutine(PlaceCanStack(stackIndex));
    }

    IEnumerator PlaceCanStack(int index)
    {
        foreach (Vector3 position in levelStacksPositions[index])
        {
            GameObject can = Instantiate(canPrefab, position + transform.position, Quaternion.identity);
            cansInPile.Add(can);

            yield return new WaitForSeconds(0.2f);
        }
    }

    float CalculateStartingXPosition(int totalNumberOfRows)
    {
        // TODO
        return 0f;
    }

    void PlaceStack(int index, float offsetX)
    {
        // TODO
    }

    internal EndLevelResult EndLevel()
    {
        // TODO: Comprobar latas derribadas sobre la mesa

        ResetPile();

        return new EndLevelResult(0, 0);
    }

    internal void AddCanToInactiveList(GameObject can)
    {
        if (inactiveCans.Contains(can)) return;

        cansInPile.Remove(can);

        if (inactiveCans.Count > maxInactiveCans) Destroy(can);
        else inactiveCans.Add(can);
    }

    void ResetPile()
    {
        foreach (GameObject can in cansInPile)
        {
            can.GetComponent<Can>().Disable();

            AddCanToInactiveList(can);
        }

        cansInPile.Clear();
    }

    int CalculateRows(int numberOfTotalCans)
    {
        int rows = 0;
        int cansCounted = 0;

        while(cansCounted < numberOfTotalCans)
        {
            rows++;
            cansCounted += rows;
        }

        return rows;
    }
}
