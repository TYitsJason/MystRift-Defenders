using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public delegate void GameEvent();
    public static event GameEvent OnStructure;
    public static event GameEvent OnEnemyMove;
    public static event GameEvent OnTowersAtk;
    public static event GameEvent OnTowersGen;
    public static event GameEvent OnPlayerAct;

    public Button nextTurn;

    private void Start()
    {
        nextTurn.interactable = false;
    }
    public void NextTurn()
    {
        StartCoroutine(TurnSequence());
    }

    IEnumerator TurnSequence()
    {
        nextTurn.interactable = false;
        OnStructure?.Invoke();
        yield return new WaitForSeconds(1f);

        OnEnemyMove?.Invoke();
        yield return new WaitForSeconds(1f);

        OnTowersAtk?.Invoke();
        yield return new WaitForSeconds(1f);

        OnTowersGen?.Invoke();
        yield return new WaitForSeconds(1f);

        OnPlayerAct?.Invoke();
        yield return new WaitForSeconds(1f);
        nextTurn.interactable = true;
    }
}
