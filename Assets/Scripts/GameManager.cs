using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public CharacterLane playerLane;
    public CharacterLane enemyLane;
    public Button endRoundButton;
    public GameObject gameOver;

    private int currentLevel = 0;
    private bool isPlayerGameOver;
    private bool isEnemyGameOver;
    public float waitTimeBetweenActions = 0.5f;

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            var playerActions = playerLane.GetTurnActions();
            ShowPlayerHand(playerActions);
        }

        var enemyActions = enemyLane.GetTurnActions();
        GenerateEnemyTimeline(enemyActions);
    }

    private static void ShowPlayerHand(IEnumerable<CardAction> cards)
    {
        foreach (var cardAction in cards)
        {
            var card = InstantiateCardAction(cardAction, "Player");
            PlayerHand.Instance.AddCard(card);
        }
    }

    private static void GenerateEnemyTimeline(IEnumerable<CardAction> cards)
    {
        foreach (var cardAction in cards)
        {
            var card = InstantiateCardAction(cardAction, "Enemy");
            TimelineHandler.Instance.AddCard(card);
        }
    }

    private static GameObject InstantiateCardAction(CardAction card, string tag)
    {
        var prefab = Resources.Load<GameObject>($"Prefabs/Cards/{tag}Card");
        var instance = Instantiate(prefab);
        instance.GetComponent<CardUI>().Action = card;
        instance.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/Card{card.Data.name}");
        return instance;
    }

    public void EndRound()
    {
        Debug.Log("PlayTimeline Ended");
        // lock player action (mouse)
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        // disable button
        endRoundButton.interactable = false;

        StartCoroutine(PlayTimeline(SetupNextTurn));
    }

    private IEnumerator PlayTimeline(Action done)
    {
        // play animation of attack
        int numCard = TimelineHandler.Instance.GetNumberOfCards();
        for (int i = 0; i < numCard; i++)
        {
            // Do attack things with animation for damage or defence
            yield return new WaitForSeconds(waitTimeBetweenActions);
            var nextAction = TimelineHandler.Instance.RemoveTopCard();

            ExecuteAction(nextAction.Action);

            //Check game over state

            if (enemyLane.IsGameOver())
            {
                // Next level
                ToNextLevel();
            }

            if (playerLane.IsGameOver())
            {
                // Game over
                GameOver();
            }

            Destroy(nextAction.gameObject);
            TimelineHandler.Instance.updateCanvas();
        }

        done?.Invoke();
    }

    void SetupNextTurn()
    {
        // unlock player
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        // enable button
        endRoundButton.interactable = true;
        var enemyCards = enemyLane.GetComponent<CharacterLane>()
                                  .GetTurnActions();

        GenerateEnemyTimeline(enemyCards);

        var playerActions = playerLane.GetTurnActions();
        ShowPlayerHand(playerActions);
    }

    void ExecuteAction(CardAction action)
    {
        var targetData = action.Data.targetData;
        var selfData = action.Data.selfData;

        if (action.Source.IsPlayer)
        {
            Debug.Log($"execute player action target: {targetData == null} self: {selfData == null}");
            if (action.Data.selfData != null)
                playerLane.ExecuteAction(action.Data.selfData);
            if (targetData != null)
                enemyLane.ExecuteAction(action.Data.targetData);
        }
        else
        {
            Debug.Log($"execute enemy action target: {targetData == null} self: {selfData == null}");
            if (action.Data.selfData != null)
                enemyLane.ExecuteAction(action.Data.selfData);
            if (targetData != null)
                playerLane.ExecuteAction(action.Data.targetData);
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