using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Flags]
public enum HealthCondition { None = 0, Stunned = 1 << 1, Weakened = 1 << 2 }

public class CharacterComponent : MonoBehaviour
{
    public CharacterData characterData;

    public LifeValue LifeValue;

    public int Health { get; private set; }
    public int Shield { get; private set; }

    public Deck Deck { get; private set; }

    private int status;
    public HealthCondition Status => (HealthCondition)status;

    public bool IsPlayer => characterData.isPlayer;

    private void Awake()
    {
        Health = characterData.baseHealth;
        Shield = characterData.baseShield;
        Deck = new Deck(characterData.actions);
    }

    public CardData ChooseAction()
    {
        return characterData.actions[Random.Range(0, characterData.actions.Length)];
    }

    public void ExecuteAction(CardData action)
    {
        var condition = action.condition;
        if (condition != HealthCondition.None)
        {
            AddCondition(condition);
        }

        AddShield(action.shield);

        var damage = action.damage;
        if (damage > 0)
        {
            Attack(damage);
        }

        var healing = action.heal;
        if (healing > 0)
        {
            Heal(healing);
        }

        LifeValue.SetLife(Health);
        LifeValue.SetShield(Shield);
    }

    public void Attack(int damage)
    {   
        if(damage != 0)
            Debug.Log($"{gameObject.name} has received {damage} damage");

        Shield -= damage;
        if (Shield < 0)
        {
            Health += Shield;
            Shield = 0;

            if (Health <= 0)
                Die();
        }
    }

    public void Heal(int amount)
    {
        Debug.Log($"{gameObject.name} has healed {amount}");
        Health += amount;
    }

    public void AddShield(int shield)
    {
        if(shield != 0)
            Debug.Log($"{gameObject.name} has received {shield} shields");
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
