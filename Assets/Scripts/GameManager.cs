using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CharacterLane playerLane;
    public CharacterLane enemyLane;

    void Start()
    {
        var playerActions = playerLane.GetTurnActions();
        LogActions(playerActions);

        var enemyActions = enemyLane.GetTurnActions();
        LogActions(enemyActions);
    }

    void LogActions(IEnumerable<CardAction> actions)
    {
        foreach (var turnAction in actions)
        {
            Debug.Log($"{turnAction.Source.name} {turnAction.Data.name}");
        }
    }

    void Update()
    {
        
    }
}
