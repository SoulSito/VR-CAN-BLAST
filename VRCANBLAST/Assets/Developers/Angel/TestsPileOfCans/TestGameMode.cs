using System.Collections.Generic;
using UnityEngine;

public class TestGameMode : MonoBehaviour
{
    public List<LevelData> levels;
    public CanStackSpawner spawner;

    int currentLevel = 0;

    void Start()
    {
        LoadLevel(currentLevel);
    }

    public void Reload()
    {
        currentLevel++;
        LoadLevel(currentLevel);
    }

    void LoadLevel(int index)
    {
        if (index < 0 || index >= levels.Count)
            return;

        spawner.SpawnLevel(levels[index]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) Reload();
    }
}