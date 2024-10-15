using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    Define.WeaponData weaponData = new Define.WeaponData();
    protected Vector2 weaponPos = Vector2.zero;

    public string weaponName = string.Empty;
    protected LayerMask targetLayer;
    protected Collider2D[] monstersInRange;
    public Transform nearestTarget;
    int currentLevel = -1;

    public float Damage { get { return weaponData.Damage * Managers.Player.GetComponent<ControllerBase>().Damage; } }
    public float Duration { get {  return weaponData.Duration; } }
    public float AttackRange { get {  return weaponData.AttackRange; } }
    public float AttackCycle { get { return weaponData.AttackCycle * Managers.Player.GetComponent<ControllerBase>().AttackSpeed; } }

    protected virtual void Awake()
    {
        targetLayer = (1 << (int)Define.Layer.Attackable);
        weaponName = Utils.GetNameExceptClone(transform.name);
    }

    public void FixedUpdate()
    {
        UpdateTransform();
    }

    public virtual void Init() { }

    public virtual void PlayAudio() { }

    public abstract void UpdateTransform();

    protected void GetTargetInCircleArea()
    {
        monstersInRange = Physics2D.OverlapCircleAll(weaponPos, AttackRange, targetLayer, 0.0f, 0.0f);
    }

    protected virtual Transform GetNearestTarget()
    {
        Transform targetObject = null;
        float minDist = AttackRange;

        foreach (Collider2D monster in monstersInRange)
        {
            if (monster == null) continue;
            Vector3 playerPosition = transform.position;
            Vector3 targetPosition = monster.transform.position;
            float targetDist = Vector3.Distance(playerPosition, targetPosition);

            if (targetDist < minDist)
            {
                minDist = targetDist;
                targetObject = monster.transform;
            }
        }
        return targetObject;
    }

    public virtual void SpawnWeapon(Vector2 startPosition, int level)
    {
        if (currentLevel != level)
        {
            Managers.Data.GetWeaponData(weaponName, level, ref weaponData);
            currentLevel = level;
        }
        weaponPos = startPosition;
        Init();
    }

    protected virtual void Clear()
    {
        Managers.Pool.ReleaseObject(weaponName, this.gameObject);
    }
}
