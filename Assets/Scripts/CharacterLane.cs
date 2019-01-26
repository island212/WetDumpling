using System.Collections.Generic;
using UnityEngine;
using Utils;

public class CharacterLane : MonoBehaviour
{
    [SerializeField]
    private CharacterComponent[] characters;

    private void Awake()
    {
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
            var cards = character.Deck.GetCards(character.characterData.speed, character.IsPlayer);
            foreach (var card in cards)
            {
                cardActions.Add(new CardAction
                {
                    Data = card, 
                    Source = character, 
                    Target = character.IsPlayer ? TargetType.Player : TargetType.Enemy
                });
            }
        }

        return cardActions.Shuffle();
    }

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
