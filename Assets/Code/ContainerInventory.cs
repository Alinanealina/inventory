using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : Container
{
    private DataSaverInventory dataSaverInventory;

    private void OnDestroy()
    {
        dataSaverInventory.SaveState();
    }
    private void Start()
    {
        AddSlots(10, 10);
        dataSaverInventory = GetComponent<DataSaverInventory>();
        FillWithItems();
    }
    public override void FillWithItems()
    {
        dataSaverInventory.LoadState();
        for (var i = 0; i < slot_column; i++)
        {
            for (var j = 0; j < slot_row; j++)
            {
                if (slots[i * slot_row + j].slot.Item_name != "")
                {
                    string[] strings = slots[i * slot_row + j].slot.Item_name.Split('x');
                    Vector2 item_size = new Vector2(int.Parse(strings[0]), int.Parse(strings[1]));
                    PlaceItemAtCoordinates(null, item_size, new Vector2(j, i));
                }
            }
        }
    }

    private void PlaceItemAtCoordinates(GameObject item, Vector2 item_size, Vector2 coord)
    {
        item = CreateItem(item_size);
        ItemGameObject itemGameObject = item.GetComponent<ItemGameObject>();
        itemGameObject.OnDragging += ItemRemoved;
        
        itemGameObject.MoveInSlot(anchor_panel + (cellSize + grid_spacing) * coord);
    }
    
    public override void ItemRemoved(ItemGameObject itemGameObject)
    {
        itemGameObject.OnDragging -= ItemRemoved;
        Vector2 coord = new Vector2(
            (float)Math.Floor((itemGameObject.start_anchor - anchor_panel).x / (cellSize + grid_spacing).x),
            (float)Math.Floor((itemGameObject.start_anchor - anchor_panel).y / (cellSize + grid_spacing).y)
        );
        SetSlotsState(coord, itemGameObject.item.Size, 0);
        slots[(int)(coord.y * slot_row + coord.x)].slot.Item_name = "";
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
    protected override void AddItemOfSize(GameObject item, Vector2 item_size)
    {
        Vector2 coord = FindEmptySpace(item_size);
        if (coord.x == -1)
            return;
        
        ItemGameObject itemGameObject = item.GetComponent<ItemGameObject>();
        itemGameObject.OnDragging += ItemRemoved;
        
        SetSlotsState(coord, item_size, 1);
        
        itemGameObject.MoveInSlot(anchor_panel + (cellSize + grid_spacing) * coord);
        slots[(int)(coord.y * slot_row + coord.x)].slot.Item_name = itemGameObject.item.Item_name;
    }
}
