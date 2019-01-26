using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TimelineHandler : MonoBehaviour
{
    public Action OnCardAdded { get; set; }
    public Action OnCardRemoved { get; set; }

    public static TimelineHandler Instance { get; private set; }

    public float cardScale;
    public GameObject cardPefab;

    [SerializeField]
    private int maxCards;
    private int cardCount = 0;
    private GameObject ghostCardHolder = null;

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

    public IEnumerable<CardAction> GetActions()
    {
        return transform.GetComponentsInChildren<CardUI>().Select(x => x.Action);
    }

    public bool AddCard(CardAction action)
    {
        if (getNumberOfCards() > maxCards)
        {
            return false;
        }

        GameObject card = Instantiate(cardPefab, transform);
        card.GetComponent<CardUI>().Action = action;
        return true;
    }

    public bool addCardFromBoard(GameObject card)
    {
        if (getNumberOfCards() > maxCards)
        {
            return false;
        }

        Transform closestCard = null;
        float smallestDistance = Mathf.Infinity;
        float currentDistance;

        foreach (Transform i in transform)
        {
            currentDistance = Mathf.Abs((i.position - card.transform.position).magnitude);
            if (i != card.transform && ghostCardHolder != i && currentDistance < smallestDistance)
            {
                closestCard = i;
                smallestDistance = currentDistance;
            }
        }

        card.transform.SetParent(transform);

        card.transform.SetSiblingIndex(closestCard.transform.GetSiblingIndex());

        card.transform.localScale = new Vector3(cardScale * 1.5f, cardScale * 1.5f, 0);

        cardCount++;

        OnCardAdded?.Invoke();

        return true;
    }

    public bool removeTopCard()
    {
        if (getNumberOfCards() <= 0)
        {
            return false;
        }

        Transform[] childrens = GetComponentsInChildren<Transform>();
        Destroy(childrens[1].gameObject);

        OnCardRemoved?.Invoke();

        return true;
    }
}
