using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Card", order = 1)]
public class CardData : ScriptableObject
{
    public int damage = 0;
    public int defense = 0;
}
