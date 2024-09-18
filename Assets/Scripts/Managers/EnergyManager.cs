using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : Singleton<EnergyManager>
{
    [SerializeField] private int maxEnergy = 5;
    [SerializeField] private float minutesPerEnergyRecharge = 30f;

    private int currentEnergy;
    private DateTime lastEnergyUpdateTime;

    private const string ENERGY_KEY = "CurrentEnergy";
    private const string LAST_UPDATE_KEY = "LastEnergyUpdate";

    private void Start()
    {
        LoadEnergyData();
        StartCoroutine(UpdateEnergyRoutine());
    }

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private bool useDebugTime = false;
    [SerializeField] private float debugTimeScale = 60f; // 1 minute per second
    private float debugTimePassed = 0f;

    private void Update()
    {
        if (useDebugTime)
        {
            debugTimePassed += Time.deltaTime * debugTimeScale;
            if (debugTimePassed >= 60f) // Check every simulated minute
            {
                UpdateEnergy();
                debugTimePassed = 0f;
            }
        }
    }

    [ContextMenu("Refill Energy")]
    private void DebugRefillEnergy()
    {
        currentEnergy = maxEnergy;
        SaveEnergyData();
        Debug.Log($"Energy refilled to {currentEnergy}");
    }

    [ContextMenu("Use Energy")]
    private void DebugUseEnergy()
    {
        if (TryUseEnergy())
        {
            Debug.Log($"Energy used. Current energy: {currentEnergy}");
        }
        else
        {
            Debug.Log("Not enough energy");
        }
    }

    [ContextMenu("Log Energy Status")]
    private void DebugLogEnergyStatus()
    {
        Debug.Log($"Current Energy: {GetCurrentEnergy()}");
        Debug.Log($"Time until next energy: {GetTimeUntilNextEnergy() / 60f} minutes");
    }
#endif

    private void LoadEnergyData()
    {
        currentEnergy = PlayerPrefs.GetInt(ENERGY_KEY, maxEnergy);
        long lastUpdateTicks = Convert.ToInt64(PlayerPrefs.GetString(LAST_UPDATE_KEY, DateTime.Now.Ticks.ToString()));
        lastEnergyUpdateTime = new DateTime(lastUpdateTicks);

        UpdateEnergy();
    }

    private void UpdateEnergy()
    {
        TimeSpan timeSinceLastUpdate = DateTime.Now - lastEnergyUpdateTime;
        int energyToAdd = Mathf.FloorToInt((float)timeSinceLastUpdate.TotalMinutes / minutesPerEnergyRecharge);

        if (energyToAdd > 0)
        {
            currentEnergy = Mathf.Min(currentEnergy + energyToAdd, maxEnergy);
            lastEnergyUpdateTime = DateTime.Now;
            SaveEnergyData();
        }
    }

    private IEnumerator UpdateEnergyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(60);
            UpdateEnergy();
        }
    }

    public bool TryUseEnergy()
    {
        if (currentEnergy > 0)
        {
            currentEnergy--;
            SaveEnergyData();
            return true;
        }
        return false;
    }

    private void SaveEnergyData()
    {
        PlayerPrefs.SetInt(ENERGY_KEY, currentEnergy);
        PlayerPrefs.SetString(LAST_UPDATE_KEY, lastEnergyUpdateTime.Ticks.ToString());
        PlayerPrefs.Save();
    }

    public int GetCurrentEnergy()
    {
        UpdateEnergy(); // Ensure energy is up-to-date before returning
        return currentEnergy;
    }

    public float GetTimeUntilNextEnergy()
    {
        if (currentEnergy >= maxEnergy) return 0;

        TimeSpan timeSinceLastUpdate = DateTime.Now - lastEnergyUpdateTime;
        float minutesSinceLastUpdate = (float)timeSinceLastUpdate.TotalMinutes;
        float minutesUntilNextEnergy = minutesPerEnergyRecharge - (minutesSinceLastUpdate % minutesPerEnergyRecharge);

        return minutesUntilNextEnergy * 60; // Return seconds
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus) // Game is resuming
        {
            UpdateEnergy();
        }
    }

    private void OnApplicationQuit()
    {
        SaveEnergyData();
    }
}
