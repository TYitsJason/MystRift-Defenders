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
    public static event GameEvent OnPlayerAct;

    public Button nextTurn;

    public Canvas GameOverScreen;
    public Text GameOverMessage;

    private void Start()
    {
        GameOverScreen.gameObject.SetActive(false);
        if (nextTurn != null)
        {
            nextTurn.interactable = false;
        }
    }
    public void NextTurn()
    {
        DeckManager.Instance.EndTurn();
        StartCoroutine(TurnSequence(false));
    }

    public void StartGame()
    {
        StartCoroutine(TurnSequence(true));
    }

    IEnumerator TurnSequence(bool firstTurn)
    {
        nextTurn.interactable = false;

        // Reset lifeline usage at the start of each turn
        RunManager.Instance.ResetLifelineUsage();
        if (firstTurn)
            yield return new WaitForSeconds(1f);
        OnStructure?.Invoke();
        Debug.Log("Enemy Spawning");
        yield return new WaitForSeconds(1f);

        Debug.Log("Enemy Acting");
        OnEnemyMove?.Invoke();
        yield return new WaitForSeconds(1f);

        Debug.Log("Towers Acting");
        OnTowersAtk?.Invoke();
        yield return new WaitForSeconds(1f);

        Debug.Log("Player turn starting");
        OnPlayerAct?.Invoke();
        yield return new WaitForSeconds(1f);
        nextTurn.interactable = true;
    }

    public void GlobalAttack()
    {
        StartCoroutine(TowerAction());
    }

    IEnumerator TowerAction()
    {
        nextTurn.interactable = false;
        Debug.Log("Manual Tower action phase starting");
        OnTowersAtk?.Invoke();
        yield return new WaitForSeconds(1f);
        nextTurn.interactable = true;
    }

    public void GameOver(bool win)
    {
        GameOverScreen.gameObject.SetActive(true);
        if (win)
            GameOverMessage.text = "Thanks for playing the beta, shops, bosses and more cards coming soon!";
        else
            GameOverMessage.text = "You died, but you can always try again!";
    }
}
