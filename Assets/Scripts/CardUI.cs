using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler
{
    public CardAction Action { get; set; }

    private static float scaleMultiplier = 1.5f;
    private float currentScale = 1.0f;
    private Vector3 oldPos;
    private Transform oldParent;
    private int oldChildIndex;
    private bool inserted = false;

    struct movement
    {
        Vector3 oldPos;
        Vector3 newPos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        oldPos = transform.position;
        oldParent = transform.parent;
        oldChildIndex = transform.GetSiblingIndex();
        transform.SetParent(transform.parent.transform.parent);
    }

    public void OnDrag(PointerEventData eventData)
    {
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
        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, hits);
        foreach (RaycastResult i in hits)
        {
            if (i.gameObject.tag == "TimelinePanel")
            {
                TimelineHandler.Instance.addCardFromBoard(gameObject);
                inserted = true;
            }
            else if (i.gameObject.tag == "PlayerhandPanel")
            {
                PlayerHand.Instance.addCardFromBoard(gameObject);
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
        transform.localScale = transform.localScale * scaleMultiplier;
        currentScale *= scaleMultiplier;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = transform.localScale / currentScale;
        currentScale = 1.0f;
    }
}
