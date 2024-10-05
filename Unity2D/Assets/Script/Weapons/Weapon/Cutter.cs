using System.Collections;
using UnityEngine;

public class Cutter : AreaWeaponBase
{
    public override void Init()
    {
        Animator animator = GetComponent<Animator>();
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        cooldown = clipInfo[0].clip.length;
        delay = spawnCycle * 0.2f;
        duration = 7.0f;
        spawnCycle = 15.0f;
        base.Init();
    }

    protected override void UpdatePosition()
    {
        if(Managers.Player == null) return;
        detectCenter = Managers.Player.transform.position;
        transform.position = detectCenter;
    }
}
