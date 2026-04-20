using UnityEngine;

public class CanStackSpawner : MonoBehaviour
{
    public GameObject canPrefab;
    public float spacing = 2f;

    public void SpawnLevel(LevelData level)
    {
        Clear();

        int numberOfStacks = level.stacks.Count;
        float offsetX = -(numberOfStacks % 2 == 0 ? (spacing / 2) : spacing) * numberOfStacks / 2;

        foreach (var stack in level.stacks)
        {
            //SpawnStack(stack.cansAmount, new Vector3(offsetX, 0, 0) + transform.position);
            offsetX += spacing; 
        }
    }

    void SpawnStack(int count, Vector3 origin)
    {
        BoxCollider col = canPrefab.GetComponent<BoxCollider>();

        float spacingMultiplier = 1.5f;
        float width = col.size.x * spacingMultiplier;
        float height = col.size.y;
        
        if (count == 2)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector3 position = origin + new Vector3(0, i * height, 0);
                Instantiate(canPrefab, position, Quaternion.identity, transform);
            }
            return;
        }

        int rows = CalculateRows(count);
        int currentCan = 0;

        for (int row = 0; row < rows; row++)
        {
            int cansInRow = rows - row;

            for (int i = 0; i < cansInRow; i++)
            {
                if (currentCan >= count)
                    return;

                float xOffset = i * width - (cansInRow - 1) * width / 2;
                float yOffset = row * height;

                Vector3 position = origin + new Vector3(xOffset, yOffset, 0);

                Instantiate(canPrefab, position, Quaternion.identity, transform);
                currentCan++;
            }
        }
    }

    int CalculateRows(int canCount)
    {
        int rows = 0;
        int total = 0;

        while (total < canCount)
        {
            rows++;
            total += rows;
        }

        return rows;
    }

    void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}