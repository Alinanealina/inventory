using System;
using UnityEngine;

[Serializable]
public class Slot
{
    [SerializeField] private float state = 0;
    [SerializeField] private Vector2 size;
    [SerializeField] private string item_name = "";
    public float State
    {
        get => state;
        set
        {
            state = value;
        }
    }
    public Vector2 Size
    {
        get => size;
        set
        {
            size = value;
        }
    }
    public string Item_name
    {
        get => item_name;
        set
        {
            item_name = value;
        }
    }
}
