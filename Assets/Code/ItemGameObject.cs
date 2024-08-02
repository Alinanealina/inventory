using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemGameObject : MonoBehaviour, IDraggable, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Item item;
    private CanvasGroup canvasGroup;
    private RectTransform item_transform, canvas_transform;
    public event Action<ItemGameObject> OnDragging, OnWrongDrop;
    public Vector2 start_anchor;
    private WrongDrop wrongDrop;

    public void Create()
    {
        item = new();
        Initialization();
    }
    public void Create(string item_name, float state, string sprite_name, Vector2 size)
    {
        item = new(item_name, state, sprite_name, size);
        Initialization();
    }
    private void Initialization()
    {
        item_transform = GetComponent<RectTransform>();
        wrongDrop = GetComponent<WrongDrop>();
        canvas_transform = transform.parent.GetComponent<RectTransform>();
        ChangeImage();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void ChangeImage()
    {
        GetComponent<Image>().sprite = item.Sprite;
        item_transform.sizeDelta *= item.Size;
    }
    
    public void MoveInSlot(Vector2 new_position)
    {
        item_transform.anchoredPosition = new_position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        item_transform.anchoredPosition += eventData.delta;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.5f;
        start_anchor = item_transform.anchoredPosition;
        OnDragging?.Invoke(this);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        
        if ((eventData == null) ||
            (item_transform.anchoredPosition.x <= 0) ||
            (item_transform.anchoredPosition.x >= canvas_transform.anchoredPosition.x) ||
            (item_transform.anchoredPosition.y <= 0) ||
            (item_transform.anchoredPosition.y >= canvas_transform.anchoredPosition.y))
        {
            wrongDrop.PlaceBack(eventData);
        }
    }

    public void InvokeOnWrongDrop(ItemGameObject itemGameObject)
    {
        itemGameObject.OnWrongDrop?.Invoke(itemGameObject);
    }
}