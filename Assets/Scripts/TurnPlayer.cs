using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnPlayer : MonoBehaviour
{
    public CharacterLane enemiesLane, playersLane;

    public Button nextTurnButton;

    private void Start()
    {
        TimelineHandler.Instance.OnCardAdded += OnCardAdded;
        TimelineHandler.Instance.OnCardRemoved += OnCardRemoved;
    }

    private void OnCardAdded()
    {
        nextTurnButton.enabled = true;
    }

    private void OnCardRemoved()
    {
        nextTurnButton.enabled = false;
    }

    public void SelectActions()
    {

    }

    public void Next()
    {
        var allActions = TimelineHandler.Instance.GetActions();
        foreach (var action in allActions)
        {
            if(!action.Source.IsPlayer)
                action.Source.RemoveAllCondition();

            CharacterLane targetedLane = action.Target == TargetType.Player ? playersLane : enemiesLane;

            targetedLane.Hit(action.Data.damage, action.Data.condition);

            if (action.Data.shield > 0)
            {
                action.Source.AddShield(action.Data.shield);
            }

            TimelineHandler.Instance.removeTopCard();
        }
    }
}
