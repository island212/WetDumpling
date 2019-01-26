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
        List<CardAction> cardActions = new List<CardAction>();
        foreach (var character in characters)
        {
            foreach (var card in character.Deck.GetCards(character.characterData.speed))
            {
                CardAction action = new CardAction();
                action.Data = card;
                action.Source = character;
                action.Target = TargetType.Player;

                cardActions.Add(action);
            }
        }


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
