using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler
{
    public CardAction Action { get; set; }

    public bool draggable = false;

    private static float scaleMultiplier = 1.5f;
    private float currentScale = 1.0f;
    private Vector3 oldPos;
    private Transform oldParent;
    private int oldChildIndex;
    private bool inserted = false;

    struct Movement
    {
        Vector3 oldPos;
        Vector3 newPos;
    }

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

        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, hits);
        foreach (RaycastResult i in hits)
        {
            if (i.gameObject.tag == "TimelinePanel")
            {

            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!draggable)
          return;

        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, hits);
        foreach (RaycastResult i in hits)
        {
            if (i.gameObject.tag == "TimelinePanel")
            {
                TimelineHandler.Instance.AddCardFromBoard(gameObject);
                inserted = true;
            }
            else if (i.gameObject.tag == "PlayerhandPanel")
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
}
