using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType { Player, Monster }

public class CardAction
{
    public CharacterComponent Source { get; set; }
    public TargetType Target { get; set; }
    public CardData Data { get; set; }
}
