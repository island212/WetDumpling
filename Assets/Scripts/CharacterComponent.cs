using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum HealthCondition { None = 0, Stun = 1 << 1, Weaken = 1 << 2 }

public class CharacterComponent : MonoBehaviour
{
    public CharacterData characterData;

    public int Health { get; private set; }
    public int Shield { get; private set; }

    private int status;
    public HealthCondition Status
    {
        get { return (HealthCondition)status; }
    }

    public bool IsPlayer { get { return characterData.isPlayer; } }

    private void Awake()
    {
        Health = characterData.maxHealth;
    }

    public CardData ChooseAction()
    {
        return characterData.actions[Random.Range(0, characterData.actions.Length)];
    }

    public void Damage(int damage)
    {
        Shield -= damage;
        if (Shield < 0)
        {
            Health += Shield;
            Shield = 0;

            if (Health <= 0)
                Die();
        }
    }

    public void Heal(int heal)
    {
        Health += heal;
        if (Health > characterData.maxHealth)
            Health = characterData.maxHealth;
    }

    public void AddShield(int shield)
    {
        Shield += shield;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void AddCondition(HealthCondition condition)
    {
        status |= (int)condition;
    }

    public void RemoveCondition(HealthCondition condition)
    {
        status |= (int)condition;
        status ^= (int)condition;
    }

    public void RemoveAllCondition()
    {
        status = 0;
    }
}
