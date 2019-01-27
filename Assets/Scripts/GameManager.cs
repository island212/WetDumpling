using System;
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
    public Fade intermission;
    public GameObject playerPos;
    public GameObject enemyPos;
    public level[] levels;

    private int currentLevel = 0;
    private bool isPlayerGameOver;
    private bool isEnemyGameOver;
    public float waitTimeBetweenActions = 0.0f;

    void Start()
    {
        StartCoroutine(LoadLevel());
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

            //Check game over state

            if (enemyLane.IsGameOver())
            {
                // Next level
                TimelineHandler.Instance.Clear();
                ToNextLevel();
                StartCoroutine(intermission.FadeAnim());
                break;
            }

            if (playerLane.IsGameOver())
            {
                // Game over
                StartCoroutine(GameOver());
            }

            var nextAction = TimelineHandler.Instance.RemoveTopCard();
            if (nextAction.Action.Source)
            {
                yield return ExecuteAction(nextAction.Action);
            }
            else
            {
                // Card is from a dead character
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
        if (action.Source.IsPlayer)
            yield return ExecutePlayerAction(action);
        else
            yield return ExecuteEnemyAction(action);
    }

    private IEnumerator ExecutePlayerAction(CardAction action)
    {
        var targetData = action.Data.targetData;
        var selfData = action.Data.selfData;

        if (selfData != null)
            yield return SelfAnimation(playerLane, selfData, action);

        if (targetData == null)
            yield break;

        if (targetData.damage > 0)
            yield return TargetAnimation(enemyPos, enemyLane, targetData, action);
        else
            enemyLane.ExecuteAction(targetData);
    }

    IEnumerator ExecuteEnemyAction(CardAction action)
    {
        var targetData = action.Data.targetData;
        var selfData = action.Data.selfData;

        if (selfData != null)
            yield return SelfAnimation(enemyLane, selfData, action);

        if (targetData == null)
            yield break;

        if (targetData.damage > 0)
            yield return TargetAnimation(playerPos, playerLane, targetData, action);
        else
            playerLane.ExecuteAction(targetData);
    }

    IEnumerator SelfAnimation(CharacterLane lane, CardData data, CardAction action)
    {
        if (action.Source == null)
            yield break;

        var initialPos = action.Source.gameObject.transform.GetChild(0).gameObject.transform.position;
        iTween.MoveTo(action.Source.gameObject.transform.GetChild(0).gameObject,
            iTween.Hash(
                "position", new Vector3(initialPos.x, initialPos.y + .3f, initialPos.z),
                "time", 0.25f));

        iTween.MoveTo(action.Source.gameObject.transform.GetChild(0).gameObject,
            iTween.Hash("position", initialPos,
                "time", 0.25f,
                "delay", 0.25f));

        yield return StartCoroutine(ExecuteWithCoroutine(lane, data, waitTimeBetweenActions / 4));
    }

    IEnumerator TargetAnimation(GameObject pos, CharacterLane lane, CardData data, CardAction action)
    {
        if (action.Source == null)
            yield break;

        var initialPos = action.Source.gameObject.transform.GetChild(0).gameObject.transform.position;
        iTween.MoveTo(action.Source.gameObject.transform.GetChild(0).gameObject,
            iTween.Hash("position", pos.transform.position,
                "easeType", "easeInQuart",
                "time", 0.5f));

        iTween.MoveTo(action.Source.gameObject.transform.GetChild(0).gameObject,
            iTween.Hash("position", initialPos,
                "time", 0.5f,
                "delay", 0.5f));

        yield return StartCoroutine(ExecuteWithCoroutine(lane, data, waitTimeBetweenActions / 2));
    }

    private IEnumerator ExecuteWithCoroutine(CharacterLane lane, CardData data, float time)
    {
        yield return new WaitForSeconds(time);
        lane.ExecuteAction(data);
    }

    private void ToNextLevel()
    {
        currentLevel++;
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(0.5f);
        PlayerHand.Instance.Clear();
        playerLane.getPlayer().Deck.Shuffle();
        for (int i = 0; i < 4; i++)
        {
            var playerActions = playerLane.GetTurnActions();
            ShowPlayerHand(playerActions);
        }

        int index = 0;
        foreach (var i in levels[currentLevel].character)
        {
            if (!levels[currentLevel].character[index])
            {
                break;
            }

            enemyLane.AddCharacter(i, index);
            index++;
        }

        SetupNextTurn();
    }

    IEnumerator GameOver()
    {
        gameOver.SetActive(true);
        // spawn game over text
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("Menu");
    }
}