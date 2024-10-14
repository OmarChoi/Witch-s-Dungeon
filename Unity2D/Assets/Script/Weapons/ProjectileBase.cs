using UnityEngine;

public class ProjectileBase : WeaponBase
{
    float speed = 10.0f;
    protected Vector2 direction = Vector2.zero;

    public override void Init()
    {
        direction = Vector2.zero;
        SetDirection();
    }
    public override void PlayAudio()
    {

    }

    public override void UpdateTransform()
    {
        Vector2 currentPos = transform.position;
        float displacement = (currentPos - weaponPos).magnitude;
        if(displacement > AttackRange || direction.magnitude < float.Epsilon)
        {
            Managers.Pool.ReleaseObject(weaponName, this.gameObject);
        }
        else
        {
            this.gameObject.transform.position = currentPos + direction * speed * Time.fixedDeltaTime;
        }
    }

    protected virtual void SetDirection() { }

    protected virtual void CalculateDirection(Transform target) { }
}
