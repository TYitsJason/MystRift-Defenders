using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    public List<Encounter> EasyEncounters;
    public List<Encounter> HardEncounters;
    public List<Encounter> BossEncounters;
    public List<SpawnerEncounter> SpawnerEncounters;
    public GameObject rewardDoorPrefab;
    public Transform rewardDoorParent;

    private Encounter currentEncounter;
    private int currentRound = 0;
    private bool allEnemiesSpawned = false;
    private List<int> lastSpawnedLanes = new List<int>();
    private RewardType selectedReward;
    public List<Encounter> completedEncounters = new List<Encounter>();
    private bool boss = false;

    public GameObject door1;
    public GameObject door2;

    void OnEnable()
    {
        GameManager.OnStructure += OnStructure;
    }

    void OnDisable()
    {
        GameManager.OnStructure -= OnStructure;
    }

    public void StartNewEncounter()
    {
        List<Encounter> encounters;
        // The first 4 encounters are easy
        if (completedEncounters.Count > 4)
            encounters = HardEncounters;

        // The next 4 encounters are hard
        else if (completedEncounters.Count < 8)
        encounters = EasyEncounters;

        // The final encounter is a boss fight
        else
        {
            encounters = BossEncounters;
            boss = true;
        }
        
        // This catches the error if there are no encounters available
        if (encounters.Count <= 0)
        { 
            Debug.LogError("No encounters available!");
            return;
        }

        // Randomly select an encounter from the list, while ensuring that the player never fights duplicate encounters in each run
        int index;
        do
        {
            index = Random.Range(0, encounters.Count);
        } while (completedEncounters.Contains(encounters[index]));

        currentEncounter = encounters[index];
        currentRound = 0;
        allEnemiesSpawned = false;
        completedEncounters.Add(currentEncounter);
    }

    public void StartNewSpawnerEncounter()
    {
        SpawnerEncounter spawnerEncounter = SpawnerEncounters[0]; // Just placeholder for now
        currentEncounter = spawnerEncounter;
        foreach (var spawnerInfo in spawnerEncounter.spawners)
        {
            GameObject spawner = Instantiate(spawnerInfo.spawnerPrefab,
                new Vector3(spawnerInfo.position.x, 0, spawnerInfo.position.y),
                Quaternion.identity);

            Spawners spawnerComponent = spawner.GetComponent<Spawners>();
            spawnerComponent.spawnPosition = new Vector2Int(spawnerInfo.position.x, spawnerInfo.position.y);

            // Set up the grid cell
            GridCell cell = GridManager.Instance.grid[spawnerInfo.position.x, spawnerInfo.position.y];
            cell.IsOccupiedByEnemyStructure = true;
            cell.structure = spawner.GetComponent<EnemyStructure>();
        }
        completedEncounters.Add(currentEncounter);
    }

    // This is called when the game manager calls the function to spawn enemies each round
    private void OnStructure()
    {
        currentRound++;
        foreach (var round in currentEncounter.rounds)
        {
            if (round.roundNumber == currentRound)
            {
                StartCoroutine(SpawnEnemies(round.spawnInfos));
            }
        }
    }

    private IEnumerator SpawnEnemies(List<SpawnInfo> spawnInfos)
    {
        foreach (var spawnInfo in spawnInfos)
        {
            for (int i = 0; i < spawnInfo.count; i++)
            {
                int lane;
                do
                {
                    lane = spawnInfo.lanes[Random.Range(0, spawnInfo.lanes.Count)];
                } while (lastSpawnedLanes.Contains(lane));

                lastSpawnedLanes.Add(lane);
                if (lastSpawnedLanes.Count > 1)
                {
                    lastSpawnedLanes.RemoveAt(0);
                }

                Vector3 spawnPosition = new Vector3(GridManager.Instance.width - 1, 0.25f, lane);
                Enemy enemy = Instantiate(spawnInfo.enemyPrefab, spawnPosition, Quaternion.identity).GetComponentInChildren<Enemy>();
                enemy.position = new Vector2Int(GridManager.Instance.width - 1, lane);

                yield return new WaitForSeconds(0.01f);
            }
        }

        allEnemiesSpawned = true;
        StartCheckingForCompletion();
    }

    private Coroutine checkCompletionCoroutine;

    private void StartCheckingForCompletion()
    {
        if (checkCompletionCoroutine != null)
            StopCoroutine(checkCompletionCoroutine);
        checkCompletionCoroutine = StartCoroutine(CheckForEncounterCompletion());
    }

    public void StopEncounter()
    {
        if (checkCompletionCoroutine != null)
            StopCoroutine(checkCompletionCoroutine);
        OnEncounterCompleted();
    }

    // This coroutine runs while the encounter is in progress and calls the end when all enemies have been spawned and killed
    private IEnumerator CheckForEncounterCompletion()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (allEnemiesSpawned && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                StopEncounter();
                break;
            }
        }
    }

    // This presents the card reward and the additional reward chosen before the start of the encounter
    private void OnEncounterCompleted()
    {
        DisableGameplayUI();
        OfferCardReward();
        StartCoroutine(WaitForCardSelection());
    }

    private void DisableGameplayUI()
    {
        DeckManager.Instance.GUI.gameObject.SetActive(false);
    }

    private void OfferCardReward()
    {
        RunManager.Instance.OfferNewCards(boss);
    }

    // This runs while the card selection ui is up, then it presents the second award, then lets the player choose the reward for the next encounter
    private IEnumerator WaitForCardSelection()
    {
        // Wait for initial card selection
        while (RunManager.Instance.canChooseReward)
        {
            yield return null;
        }

        // Reset the flag
        RunManager.Instance.canChooseReward = false;

        // Present the selected additional reward
        PresentAdditionalReward(selectedReward);

        // Wait for additional reward processing
        while (RunManager.Instance.canChooseReward)
        {
            yield return null;
        }

        // Now that all rewards are processed, present doors for next encounter
        PresentRewardSelectionForNextEncounter();
    }

    private void PresentAdditionalReward(RewardType reward)
    {
        switch (reward)
        {
            case RewardType.Gold:
                RunManager.Instance.gold += 100;
                break;
            case RewardType.AnotherCard:
                RunManager.Instance.canChooseReward = true;
                RunManager.Instance.OfferNewCards(false);
                break;
        }
    }

    private void PresentRewardSelectionForNextEncounter()
    {
        // Remove any existing reward doors
        foreach (Transform child in rewardDoorParent)
        {
            Destroy(child.gameObject);
        }

        // Randomly select two different rewards
        List<RewardType> availableRewards = new List<RewardType> { RewardType.Gold, RewardType.AnotherCard }; // Add other reward types as needed
        List<RewardType> selectedRewards = new List<RewardType>();
        while (selectedRewards.Count < 2)
        {
            RewardType reward = availableRewards[Random.Range(0, availableRewards.Count)];
            if (!selectedRewards.Contains(reward))
            {
                selectedRewards.Add(reward);
            }
        }

        // Instantiate reward doors
        door1 = Instantiate(rewardDoorPrefab, rewardDoorParent);
        door2 = Instantiate(rewardDoorPrefab, rewardDoorParent);

        door1.transform.position = new Vector3(9, 1, 0);
        door2.transform.position = new Vector3(9, 1, 2);

        RewardDoor door1Reward = door1.GetComponent<RewardDoor>();
        RewardDoor door2Reward = door2.GetComponent<RewardDoor>();
        door1Reward.SetReward(selectedRewards[0], this);
        door2Reward.SetReward(selectedRewards[1], this);
    }

    public void OnRewardSelected(RewardType reward)
    {
        selectedReward = reward;
        Destroy(door1.gameObject);
        Destroy(door2.gameObject);
        RunManager.Instance.StartNewEncounter();
    }
}

public enum RewardType
{
    Gold,
    AnotherCard,
    None,
    // More reward types will be added later
}
