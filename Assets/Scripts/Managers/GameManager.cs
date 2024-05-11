using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public delegate void GameEvent();
    public static event GameEvent OnStructure;
    public static event GameEvent OnEnemyMove;
    public static event GameEvent OnTowersAtk;
    public static event GameEvent OnTowersGen;
    public static event GameEvent OnPlayerAct;

    public void NextTurn()
    {
        StartCoroutine(TurnSequence());
    }

    IEnumerator TurnSequence()
    {
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
    }
}
