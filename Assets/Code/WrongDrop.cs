using UnityEngine;
using UnityEngine.EventSystems;

public class WrongDrop : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        PlaceBack(eventData);
    }

    public void PlaceBack(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            ItemGameObject itemGameObject = eventData.pointerDrag.GetComponent<ItemGameObject>();
            itemGameObject.dropped = true;
            itemGameObject.InvokeOnWrongDrop(itemGameObject);
        }
    }
}
