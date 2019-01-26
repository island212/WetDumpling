using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public static PlayerHand Instance { get; private set; }

    [SerializeField]
    private int maxCards;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int getNumberOfCards()
    {
        return transform.childCount;
    }

    public bool addCard(GameObject card)
    {
        if (getNumberOfCards() > maxCards)
        {
            return false;
        }

        card.transform.parent = transform;
        card.transform.localScale = new Vector3(1f, 1f, 0);
        return true;
    }
}
