using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardDoor : MonoBehaviour
{
    private RewardType reward;
    private EncounterManager encounterManager;
    private MeshRenderer mr;
    public Material gold;
    public Material card;

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }

    public void SetReward(RewardType reward, EncounterManager encounterManager)
    {
        this.reward = reward;
        this.encounterManager = encounterManager;
        switch (reward)
        {
            case RewardType.None:
                Debug.Log("No reward");
                break;
            case RewardType.Gold:
                mr.material = gold;
                break;
            case RewardType.AnotherCard:
                mr.material = card;
                break;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Works for both mouse click and touch
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform == this.transform)
            {
                OnDoorSelected();
            }
        }
    }

    private void OnDoorSelected()
    {
        if (encounterManager != null)
        {
            encounterManager.OnRewardSelected(reward);
            RunManager.Instance.UpdateGold();
        }
        else
        {
            Debug.LogError("EncounterManager is not set!");
        }
    }
}