using UnityEngine;
using UnityEngine.EventSystems;

public class SlotObject : MonoBehaviour, IDropHandler
{
    public Slot slot = new();
    private Container parent_container;

    private void Start()
    {
        parent_container = transform.parent.GetComponent<Container>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            ItemGameObject itemGameObject = eventData.pointerDrag.GetComponent<ItemGameObject>();
            itemGameObject.dropped = true;
            parent_container.ItemDropped(itemGameObject);
        }
    }
}
