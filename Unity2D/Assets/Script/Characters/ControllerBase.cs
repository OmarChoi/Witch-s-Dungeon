using UnityEngine;

public class ControllerBase : MonoBehaviour
{
    protected Define.Direction direction = Define.Direction.None;
    protected Define.State state = Define.State.Idle;
    protected Define.Status status = new Define.Status();
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigidBody;
    protected Vector2 moveDirection = Vector2.zero;

    public float HP { get { return status.CurrentHp; } protected set { status.CurrentHp = value; } }
    public float MaxHp { get { return status.MaxHp; } protected set { status.CurrentHp = value; } }
    public float Speed { get { return status.Speed; } protected set { status.Speed = value; } }
    public float Damage { get { return status.Damage; } protected set { status.Damage = value; } }

    public void Start()
    {
        Init();
    }

    public void Awake()
    {
        Init();
    }

    public virtual void Init()
    {
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

    private void FixedUpdate()
    {
        UpdateTransform();
    }

    protected virtual void UpdateTransform()
    {
        Vector2 nextPos = rigidBody.position + moveDirection * Speed * Time.fixedDeltaTime;
        // check Can Go
        rigidBody.MovePosition(nextPos);
        rigidBody.velocity = Vector2.zero;
    }
}
