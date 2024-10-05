using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FireField : AreaWeaponBase
{
    public override void Init()
    {
        delay = 0.0f;
        cooldown = 0.1f;
        duration = 5.0f;
        spawnCycle = 2.0f;
        base.Init();
    }

    protected override void UpdatePosition()
    {
        detectCenter = Managers.Player.transform.position;
        transform.position = detectCenter;
    }
}
