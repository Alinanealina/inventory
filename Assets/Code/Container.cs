using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IContainer
{
    void FillWithItems();
}

public abstract class Container : MonoBehaviour, IContainer
{
    protected List<Slot> slots = new();
    [SerializeField] private GameObject slot_prefab;
    public abstract void FillWithItems();
    
    protected void AddSlots(int slots_count)
    {
        for (var i = 0; i < slots_count; i++)
        {
            CleanSlotAt(0);
            GameObject obj = Instantiate(slot_prefab, transform);
            slots.Add(obj.GetComponent<Slot>());
        }
    }
    private void CleanSlotAt(int i)
    {
        if (slots.Count > 0)
        {
            Destroy(slots[i]);
            slots.RemoveAt(i);
        }
    }
}
