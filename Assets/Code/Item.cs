using UnityEngine;

public class Item
{
    private string item_name;
    private float state;
    private Sprite sprite;
    private Vector2 size;
    public Item()
    {
        Initialize("1x1", 0, "item_default_sprite", new Vector2(1, 1));
        Debug.Log(sprite);
    }
    public Item(string item_name, float state, string sprite_name, Vector2 size)
    {
        Initialize(item_name, state, sprite_name, size);
    }
    private void Initialize(string item_name, float state, string sprite_name, Vector2 size)
    {
        this.item_name = item_name;
        this.state = state;
        sprite = Resources.Load<Sprite>("Sprites/" + sprite_name);
        this.size = size;
    }
    
    public string Item_name
    {
        get => item_name;
        set
        {
            item_name = value;
        }
    }
    public float State
    {
        get => state;
        set
        {
            state = value;
        }
    }
    public Sprite Sprite
    {
        get => sprite;
        set
        {
            sprite = value;
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
}
