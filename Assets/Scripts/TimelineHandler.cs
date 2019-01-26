using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineHandler : MonoBehaviour
{
    public Action OnCardAdded { get; set; }
    public Action OnCardRemoved { get; set; }

    [SerializeField]
    private int maxCards;
    private int cardCount = 0;

    public int GetNumberOfCards()
    {
        return transform.childCount;
    }

    public CardAction[] GetActions()
    {
        return transform.GetComponentsInChildren<CardAction>();
    }

    public bool AddCard(GameObject card)
    {
        if (GetNumberOfCards() > maxCards)
        {
            return false;
        }

        GameObject newCard = Instantiate(card, transform);
        Transform[] childrens = GetComponentsInChildren<Transform>();
        Transform closestCard = null;
        float smallestDistance = Mathf.Infinity;
        float currentDistance;

        foreach (Transform i in childrens)
        {
            currentDistance = (i.position - card.transform.position).magnitude;
            if (currentDistance < smallestDistance)
            {
                closestCard = i;
                smallestDistance = currentDistance;
            }
        }

        if (Mathf.Sign(closestCard.transform.position.x - card.transform.position.x) == 1)
        {
            newCard.transform.SetSiblingIndex(closestCard.transform.GetSiblingIndex() + 1);
        }
        else
        {
            newCard.transform.SetSiblingIndex(closestCard.transform.GetSiblingIndex() - 1);
        }

        cardCount++;

        OnCardAdded?.Invoke();

        return true;
    }

    public bool RemoveTopCard()
    {
        if (GetNumberOfCards() <= 0)
        {
            return false;
        }

        Transform[] childrens = GetComponentsInChildren<Transform>();
        Destroy(childrens[1].gameObject);

        OnCardRemoved?.Invoke();

        return true;
    }
}
