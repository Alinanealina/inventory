using UnityEngine;
using UnityEngine.EventSystems;

public class ItemsContainer : Container
{
    private void Start()
    {
        AddSlots(10, 5);
        FillWithItems();
    }
    public override void FillWithItems()
    {
        System.Random random = new();
        int count = random.Next(2, 10);
        for (var i = 0; i < count; i++)
        {
            AddItemOfSize(null, new Vector2(random.Next(1, 5), random.Next(1, 5)));
        }
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            ItemGameObject itemGameObject = eventData.pointerDrag.GetComponent<ItemGameObject>();
            AddItemOfSize(itemGameObject.gameObject, itemGameObject.item.Size);
            itemGameObject.item.State = 0;
        }
    }
}
