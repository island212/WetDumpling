using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeValue : MonoBehaviour
{
    private int currentLife;
    private Text lifeText;

    void Start()
    {
        currentLife = transform.parent.GetComponent<CharacterComponent>().characterData.maxHealth;
        lifeText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        lifeText.text = currentLife.ToString(); ;
    }

    public void SetLife(int life)
    {
        currentLife = life;
        lifeText.text = currentLife.ToString();
    }
}
