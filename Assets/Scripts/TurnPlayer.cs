using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnPlayer : MonoBehaviour
{
    public TimelineHandler timeline;
    public CharacterLine enemiesLine, playersLine;

    public Button nextTurnButton;

    private void Start()
    {
        timeline.OnCardAdded += OnCardAdded;
        timeline.OnCardRemoved += OnCardRemoved;
    }

    private void OnCardAdded()
    {
        nextTurnButton.enabled = true;
    }

    private void OnCardRemoved()
    {
        nextTurnButton.enabled = false;
    }

    public void Next()
    {
        CardAction[] allActions = timeline.GetActions();
        foreach (var action in allActions)
        {
            if(!action.Source.IsPlayer)
                action.Source.RemoveAllCondition();

            CharacterLine targetedLine = action.Target == TargetType.Player ? playersLine : enemiesLine;

            targetedLine.Hit(action.Data.damage, action.Data.condition);

            if (action.Data.shield > 0)
            {
                action.Source.AddShield(action.Data.shield);
            }
        }
    }
}
