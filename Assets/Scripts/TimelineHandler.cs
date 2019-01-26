using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineHandler : MonoBehaviour
{
    public static TimelineHandler Instance { get; private set; }
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

    public int getNumberOfCards()
    {
        return transform.childCount;
    }

    public CardAction[] getActions()
    {
        return transform.GetComponentsInChildren<CardAction>();
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
            if (i != card.transform && currentDistance < smallestDistance)
            {
                closestCard = i;
                smallestDistance = currentDistance;
            }
        }

        card.transform.SetParent(transform);

        card.transform.SetSiblingIndex(closestCard.transform.GetSiblingIndex());

        card.transform.localScale = new Vector3(cardScale * 1.5f, cardScale * 1.5f, 0);

        return true;
    }

    public bool addEnemyMove(GameObject move)
    {
        move.transform.SetParent(transform);
        move.transform.localScale = new Vector3(cardScale, cardScale, 0);

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
