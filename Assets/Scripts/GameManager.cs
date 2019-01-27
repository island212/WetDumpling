using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public CharacterLane playerLane;
    public CharacterLane enemyLane;
    public Button endRoundButton;
    public GameObject gameOver;

    public Transform bottomPanel;
    private bool playing;
    private int currentLevel = 0;

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            var playerActions = playerLane.GetTurnActions();
            LogActions(playerActions);
            ShowPlayerHand(playerActions);
        }

        var enemyActions = enemyLane.GetTurnActions();
        GenerateBaseTimeline(enemyActions);
        //LogActions(enemyActions);

        playing = true;
    }

    void LogActions(IEnumerable<CardAction> actions)
    {
        foreach (var action in actions)
        {
            Debug.Log($"{action.Source.name} {action.Data.name}");
        }
    }

    void ShowPlayerHand(IEnumerable<CardAction> cards) {
        foreach (var cardAction in cards)
        {
            var card = InstantiateCardAction(cardAction, "Player");
            PlayerHand.Instance.AddCard(card);
        }
    }

    void GenerateBaseTimeline(IEnumerable<CardAction> cards) {
        foreach (var cardAction in cards)
        {
            var card = InstantiateCardAction(cardAction, "Enemy");
            TimelineHandler.Instance.AddCard(card);
        }
    }

    GameObject InstantiateCardAction(CardAction card, string tag)
    {
        string path = $"Prefabs/Cards/{tag}/{tag}{card.Data.name}";
        Debug.Log(path);
        var spritePrefab = Resources.Load<GameObject>(path);
        var instance = Instantiate(spritePrefab);
        instance.GetComponent<CardUI>().Action = card;
        return instance;
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
            if (playerLane.updateGameState())
            {
                // Next level
                ToNextLevel();
            }
            if (enemyLane.updateGameState())
            {
                // Game over
                GameOver();
            }

            var playerActions = playerLane.GetTurnActions();
            LogActions(playerActions);
            ShowPlayerHand(playerActions);

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
        for (int i = 0; i < numCard; i++) 
        {
            // Do attack things with animation for damage or defence
            yield return new WaitForSeconds(1);
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
        var targetData = action.Data.targetData;
        var selfData = action.Data.selfData;
        if (action.Source.IsPlayer)
        {
            if(selfData != null)
                playerLane.ExecuteAction(selfData);
            if(targetData != null)
                enemyLane.ExecuteAction(targetData);
        }
        else
        {
            if(selfData != null)
                enemyLane.ExecuteAction(selfData);
            if(targetData != null)
                playerLane.ExecuteAction(targetData);
        }
    }

    private void ToNextLevel()
    {
        currentLevel++;
        LoadLevel();
    }

    private void LoadLevel()
    {
        // TODO
    }

    IEnumerator GameOver()
    {
        gameOver.SetActive(true);
        // spawn game over text
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("Menu");
    }
}
