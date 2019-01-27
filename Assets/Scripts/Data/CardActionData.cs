using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/CardAction", order = 2)]
public class CardActionData : ScriptableObject
{
    public CardData targetData;
    public CardData selfData;
}
