using UnityEngine;

public class Controller_PilesOfCans : MonoBehaviour
{
    [SerializeField] GameObject canPrefab;
    [SerializeField] float canWidth = 1f;
    [SerializeField] float canHeight = 1f;
    [SerializeField] float canDistance = 1.5f;
    [SerializeField] int numberOfCans = 3;

    Vector3[] cansPositions;

    private void Start()
    {
        cansPositions = new Vector3[numberOfCans];

        Setup();
        InstantiateCans();
    }

    void Setup()
    {
        int numberOfCansRemaining = numberOfCans;

        int row = 1;

        while (numberOfCansRemaining > 0)
        {
            float positionX = IsEvenRow(row) ? -(canWidth * canDistance) * row / 2 : 0;
            float positionY = (row - 1) * canHeight + canHeight / 2;

            for (int i = 0; i < row; i++)
            {
                if (numberOfCansRemaining <= 0) return;

                cansPositions[numberOfCansRemaining - 1] = new Vector3(positionX + (i * (canWidth * canDistance)), positionY, transform.position.z);

                numberOfCansRemaining--;
            }

            row++;
        }
    }

    void InstantiateCans()
    {
        for (int i = 0; i < numberOfCans; i++)
        {
            Instantiate(canPrefab, cansPositions[i], Quaternion.identity);
        }
    }

    bool IsEvenRow(int row) => row % 2 == 0;
}
