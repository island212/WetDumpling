﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[Serializable]
public class level
{
    public GameObject[] character;
}

public class GameManager : MonoBehaviour
{
    public CharacterLane playerLane;
    public CharacterLane enemyLane;
    public Button endRoundButton;
    public GameObject gameOver;
    public GameObject playerPos;
    public GameObject enemyPos;
    public level[] levels;

    private int currentLevel = 0;
    private bool isPlayerGameOver;
    private bool isEnemyGameOver;
    public float waitTimeBetweenActions = 0.5f;

    void Start()
    {
        LoadLevel();

        var playerActions = playerLane.GetTurnActions();
        ShowPlayerHand(playerActions);

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
            yield return new WaitForSeconds(waitTimeBetweenActions / 2);
            var nextAction = TimelineHandler.Instance.RemoveTopCard();

            yield return ExecuteAction(nextAction.Action);

            //Check game over state

            if (enemyLane.IsGameOver())
            {
                // Next level
                ToNextLevel();
            }

            if (playerLane.IsGameOver())
            {
                // Game over
                StartCoroutine(GameOver());
            }

            Destroy(nextAction.gameObject);
            TimelineHandler.Instance.updateCanvas();
        }

        done?.Invoke();
    }

    private void SetupNextTurn()
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

    private IEnumerator ExecuteAction(CardAction action)
    {
        var targetData = action.Data.targetData;
        var selfData = action.Data.selfData;

        if (action.Source.IsPlayer)
        {
            if (selfData != null)
                playerLane.ExecuteAction(selfData);

            if (targetData != null) 
            {
                if (targetData.damage > 0)
                {
                    Vector3 initialPos = action.Source.gameObject.transform.GetChild(0).gameObject.transform.position;
                    iTween.MoveTo(action.Source.gameObject.transform.GetChild(0).gameObject,
                      iTween.Hash("position", enemyPos.transform.position,
                                  "easeType", "easeInQuart",
                                  "time", 0.49f));

                    iTween.MoveTo(action.Source.gameObject.transform.GetChild(0).gameObject,
                      iTween.Hash("position", initialPos,
                                  "time", 0.49f,
                                  "delay", 0.49f));

                    yield return StartCoroutine(ExecuteWithCoroutine(enemyLane, targetData));
                }
                else
                {
                    enemyLane.ExecuteAction(targetData);
                }
            }
        }
        else
        {

            if (selfData != null)
            {
                // Animations de shield/heal ici.              

                enemyLane.ExecuteAction(selfData);
            }

            if (targetData != null) 
            {
               if (targetData.damage > 0) 
               {
                  if (action.Source != null)
                  {
                      Vector3 initialPos = action.Source.gameObject.transform.GetChild(0).gameObject.transform.position;
                      iTween.MoveTo(action.Source.gameObject.transform.GetChild(0).gameObject,
                        iTween.Hash("position", playerPos.transform.position,
                                    "easeType", "easeInQuart",
                                    "time", 0.5f));

                      iTween.MoveTo(action.Source.gameObject.transform.GetChild(0).gameObject,
                        iTween.Hash("position", initialPos,
                                    "time", 0.5f,
                                    "delay", 0.5f));

                      yield return StartCoroutine(ExecuteWithCoroutine(playerLane, targetData));
                  }
               }
               else 
               {
                  playerLane.ExecuteAction(targetData);
               }
            }
        }
    }

    private IEnumerator ExecuteWithCoroutine(CharacterLane lane, CardData data) 
    {
        yield return new WaitForSeconds(waitTimeBetweenActions / 2);
        lane.ExecuteAction(data);
    }

    private void ToNextLevel()
    {
        currentLevel++;
        LoadLevel();
    }

    private void LoadLevel()
    {
        PlayerHand.Instance.Clear();
        playerLane.getPlayer().ResetDeck();
        for (int i = 0; i < 4; i++)
        {
            var playerActions = playerLane.GetTurnActions();
            ShowPlayerHand(playerActions);
        }

        int index = 0;
        foreach (var i in levels[currentLevel].character) {
            if (!levels[currentLevel].character[index])
            {
                break;
            }
            enemyLane.AddCharacter(i, index);
            index++;
        }
    }

    IEnumerator GameOver()
    {
        gameOver.SetActive(true);
        // spawn game over text
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("Menu");
    }
}