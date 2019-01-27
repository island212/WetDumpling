using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public static PlayerHand Instance { get; private set; }
    public float cardScale;

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

    public int GetNumberOfCards()
    {
        return transform.childCount;
    }

    public bool AddCardFromBoard(GameObject card)
    {
        if (GetNumberOfCards() > maxCards)
        {
            return false;
        }

        card.transform.SetParent(transform);
        card.transform.localScale = new Vector3(cardScale * 1.5f, cardScale * 1.5f, 0);
        return true;
    }

    public bool AddCard(GameObject card)
    {
        if (GetNumberOfCards() > maxCards)
        {
            return false;
        }

        card.transform.SetParent(transform);
        card.transform.localScale = new Vector3(cardScale, cardScale, 0);
        return true;
    }
}
