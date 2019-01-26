using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Character", order = 2)]
public class CharacterData : ScriptableObject
{
    public bool isPlayer;
    public int maxHealth;
    public CardData[] actions;
}
