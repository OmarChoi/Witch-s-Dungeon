using System.Xml.Linq;
using UnityEngine;

public class Define
{
    public const int TotalTime = 15;
    public const int SpawnCycle = 3;
    public const int SpawnRange = 15;

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

    public enum Monster
    {
        Mouse,
        Slime,
        Ghost,
        Skeleton,
        Zombie,
        Minotauros,
        Jinn,
        MonsterTypeCount,
    }

    public enum Layer
    {
        Attackable = 10,
        Weapon = 11,
    }

    public class Status
    {
        public float MaxHp;
        public float CurrentHp;
        public float Speed;
        public float Damage;

        public Status(float hp = 0.0f, float speed = 0.0f, float damage = 0.0f)
        {
            MaxHp = hp;
            CurrentHp = hp;
            Speed = speed;
            Damage = damage;
        }
    }
}
