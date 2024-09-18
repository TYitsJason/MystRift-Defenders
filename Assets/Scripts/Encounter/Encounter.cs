using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEncounter", menuName = "Encounters/Encounter")]
public class Encounter : ScriptableObject
{
    public string encounterName;
    public List<Round> rounds;
}

[System.Serializable]
public class Round
{
    public int roundNumber;
    public List<SpawnInfo> spawnInfos;
}

[System.Serializable]
public class SpawnInfo
{
    public GameObject enemyPrefab;
    public int count;
    public List<int> lanes; // List of lanes where enemies can spawn
}
