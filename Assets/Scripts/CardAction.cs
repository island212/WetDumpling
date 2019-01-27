using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum TargetType { Player, Enemy }

public class CardAction
{
    public CharacterComponent Source { get; set; }
    public CardActionData Data { get; set; }
}
