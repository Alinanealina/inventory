using System;
using UnityEngine;

public class Inventory : Container
{
    private DataSaverInventory dataSaverInventory;

    private void OnDestroy()
    {
        dataSaverInventory.SaveState();
    }
    
    private void Start()
    {
        dataSaverInventory = GetComponent<DataSaverInventory>();
        AddSlots(10, 10);
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
        itemGameObject.OnWrongDrop += AddBack;

        SetSlotsState(coord, item_size, 1);
        
        itemGameObject.MoveInSlot(anchor_panel + (cellSize + grid_spacing) * coord);
    }
    
    protected override void AddItemOfSize(GameObject item, Vector2 item_size)
    {
        Vector2 coord = FindEmptySpace(item_size);
        ItemGameObject itemGameObject = item.GetComponent<ItemGameObject>();
        if (coord.x == -1)
        {
            itemGameObject.InvokeOnWrongDrop(itemGameObject);
            return;
        }
        
        itemGameObject.OnDragging += ItemRemoved;
        itemGameObject.OnWrongDrop += AddBack;
        
        SetSlotsState(coord, item_size, 1);
        
        itemGameObject.MoveInSlot(anchor_panel + (cellSize + grid_spacing) * coord);
        itemGameObject.item.State = 1;
        slots[(int)(coord.y * slot_row + coord.x)].slot.Item_name = itemGameObject.item.Item_name;
    }
    
    public override void ItemRemoved(ItemGameObject itemGameObject)
    {
        itemGameObject.OnDragging -= ItemRemoved;
        Vector2 coord = new Vector2(
            (float)Math.Floor(((itemGameObject.start_position - anchor_panel) / (cellSize + grid_spacing)).x),
            (float)Math.Floor(((itemGameObject.start_position - anchor_panel) / (cellSize + grid_spacing)).y)
        );
        SetSlotsState(coord, itemGameObject.item.Size, 0);
        slots[(int)(coord.y * slot_row + coord.x)].slot.Item_name = "";
    }

    public override void AddBack(ItemGameObject itemGameObject)
    {
        itemGameObject.OnWrongDrop -= AddBack;
        if (itemGameObject.item.State == 1)
        {
            AddItemOfSize(itemGameObject.gameObject, itemGameObject.item.Size);
        }
    }
}
