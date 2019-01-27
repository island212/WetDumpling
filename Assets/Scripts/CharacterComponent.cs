using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

[System.Flags]
public enum HealthCondition
{
    None = 0,
    Stunned = 1 << 1,
    Weakened = 1 << 2
}

public class CharacterComponent : MonoBehaviour
{
    public CharacterData characterData;

    public LifeValue LifeValue;

    public int Health { get; private set; }
    public int Shield { get; private set; }

    public Deck Deck { get; private set; }

    private int status;
    public HealthCondition Status => (HealthCondition) status;

    public bool IsPlayer => characterData.isPlayer;

    public bool IsDead => Health <= 0 || Deck.IsEmpty;

    private void Awake()
    {
        Health = characterData.baseHealth;
        Shield = characterData.baseShield;
        Deck = new Deck(characterData.actions);
    }

    public void ExecuteAction(CardData action)
    {
        var condition = action.condition;
        if (condition != HealthCondition.None)
            AddCondition(condition);

        int healing = action.heal;
        if (healing != 0)
            Heal(healing);

        int shield = action.shield;
        if(shield != 0)
            AddShield(shield);

        int damage = action.damage;
        if (damage != 0)
            Attack(damage);

        LifeValue.SetLife(Health);
        LifeValue.SetShield(Shield);
    }

    private void Attack(int damage)
    {
        Debug.Log($"{gameObject.name} has received {damage} damage");
        //iTween.ShakeRotation(gameObject, new Vector3(0.2f, 0.2f, 0.0f), 0.5f);

        Shield -= damage;
        if (Shield < 0)
        {
            Health += Shield;
            Shield = 0;
        }
    }

    private void Heal(int amount)
    {
        Debug.Log($"{gameObject.name} has healed {amount}");
        Health += amount;
    }

    private void AddShield(int shield)
    {
        Debug.Log($"{gameObject.name} has received {shield} shields");
        Shield += shield;
    }

    private void AddCondition(HealthCondition condition)
    {
        status |= (int) condition;
    }

    private void RemoveCondition(HealthCondition condition)
    {
        status |= (int) condition;
        status ^= (int) condition;
    }

    public void RemoveAllCondition()
    {
        status = 0;
    }
}