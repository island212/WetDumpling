using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using UnityEditor;

public class CharacterLane : MonoBehaviour
{
    [SerializeField] private List<CharacterComponent> characters = null;
    public List<Transform> spawnPositions = null;

    private void Awake()
    {
        Transform[] spawns = transform.GetComponentsInChildren<Transform>().Where(r => r.tag == "Enemy").ToArray();
        if (spawns.Length > 0)
        {
            spawnPositions = new List<Transform>(spawns);
        }

        Transform[] components = transform.GetComponentsInChildren<Transform>().Where(r => r.tag == "Entity").ToArray();
        if (components.Length > 0)
        {
            characters.Add(components[0].GetComponent<CharacterComponent>());
        }
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
        //TODO check if the action is a push and move the character by the specified amount

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
        return characters.Count <= 0; //TODO maybe check if deck is empty
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

    public void AddCharacter(GameObject character, int index)
    {
        // does not spawn at right position yet
        var newCharacter = Instantiate(character, spawnPositions[index]);
        characters.Add(newCharacter.GetComponent<CharacterComponent>());
    }
}