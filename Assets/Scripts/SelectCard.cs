using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCard : MonoBehaviour
{
    public GameObject c1;
    public GameObject c2;
    public GameObject c3;
    public CardActionData[] cardActions;
    public CharacterLane playerLane;
    private bool hasPicked;
    private GameObject pickedCard;
    public GameObject[] objects;

    private void Awake()
    {
        cardActions = Resources.LoadAll<CardActionData>("Data/Cards/Action");
    }

    private void OnEnable()
    {
        hasPicked = false;
        objects = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            CardAction cardAction = new CardAction();
            cardAction.Data = cardActions[Random.Range(0, cardActions.Length)];
            cardAction.Source = playerLane.getPlayer();
            objects[i] = GameManager.InstantiateCardAction(cardAction, "Player");
            objects[i].transform.SetParent(transform);
        }

        int pos = transform.parent.childCount;
        transform.SetSiblingIndex(pos + 1);

        // instantiate random card on c1
        objects[0].transform.position = c1.transform.position;
        objects[0].transform.localScale = new Vector3(4, 4, 0);
        objects[0].GetComponent<CardUI>().draggable = false;
        objects[0].GetComponent<CardUI>().selectLink = gameObject;
        // instantiate random card on c2
        objects[1].transform.position = c2.transform.position;
        objects[1].transform.localScale = new Vector3(4, 4, 0);
        objects[1].GetComponent<CardUI>().draggable = false;
        objects[1].GetComponent<CardUI>().selectLink = gameObject;
        // instantiate random card on c3
        objects[2].transform.position = c3.transform.position;
        objects[2].transform.localScale = new Vector3(4, 4, 0);
        objects[2].GetComponent<CardUI>().draggable = false;
        objects[2].GetComponent<CardUI>().selectLink = gameObject;
    }

    public bool picked()
    {
        return hasPicked;
    }

    public void setPicked(GameObject card)
    {
        pickedCard = card;
        hasPicked = true;
        for (int i = 0; i < 3; i++)
        {
            Destroy(objects[i]);
        }
    }

    public GameObject returnCard()
    {
        return pickedCard;
    }
}
