using UnityEngine;

public class FireField : AreaWeaponBase
{
    protected override void FixedUpdate()
    {
        detectCenter = Managers.Player.transform.position;
        transform.position = detectCenter;
        base.FixedUpdate();
    }
}
