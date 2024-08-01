using UnityEngine.EventSystems;

public class Inventory : Container
{
    private void Start()
    {
        AddSlots(10, 10);
    }
    public override void FillWithItems()
    {
        
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            ItemGameObject itemGameObject = eventData.pointerDrag.GetComponent<ItemGameObject>();
            AddItemOfSize(itemGameObject.gameObject, itemGameObject.item.Size);
            itemGameObject.item.State = 1;
        }
    }
    /* private bool PlaceCheck(Vector2 coord, Vector2 item_size)
    {
        for (var i = (int)coord.y; i < coord.y + item_size.y; i++)
        {
            for (var j = (int)coord.x; j < coord.x + item_size.x; j++)
            {
                if (slots[i * slots_width + j].State == 1)
                    return false;
            }
        }
        return true;
    } */
}
