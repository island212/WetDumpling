﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using UnityEditor;

public class CharacterLane : MonoBehaviour
{
    [SerializeField] private List<CharacterComponent> characters = null;

    private void Awake()
    {
        var components = transform.GetComponentsInChildren<CharacterComponent>();
        characters.AddRange(components);
    }

    public IEnumerable<CardAction> GetTurnActions()
    {
        var cardActions = new List<CardAction>();
        foreach (var character in characters)
        {
            bool isPlayer = character.IsPlayer;
            var cards = isPlayer ? GetPlayerCards(character) : GetEnemyCards(character);

            cardActions.AddRange(cards.Select(cardActionData => new CardAction {Data = cardActionData, Source = character}));
        }

        return cardActions.Shuffle();
    }

    private static IList<CardActionData> GetPlayerCards(CharacterComponent player) =>
        player.Deck.GetCards(player.characterData.speed, true);

    private static IList<CardActionData> GetEnemyCards(CharacterComponent enemy) =>
        enemy.Deck
             .Shuffle()
             .GetCards(enemy.characterData.speed, false);

    public void ExecuteAction(CardData data)
    {
        int pushIndex = data.push;

        if (pushIndex != 0)
        {
            var charToPush = characters[0];
            var nextChar = characters[pushIndex];
            characters[pushIndex] = charToPush;
            characters[0] = nextChar;
        }

        characters[0].ExecuteAction(data);
    }

    public bool IsGameOver()
    {
        UpdateLaneState();
        return characters.Count <= 0;
    }

    private void UpdateLaneState()
    {
        var charsToRemove = new List<CharacterComponent>();

        foreach (var character in characters)
        {
            if (character.IsDead) 
                charsToRemove.Add(character);
        }

        foreach (var character in charsToRemove)
        {
            characters.Remove(character);
            Destroy(character.gameObject);
        }
    }

    public void AddCharacter(GameObject character)
    {
        // does not spawn at right position yet
        var newCharacter = Instantiate(character, transform);
        characters.Add(newCharacter.GetComponent<CharacterComponent>());
    }
}