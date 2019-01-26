using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CharacterLane playerLane;
    public CharacterLane enemyLane;

    private bool playing;

    void Start()
    {
        var playerActions = playerLane.GetTurnActions();
        LogActions(playerActions);

        var enemyActions = enemyLane.GetTurnActions();
        LogActions(enemyActions);
    }

    void LogActions(IEnumerable<CardAction> actions)
    {
        foreach (var turnAction in actions)
        {
            Debug.Log($"{turnAction.Source.name} {turnAction.Data.name}");
        }
    }

    void Update()
    {
        if (playing == false)
        {
            // unlock player
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void StartRound()
    {
        Debug.Log("Round Started");
        // lock player action (mouse)
        playing = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


        // Start fight animation
        StartCoroutine(round());

        // start turn todo (draw cards)
    }

    IEnumerator round()
    {
        // play animation of attack
        int numCard = TimelineHandler.Instance.getNumberOfCards();
        for (int i = 0; i < numCard; i++) {
            yield return new WaitForSeconds(2);
            Transform nextAction = TimelineHandler.Instance.removeTopCard();
            // Do attack things with animation for damage or defence
            Destroy(nextAction.gameObject);
            TimelineHandler.Instance.updateCanvas();
        }

        // give enemies new attack in timeline
        playing = false;
    }
}
