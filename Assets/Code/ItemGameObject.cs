using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemGameObject : MonoBehaviour, IDraggable, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Item item;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    public event Action<ItemGameObject> OnDragging;
    public Vector2 start_anchor;

    public void Create()
    {
        item = new();
        rectTransform = GetComponent<RectTransform>();
        ChangeImage();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Create(string item_name, float state, string sprite_name, Vector2 size)
    {
        item = new(item_name, state, sprite_name, size);
        rectTransform = GetComponent<RectTransform>();
        ChangeImage();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void ChangeImage()
    {
        GetComponent<Image>().sprite = item.Sprite;
        rectTransform.sizeDelta *= item.Size;
    }
    
    public void MoveInSlot(Vector2 new_position)
    {
        rectTransform.anchoredPosition = new_position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.5f;
        start_anchor = rectTransform.anchoredPosition;
        OnDragging?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }
}