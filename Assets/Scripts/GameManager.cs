using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public CharacterLane playerLane;
    public CharacterLane enemyLane;
    public Button endRoundButton;

    public Transform bottomPanel;
    private bool playing;

    void Start()
    {
        var playerActions = playerLane.GetTurnActions();
        LogActions(playerActions);
        ShowPlayerHand(playerActions);

        var enemyActions = enemyLane.GetTurnActions();
        GenerateBaseTimeline(enemyActions);
        //LogActions(enemyActions);

        playing = true;
    }

    void LogActions(IEnumerable<CardAction> actions)
    {
        foreach (var turnAction in actions)
        {
            Debug.Log($"{turnAction.Source.name} {turnAction.Data.name}");
        }
    }

    void ShowPlayerHand(IEnumerable<CardAction> cards) {
        foreach (var card in cards) {
            var instance = Instantiate(card.Data.playerCardSprite);
            instance.GetComponent<CardUI>().Action = card;
            PlayerHand.Instance.AddCard(instance);
        }
    }

    void GenerateBaseTimeline(IEnumerable<CardAction> cards) {
        foreach (var card in cards) {
            TimelineHandler.Instance.AddCard(card);
        }
    }

    void Update()
    {
        if (!playing)
        {
            // unlock player
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            // enable button
            endRoundButton.interactable = true;
            IEnumerable<CardAction> enemyCards = enemyLane.GetComponent<CharacterLane>().GetTurnActions();
            GenerateBaseTimeline(enemyCards);
            playing = true;
        }
    }

    public void EndRound()
    {
        Debug.Log("Round Ended");
        // lock player action (mouse)
        playing = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        // disable button
        endRoundButton.interactable = false;

        // Start fight animation
        StartCoroutine(round());

        // start turn todo (draw cards and etc)
    }

    IEnumerator round()
    {
        // play animation of attack
        int numCard = TimelineHandler.Instance.GetNumberOfCards();
        for (int i = 0; i < numCard; i++) {
            yield return new WaitForSeconds(2);
            var nextAction = TimelineHandler.Instance.RemoveTopCard();
            
            ExecuteAction(nextAction.Action);

            Destroy(nextAction.gameObject);
            TimelineHandler.Instance.updateCanvas();
        }

        // give enemies new attack in timeline
        playing = false;
    }

    void ExecuteAction(CardAction action)
    {
        switch (action.Target)
        {
           case TargetType.Player:
               playerLane.ExecuteAction(action.Data);
               break;
           case TargetType.Enemy:
               enemyLane.ExecuteAction(action.Data);
               break;
        }
    }
}
