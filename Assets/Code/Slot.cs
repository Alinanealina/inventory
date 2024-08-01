using UnityEngine;

public abstract class Slot : MonoBehaviour
{
    private float state = 0;
    private Vector2 size;
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
}
