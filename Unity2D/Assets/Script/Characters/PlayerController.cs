using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class PlayerController : ControllerBase
{
    private Define.WeaponInfo[] weaponInfo = new Define.WeaponInfo[(int)Define.Weapon.WeaponTypeCount + (int)Define.Projectile.ProjectileTypeCount];
    AudioSource playerAudioSource = null;
    AudioSource weaponAudioSource = null;
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
        Managers.Scene.GameScene.ChangeWeaponLevel(idx, weaponInfo[idx].WeaponLevel);
        SetAudio();
    }

    private void SetAudio()
    {
        playerAudioSource = this.gameObject.AddComponent<AudioSource>();
        weaponAudioSource = this.gameObject.AddComponent<AudioSource>();
        AudioMixerGroup[] effectGroups = Managers.Audio.Mixer.FindMatchingGroups("Effect");
        if (effectGroups.Length == 0)
            Debug.LogError("Can't Find Group 'Effect' In SoundMixer");
        playerAudioSource.outputAudioMixerGroup = effectGroups[0];
        weaponAudioSource.outputAudioMixerGroup = effectGroups[0];
        weaponAudioSource.mute = false;
        AudioClip audioClip = Managers.Resource.GetResource<AudioClip>("MoveAudio");
        playerAudioSource.clip = audioClip;
        playerAudioSource.loop = true;
        playerAudioSource.Play();
    }

    public void Update()
    {
        if (state == Define.State.Die)
            return;
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
            float execTime = ProgressTime - weaponInfo[i].LastSpawnTime;
            if (execTime < weaponData.SpawnCycle) continue;
            for (int j = 0; j < weaponData.SpawnNum; ++j)
            {
                GameObject weapon = Managers.Pool.GetObject(weaponInfo[i].Name, transform.position);
                WeaponBase weaponController = weapon.GetComponent<WeaponBase>();

                // Audio 설정
                string clipName = weaponInfo[i].Name + "Audio";
                AudioClip weaponAudio = Managers.Resource.GetResource<AudioClip>(clipName);
                weaponAudioSource.PlayOneShot(weaponAudio);

                // 무기 소환
                if (weaponController != null)
                    weaponController.SpawnWeapon(transform.position, weaponInfo[i].WeaponLevel);
            }
            // 마지막 소환시간 설정을 통한 소환 주기 설정
            weaponInfo[i].LastSpawnTime = ProgressTime;
        }
    }

    void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(rigidBody.position.x, rigidBody.position.y, -10);
    }

    protected override void UpdateTransform()
    {
        if (state == Define.State.Die) return;
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Managers.UI.DeActivateUI();
        }
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
                if (playerAudioSource.mute == true)
                    playerAudioSource.mute = false;
                animator.Play("Move");
                if (moveDirection.x < 0)
                    spriteRenderer.flipX = true;
                else
                    spriteRenderer.flipX = false;
                break;
            case Define.State.Idle:
                if (playerAudioSource.mute == false)
                    playerAudioSource.mute = true;
                animator.Play("Idle");
                break;
            case Define.State.Die:
                playerAudioSource.mute = true;
                animator.Play("Die");
                break;
        }
    }

    public void AddExp(float exp)
    {
        currentExp += exp;
        int requiredExp = Managers.Data.GetRequiredExpPerLevel(currentLevel);
        if (currentExp > requiredExp)
        {
            currentExp -= requiredExp;
            currentLevel += 1;
            Managers.Scene.GameScene.SpawnLevelUpUI();
        }
    }

    public void SetWeaponLevel(int idx, int level)
    {
        int settingLevel = Mathf.Clamp(level, 0, Define.MaxWeaponLevel - 1);
        weaponInfo[idx].WeaponLevel = settingLevel;
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
        base.Dead();
        playerAudioSource.mute = true;
        weaponAudioSource.mute = true;
        Managers.Scene.GameScene.GameEnd();
    }
}
