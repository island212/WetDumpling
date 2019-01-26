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
        foreach (var turnAction in playerActions)
        {
            Debug.Log(turnAction.Source.name);
        }

        var enemyActions = enemyLane.GetTurnActions();
        foreach (var turnAction in enemyActions)
        {
            Debug.Log($"{turnAction.Source.name} {turnAction.Data.name}");
        }
    }

    void Update()
    {
        
    }
}
