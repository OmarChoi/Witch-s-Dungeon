using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FireField : AreaWeaponBase
{
    public override void Init()
    {
        float scale = AttackRange / 2.0f;
        this.gameObject.transform.localScale = new Vector3(scale, scale, scale);
        base.Init();
    }

    public override void UpdateTransform()
    {
        weaponPos = Managers.Player.transform.position;
        transform.position = weaponPos;
    }
}
