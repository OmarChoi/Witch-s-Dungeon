using UnityEngine;

public class ControllerBase : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody2D rigidBody;
    protected SpriteRenderer spriteRenderer;

    protected Vector2 moveDirection = Vector2.zero;
    protected Define.Status status = new Define.Status();

    private float deltaTime = 0.0f;
    public int currentExp = 0;
    public int currentLevel = 1;

    public float HP { get { return status.CurrentHp; } protected set { status.CurrentHp = value; } }
    public float MaxHp { get { return status.MaxHp; } protected set { status.CurrentHp = value; } }
    public float Speed { get { return status.Speed; } protected set { status.Speed = value; } }
    public float Damage { get { return status.Damage; } protected set { status.Damage = value; } }
    public float AttackSpeed { get { return status.AttackSpeed; } protected set { status.AttackSpeed = value; } }
    public Vector2 Direction { get { return moveDirection; } }
    protected float ProgressTime { get { return deltaTime; } }

    public void Awake()
    {
        Init();
    }

    public void OnEnable()
    {
        ResetData();
    }

    public virtual void Init()
    {
        if (animator != null) return;
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError($"{this.name} Animator Does't Exist");
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError($"{this.name} Sprite Renderer Does't Exist");
        }
        rigidBody = GetComponent<Rigidbody2D>();
        if( rigidBody == null )
        {
            Debug.LogError($"{this.name} RigidBody2D Does't Exist");
        }
    }

    public virtual void ResetData()
    {
        HP = MaxHp;
        moveDirection = Vector2.zero;
    }

    private void FixedUpdate()
    {
        deltaTime += Time.fixedDeltaTime;
        UpdateTransform();
    }

    protected virtual void UpdateTransform()
    {
        Vector2 nextPos = rigidBody.position + moveDirection * Speed * Time.fixedDeltaTime;
        rigidBody.MovePosition(nextPos);
        rigidBody.velocity = Vector2.zero;
    }

    public void GetDamage(float damage)
    {
        HP -= damage;
        if (HP < float.Epsilon)
        {
            Dead();
        }
    }

    protected virtual void Dead()
    {

    }
}
