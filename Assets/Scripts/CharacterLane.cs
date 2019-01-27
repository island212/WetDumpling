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
                        Source = character, 
                        Target = isPlayer ? TargetType.Enemy : TargetType.Player
                    }
                )
            );
        }

        return cardActions.Shuffle();
    }

    private static IList<CardData> GetPlayerCards(CharacterComponent player) => 
        player.Deck.GetCards(player.characterData.speed, true);

    private static IList<CardData> GetEnemyCards(CharacterComponent enemy) =>
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
    public bool updateGameState()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].Health <= 0)
            {
                CharacterComponent tempChar = characters[i];
                characters.Remove(tempChar);
                Destroy(tempChar);
            }
        }

        if (characters.Count <= 0) {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void addCharacter(GameObject character)
    {
        // does not spawn at right position yet
        GameObject newCharacter = Instantiate(character, transform);
        characters.Add(newCharacter.GetComponent<CharacterComponent>());
    }
}
