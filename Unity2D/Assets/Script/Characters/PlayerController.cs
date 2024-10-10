using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : ControllerBase
{
    struct WeaponInfo
    {
        public string Name;
        public int WeaponLevel;
        public float LastSpawnTime;
    }
    private float deltaTime = 0.0f;
    protected Define.State state = Define.State.Idle;
    private WeaponInfo[] weaponInfo = new WeaponInfo[(int)Define.Weapon.WeaponTypeCount + (int)Define.Projectile.ProjectileTypeCount];
    string baseWeapon = "Shuriken";

    public override void Init()
    {
        base.Init();
        Speed = 5;
        HP = 100.0f;
        MaxHp = 1000.0f;

        for (int i = 0; i < weaponInfo.Length; ++i)
        {
            weaponInfo[i].Name = Define.GetWeaponName(i);
            weaponInfo[i].WeaponLevel = -1;
            weaponInfo[i].LastSpawnTime = -60.0f;
        }
        int idx = Define.GetWeaponIndex(baseWeapon);
        weaponInfo[idx].WeaponLevel = 4;
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
        deltaTime += Time.fixedDeltaTime;
        UpdateAnimation();
        base.UpdateTransform();
    }

    private void OnMove(InputValue value)
    {
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
            // 무기 선택 UI 실행
            // 무기 선택에 따른 처리 진행
        }
    }

    protected override void Dead()
    {
        Debug.Log("Player is Dead");
    }
}
