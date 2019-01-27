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
        enemies = Resources.LoadAll("Assets/Prefabs/Enemies") as GameObject[];
        characters = transform.GetComponentsInChildren<CharacterComponent>();
    }

    public void Hit(int damage, HealthCondition condition)
    {
        if(damage > 0)
            characters[0].Damage(damage);
        if(condition != HealthCondition.None)
            characters[0].AddCondition(condition);
    }

    public IEnumerable<CardAction> GetTurnActions()
    {
        var cardActions = new List<CardAction>();
        foreach (var character in characters)
        {
            bool isPlayer = character.IsPlayer;
            //Debug.Log($"{character.Deck.Count()} {character.characterData.speed}");
            var cards = isPlayer ? GetPlayerCards(character): GetEnemyCards(character);
            //Debug.Log(character.Deck.Count());
            cardActions.AddRange(
                cards.Select(card => 
                    new CardAction
                    {
                        Data = card, 
                        Source = character, 
                        Target = isPlayer ? TargetType.Player : TargetType.Enemy
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

    public void RemoveAllConditions()
    {
        foreach (var character in characters)
        {
            character.RemoveAllCondition();
        }
    }

    public bool CanTakeActions()
    {
        foreach (var character in characters)
        {
            if (!character.Status.HasFlag(HealthCondition.Stun))
                return false;
        }
        return true;
    }
}
