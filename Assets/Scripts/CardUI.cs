﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IPointerClickHandler
{
    public CardAction Action { get; set; }

    public bool draggable = false;
    public GameObject selectLink = null;

    private static float scaleMultiplier = 1.5f;
    private float currentScale = 1.0f;
    private Vector3 oldPos;
    private Transform oldParent;
    private int oldChildIndex;
    private bool inserted = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!draggable)
          return;

        oldPos = transform.position;
        oldParent = transform.parent;
        oldChildIndex = transform.GetSiblingIndex();
        transform.SetParent(transform.parent.transform.parent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!draggable)
          return;

        transform.position = Input.mousePosition;

        //var hits = new List<RaycastResult>();
        //EventSystem.current.RaycastAll(eventData, hits);
        //foreach (var raycastResult in hits)
        //{
        //    if (raycastResult.gameObject.tag == "TimelinePanel")
        //    {

        //    }
        //}
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!draggable)
          return;

        var hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, hits);
        foreach (var raycastResult in hits)
        {
            if (raycastResult.gameObject.CompareTag("TimelinePanel"))
            {
                TimelineHandler.Instance.AddCardFromBoard(gameObject);
                inserted = true;
            }
            else if (raycastResult.gameObject.CompareTag("PlayerhandPanel"))
            {
                PlayerHand.Instance.AddCardFromBoard(gameObject);
                inserted = true;
            }
        }

        if (!inserted)
        {
            transform.SetParent(oldParent);
            transform.SetSiblingIndex(oldChildIndex);
            iTween.MoveTo(gameObject, oldPos, 1.0f);
        }
        else
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
        }
        inserted = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!draggable)
          return;

        transform.localScale = transform.localScale * scaleMultiplier;
        currentScale *= scaleMultiplier;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!draggable)
          return;

        transform.localScale = transform.localScale / currentScale;
        currentScale = 1.0f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (selectLink)
        {
            selectLink.GetComponent<SelectCard>().setPicked(gameObject);
        }
    }
}
