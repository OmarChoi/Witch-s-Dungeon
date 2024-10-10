using System;
using UnityEditor;
using UnityEngine;

public class Define
{
    public const int TotalTime = 15;
    public const int SpawnCycle = 3;
    public const int SpawnRange = 10;
    public const int MaxWeaponLevel = 5;

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

    public enum Weapon
    {
        FireField,
        Cutter,
        WeaponTypeCount
    }

    public enum Projectile
    {
       Shuriken,
       Bird,
       ProjectileTypeCount
    }

    static public int GetWeaponIndex(string weaponName)
    {
        if (Enum.TryParse(typeof(Weapon), weaponName, true, out var resultInWeapon))
        {
            return (int)(Weapon)resultInWeapon;
        }
        else if (Enum.TryParse(typeof(Projectile), weaponName, true, out var resultInProjectile))
        {
            return (int)(Projectile)resultInProjectile + (int)Weapon.WeaponTypeCount;
        }
        else
        {
            return -1;
        }
    }

    static public string GetWeaponName(int index)
    {
        if (index >= (int)Weapon.WeaponTypeCount)
        {
            index -= (int)Weapon.WeaponTypeCount;
            return Enum.GetName(typeof(Projectile), index).ToString();
        }
        else
        {
            return Enum.GetName(typeof(Weapon), index).ToString();
        }
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
        public int Exp;

        public Status(float hp = 0.0f, float speed = 0.0f, float damage = 0.0f, int exp = 0)
        {
            MaxHp = hp;
            CurrentHp = hp;
            Speed = speed;
            Damage = damage;
            Exp = exp;
        }
    }

    public struct WeaponData
    {
        public float Damage;
        public float Duration;
        public float AttackRange;
        public float AttackCycle;
        public float SpawnCycle;
        public int SpawnNum;
    }
}
