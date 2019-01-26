using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLine : MonoBehaviour
{
    [SerializeField]
    private CharacterComponent[] characters;

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
            var cards = character.Deck.GetCards(character.characterData.speed);
            foreach (var card in cards)
            {
                var action = new CardAction
                {
                    Data = card, 
                    Source = character, 
                    Target = TargetType.Player
                };

                cardActions.Add(action);
            }
        }

        return cardActions;
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
