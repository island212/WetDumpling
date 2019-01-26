using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CardAction Action { get; set; }

    private static float scaleMultiplier = 1.5f;
    private float currentScale = 1.0f;

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
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
            }
            else if (i.gameObject.tag == "PlayerhandPanel")
            {
                PlayerHand.Instance.addCardFromBoard(gameObject);
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
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
