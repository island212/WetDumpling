using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using UnityEditor;

public class CharacterLane : MonoBehaviour
{
    [SerializeField]
    private CharacterComponent[] characters;
    [SerializeField]
    private GameObject[] enemies;

    private void Awake()
    {
        enemies = Resources.LoadAll<GameObject>("Prefabs/Enemies");
        characters = transform.GetComponentsInChildren<CharacterComponent>();
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
}
