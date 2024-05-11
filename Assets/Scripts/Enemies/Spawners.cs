using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawners : EnemyStructure
{
    public GameObject spawnedUnit;
    public int SpawnTimer;
    public int InitialSpawnTimer;

    protected override void Awake()
    {
        base.Awake();
        SpawnTimer = InitialSpawnTimer;
    }

    void SpawnUnit()
    {
        Instantiate(spawnedUnit, transform.position, Quaternion.identity);
        SpawnTimer = InitialSpawnTimer;
    }

    // Listener for the OnStructure event
    protected virtual void OnEnable()
    {
        GameManager.OnStructure += OnAct;
    }
    protected virtual void OnDisable()
    {
        GameManager.OnStructure -= OnAct;
    }
    protected virtual void OnAct()
    {
        AdvanceSpawnTimer();
    }
    public virtual void AdvanceSpawnTimer()
    {
        if (SpawnTimer > 0)
        {
            SpawnTimer--;
        }
        if (SpawnTimer <= 0)
        {
            SpawnUnit();
        }
    }
}
