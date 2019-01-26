using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPlayer
{
    private struct CharacterCondition
    {
        public HealthCondition condition;
        public CharacterComponent source;

        public CharacterCondition(CardAction action)
        {
            condition = action.Data.condition;
            source = action.Source;
        }
    }

    private TimelineHandler timeline;
    private CharacterLine enemiesLine, playersLine;

    public TurnPlayer(TimelineHandler timeline, CharacterLine enemiesLine, CharacterLine playersLine)
    {
        this.timeline = timeline;
        this.playersLine = playersLine;
        this.enemiesLine = enemiesLine;
    }

    public void Next()
    {
        CardAction[] allActions = timeline.getActions();
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
