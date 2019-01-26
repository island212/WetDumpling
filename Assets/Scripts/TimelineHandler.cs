using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineHandler : MonoBehaviour
{
    [SerializeField]
    private int maxCards;

    void Start()
    {
        removeTopCard();    
    }

    public int getNumberOfCards()
    {
        return transform.childCount;
    }

    public CardAction[] getActions()
    {
        return transform.GetComponentsInChildren<CardAction>();
    }

    public bool addCard(GameObject card)
    {
        if (getNumberOfCards() > maxCards)
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

        return true;
    }
}
