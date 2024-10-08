using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FireField : AreaWeaponBase
{
    public override void Init()
    {
        base.Init();
    }

    public override void UpdateTransform()
    {
        weaponPos = Managers.Player.transform.position;
        transform.position = weaponPos;
    }
}
