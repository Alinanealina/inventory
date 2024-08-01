using UnityEngine.EventSystems;

public interface IDraggable
{
    void OnBeginDrag(PointerEventData eventData);
    void OnEndDrag(PointerEventData eventData);
    void OnDrag(PointerEventData eventData);
}