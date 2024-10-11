using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : ControllerBase
{
    private Define.WeaponInfo[] weaponInfo = new Define.WeaponInfo[(int)Define.Weapon.WeaponTypeCount + (int)Define.Projectile.ProjectileTypeCount];
    private float deltaTime = 0.0f;
    protected Define.State state = Define.State.Idle;

    string baseWeapon = "Shuriken";

    public override void Init()
    {
        base.Init();
        Managers.Data.GetPlayerStatus(out this.status);
        for (int i = 0; i < weaponInfo.Length; ++i)
        {
            weaponInfo[i].Name = Define.GetWeaponName(i);
            weaponInfo[i].WeaponLevel = -1;
            weaponInfo[i].LastSpawnTime = -60.0f;
        }
        int idx = Define.GetWeaponIndex(baseWeapon);
        weaponInfo[idx].WeaponLevel = 0;
        Managers.Scene.ChangeWeaponLevel(idx, 0);
    }

    public void Update()
    {
        GetKeyBoardInput();
        SpawnWeapon();
    }

    private void SpawnWeapon()
    {
        for (int i = 0; i < weaponInfo.Length; ++i) 
        {
            if (weaponInfo[i].WeaponLevel < 0) continue;
            Define.WeaponData weaponData = new Define.WeaponData();
            Managers.Data.GetWeaponData(weaponInfo[i].Name, weaponInfo[i].WeaponLevel, ref weaponData);
            float execTime = deltaTime - weaponInfo[i].LastSpawnTime;
            if (execTime < weaponData.SpawnCycle) continue;
            for (int j = 0; j < weaponData.SpawnNum; ++j)
            {
                GameObject weapon = Managers.Pool.GetObject(weaponInfo[i].Name, transform.position);
                WeaponBase weaponController = weapon.GetComponent<WeaponBase>();
                if (weaponController != null)
                {
                    weaponController.SpawnWeapon(transform.position, weaponInfo[i].WeaponLevel);
                    weaponInfo[i].LastSpawnTime = deltaTime;
                }
            }
        }
    }

    void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(rigidBody.position.x, rigidBody.position.y, -10);
    }

    protected override void UpdateTransform()
    {
        if (state == Define.State.Die) return;
        deltaTime += Time.fixedDeltaTime;
        UpdateAnimation();
        base.UpdateTransform();
    }

    private void OnMove(InputValue value)
    {
        if (state == Define.State.Die) return;
            moveDirection = value.Get<Vector2>();
    }

    void GetKeyBoardInput()
    {
        if (moveDirection.magnitude == 0)
            state = Define.State.Idle;
        else
            state = Define.State.Move;
    }

    void UpdateAnimation()
    {
        switch(state)
        {
            case Define.State.Move:
                animator.Play("Move");
                if (moveDirection.x < 0)
                    spriteRenderer.flipX = true;
                else
                    spriteRenderer.flipX = false;
                break;
            case Define.State.Idle:
                animator.Play("Idle");
                break;
            case Define.State.Die:
                animator.Play("Die");
                break;
        }
    }

    public void AddExp(int exp)
    {
        currentExp += exp;
        int requiredExp = Managers.Data.GetRequiredExpPerLevel(currentLevel);
        if (currentExp > requiredExp)
        {
            currentExp -= requiredExp;
            currentLevel += 1;
            Managers.Scene.SpawnLevelUpUI();
        }
    }

    public void SetWeaponLevel(int idx, int level)
    {
        weaponInfo[idx].WeaponLevel = level - 1;
    }

    public int GetWeaponLevel(int idx)
    {
        if(idx < 0 || idx >= weaponInfo.Length)
        {
            Debug.LogError("Index Error PlayerController.cs GetWeaponInfo()");
        }
        return weaponInfo[idx].WeaponLevel;
    }


    protected override void Dead()
    {
        Managers.Scene.GameEnd();
    }
}
