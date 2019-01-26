using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Flags]
public enum HealthCondition { None = 0, Stun = 1 << 1, Weaken = 1 << 2 }

public class CharacterComponent : MonoBehaviour
{
    public CharacterData characterData;

    public int Health { get; private set; }
    public int Shield { get; private set; }

    public Deck Deck { get; private set; }

    private int status;
    public HealthCondition Status => (HealthCondition)status;

    public bool IsPlayer => characterData.isPlayer;

    private void Awake()
    {
        Health = characterData.maxHealth;
        Deck = new Deck(characterData.actions);
    }

    public CardData ChooseAction()
    {
        return characterData.actions[Random.Range(0, characterData.actions.Length)];
    }

    public void Damage(int damage)
    {
        Debug.Log(gameObject.name + " has received " + damage);
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
        Debug.Log(gameObject.name + " has healed " + heal);
        Health += heal;
        if (Health > characterData.maxHealth)
            Health = characterData.maxHealth;
    }

    public void AddShield(int shield)
    {
        Debug.Log(gameObject.name + " has received " + shield + " shields");
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
