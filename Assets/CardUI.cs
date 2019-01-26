using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour, IDragHandler, IEndDragHandler
{
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
                TimelineHandler.Instance.addCard(gameObject);
            }
            else if (i.gameObject.tag == "PlayerhandPanel")
            {
                PlayerHand.Instance.addCard(gameObject);
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
    }
}
