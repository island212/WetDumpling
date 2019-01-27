using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using UnityEditor;

public class CharacterLane : MonoBehaviour
{
    [SerializeField]
    private List<CharacterComponent> characters = null;

    private void Awake()
    {
        characters.AddRange(transform.GetComponentsInChildren<CharacterComponent>());
    }

    public IEnumerable<CardAction> GetTurnActions()
    {
        var cardActions = new List<CardAction>();
        foreach (var character in characters)
        {
            bool isPlayer = character.IsPlayer;
            var cards = isPlayer ? GetPlayerCards(character): GetEnemyCards(character);
            cardActions.AddRange(
                cards.Select(card => 
                    new CardAction
                    {
                        Data = card, 
                        Source = character
                    }
                )
            );
        }

        return cardActions.Shuffle();
    }

    private static IList<CardActionData> GetPlayerCards(CharacterComponent player) => 
        player.Deck.GetCards(player.characterData.speed, true);

    private static IList<CardActionData> GetEnemyCards(CharacterComponent enemy) =>
        enemy.Deck
            .Shuffle()
            .GetCards(enemy.characterData.speed, false);

    public bool CanTakeActions()
    {
        return true;
    }

    public void RemoveAllConditions()
    {
        foreach (var character in characters)
        {
            character.RemoveAllCondition();
        }
    }

    public void ExecuteAction(CardData data)
    {
        if (!CanTakeActions()) 
            return;

        characters[0].ExecuteAction(data);
    }

    // Check for level or game over
    public bool IsGameOver()
    {
        UpdateLaneState();
        return characters.Count <= 0;
    }

    void UpdateLaneState()
    {
        var charsToRemove = new List<CharacterComponent>();
        foreach (var character in characters)
        {
            if (character.Health <= 0)
            {
                charsToRemove.Add(character);
            }
        }

        foreach (var character in charsToRemove)
        {
            characters.Remove(character);
            Destroy(character.gameObject);
        }
    }

    public void addCharacter(GameObject character)
    {
        // does not spawn at right position yet
        GameObject newCharacter = Instantiate(character, transform);
        characters.Add(newCharacter.GetComponent<CharacterComponent>());
    }
}
