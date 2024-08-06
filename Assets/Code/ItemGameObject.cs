using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemGameObject : MonoBehaviour, IDraggable
{
    public Item item;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private RectTransform item_transform, canvas_transform;
    public event Action<ItemGameObject> OnDragging, OnWrongDrop;
    public Vector2 start_position;
    private WrongDrop wrongDrop;
    public bool dropped = false;

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
        canvas = transform.parent.GetComponent<Canvas>();
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
        start_position = new_position;
        item_transform.anchoredPosition = start_position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        item_transform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.5f;
        OnDragging?.Invoke(this);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        
        if (!dropped && (eventData != null) &&
            ((item_transform.anchoredPosition.x <= canvas_transform.anchoredPosition.x) ||
            (item_transform.anchoredPosition.x + item_transform.sizeDelta.x >= canvas_transform.anchoredPosition.x + canvas_transform.sizeDelta.x) ||
            (item_transform.anchoredPosition.y + item_transform.sizeDelta.y >= canvas_transform.anchoredPosition.y) ||
            (item_transform.anchoredPosition.y <= canvas_transform.anchoredPosition.y - canvas_transform.sizeDelta.y)))
        {
            wrongDrop.PlaceBack(eventData);
        }
        dropped = false;
    }

    public void InvokeOnWrongDrop(ItemGameObject itemGameObject)
    {
        itemGameObject.OnWrongDrop?.Invoke(itemGameObject);
    }
}