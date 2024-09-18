using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SpawnerEncounter;

[CreateAssetMenu(fileName = "New Spawner Encounter", menuName = "Encounters/Spawner Encounter")]
public class SpawnerEncounter : Encounter
{
    [System.Serializable]
    public class SpawnerInfo
    {
        public GameObject spawnerPrefab;
        public int initialSpawnTimer;
        public Vector2Int position;
    }

    public List<SpawnerInfo> spawners;
}

[System.Serializable]
public class SpawnerRound : Round
{
    public List<SpawnerInfo> newSpawners;
}