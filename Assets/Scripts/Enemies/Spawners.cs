using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawners : EnemyStructure
{
    public GameObject spawnedUnit;
    public int SpawnTimer;
    public int InitialSpawnTimer;
    public Vector2Int spawnPosition;

    protected override void Awake()
    {
        base.Awake();
        SpawnTimer = InitialSpawnTimer;
    }

    void SpawnUnit()
    {
        Vector3 spawnWorldPosition = new Vector3(spawnPosition.x, 0.25f, spawnPosition.y);
        Instantiate(spawnedUnit, transform.position, Quaternion.identity);
        SpawnTimer = InitialSpawnTimer;
        Enemy enemyComponent = spawnedUnit.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            enemyComponent.position = spawnPosition;
        }
        else Debug.Log("No enemy component found");
    }

    // Listener for the OnStructure event
    
    protected override void OnAct()
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
