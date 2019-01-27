using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeValue : MonoBehaviour
{
    public Text lifeText;
    public Text shieldText;
    public GameObject shieldSprite;

    private int currentLife;
    private int currentShield;

    void Start()
    {
        currentLife = transform.parent.GetComponent<CharacterComponent>().characterData.baseHealth;
        lifeText.text = currentLife.ToString();

        currentShield = transform.parent.GetComponent<CharacterComponent>().characterData.baseShield;
        shieldText.text = currentShield.ToString();
        UpdateVisible();
    }

    public void SetLife(int life)
    {
        currentLife = life;
        lifeText.text = currentLife.ToString();
    }

    public void SetShield(int shield) 
    {
        currentShield += shield;
        shieldText.text = currentShield.ToString();

        UpdateVisible();
    }

    private void UpdateVisible() 
    {
        shieldText.enabled = currentShield > 0;
        shieldSprite.SetActive(currentShield > 0);
    }
}
