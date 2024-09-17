using UnityEngine;

public class Define
{
    public enum Direction : byte
    {
        None = 0,
        Up = 0x01,
        Right = 0x02,
        Down = 0x04,
        Left = 0x08,
    }

    public enum State
    {
        Idle= 0,
        Move = 1,
        Die = 2,
    }
}
