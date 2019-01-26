using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLine : MonoBehaviour
{
    private CharacterComponent[] characters;

    public void Hit(int damage, HealthCondition condition)
    {
        if(damage > 0)
            characters[0].Damage(damage);
        if(condition != HealthCondition.None)
            characters[0].AddCondition(condition);
    }

    public void RemoveAllConditions()
    {
        foreach (var character in characters)
        {
            character.RemoveAllCondition();
        }
    }

    public bool CanTakeActions()
    {
        foreach (var character in characters)
        {
            if (!character.Status.HasFlag(HealthCondition.Stun))
                return false;
        }
        return true;
    }
}
