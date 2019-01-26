using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class TimelineHandler : MonoBehaviour
{
    public static TimelineHandler Instance { get; private set; }

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

    public bool addCard(GameObject card)
    {
        if (getNumberOfCards() > maxCards)
        {
            return false;
        }

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

        card.transform.parent = transform;

        if (Mathf.Sign(closestCard.transform.position.x - card.transform.position.x) == 1)
        {
            card.transform.SetSiblingIndex(closestCard.transform.GetSiblingIndex() + 1);
        }
        else
        {
            card.transform.SetSiblingIndex(closestCard.transform.GetSiblingIndex() - 1);
        }

        card.transform.localScale = new Vector3(0.3f, 0.3f, 0);

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
