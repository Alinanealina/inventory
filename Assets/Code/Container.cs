using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IContainer
{
    void FillWithItems();
    void ItemRemoved(ItemGameObject itemGameObject);
}

public abstract class Container : MonoBehaviour, IContainer, IDropHandler
{
    public List<SlotObject> slots = new();
    protected int slot_column, slot_row;
    protected Vector2 grid_spacing, anchor_panel, cellSize;
    [SerializeField] private GameObject slot_prefab;
    [SerializeField] private GameObject item_prefab;
    public abstract void FillWithItems();
    public abstract void OnDrop(PointerEventData eventData);

    private void Awake()
    {
        grid_spacing = GetComponent<GridLayoutGroup>().spacing;
        anchor_panel = GetComponent<RectTransform>().anchoredPosition;
        cellSize = GetComponent<GridLayoutGroup>().cellSize;
    }
    
    protected void AddSlots(int slot_column, int slot_row)
    {
        this.slot_column = slot_column;
        this.slot_row = slot_row;
        for (var i = 0; i < slot_column * slot_row; i++)
        {
            if (slots.Count > slot_column * slot_row)
            {
                RemoveSlotAt(0);
            }
            GameObject obj = Instantiate(slot_prefab, transform);
            slots.Add(obj.GetComponent<SlotObject>());
            slots[i].slot.Size = cellSize;
        }
    }
    private void RemoveSlotAt(int i)
    {
        if (slots.Count > 0)
        {
            Destroy(slots[i]);
            slots.RemoveAt(i);
        }
    }

    protected virtual void AddItemOfSize(GameObject item, Vector2 item_size)
    {
        Vector2 coord = FindEmptySpace(item_size);
        if (coord.x == -1)
            return;
        
        if (item == null)
        {
            item = CreateItem(item_size);
        }
        ItemGameObject itemGameObject = item.GetComponent<ItemGameObject>();
        itemGameObject.OnDragging += ItemRemoved;
        
        SetSlotsState(coord, item_size, 1);
        
        itemGameObject.MoveInSlot(anchor_panel + (cellSize + grid_spacing) * coord);
    }
    protected GameObject CreateItem(Vector2 item_size)
    {
        GameObject item = Instantiate(item_prefab, transform.parent);
        ItemGameObject itemGameObject = item.GetComponent<ItemGameObject>();
        itemGameObject.Create(item_size.x + "x" + item_size.y,
            0,
            "item_default_sprite",
            item_size
        );
        return item;
    }
    protected Vector2 FindEmptySpace(Vector2 item_size)
    {
        int row_empty = 0, column_empty = 0, i, j;
        Vector2 coord = new Vector2(0, 0);
        for (i = 0; (i < slot_column) && (column_empty != item_size.y); i++, row_empty = 0)
        {
            if (item_size.y - column_empty > slot_column - i)
                return new Vector2(-1, -1);

            for (j = (int)coord.x; (j < slot_row) && (row_empty != item_size.x); j++)
            {
                if (item_size.x - row_empty > slot_row - j)
                    break;
                if (slots[i * slot_row + j].slot.State == 1)
                {
                    row_empty = 0;
                    column_empty = 0;
                    continue;
                }
                row_empty++;
            }

            if (row_empty == item_size.x)
            {
                column_empty++;
                coord.x = j - item_size.x;
            }
            else if (column_empty > 0)
            {
                column_empty = 0;
                coord.x = 0;
            }
        }
        if (column_empty != item_size.y)
            return new Vector2(-1, -1);
        coord.y = i - item_size.y;
        return coord;
    }

    protected void SetSlotsState(Vector2 start_coord, Vector2 end_coord, float state)
    {
        for (var i = (int)start_coord.y; i < start_coord.y + end_coord.y; i++)
        {
            for (var j = (int)start_coord.x; j < start_coord.x + end_coord.x; j++)
            {
                slots[i * slot_row + j].slot.State = state;
            }
        }
    }

    public virtual void ItemRemoved(ItemGameObject itemGameObject)
    {
        itemGameObject.OnDragging -= ItemRemoved;
        SetSlotsState(new Vector2(
            (float)Math.Floor((itemGameObject.start_anchor - anchor_panel).x / (cellSize + grid_spacing).x),
            (float)Math.Floor((itemGameObject.start_anchor - anchor_panel).y / (cellSize + grid_spacing).y)
        ), itemGameObject.item.Size, 0);
    }
}
