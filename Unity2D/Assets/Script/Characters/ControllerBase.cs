using UnityEngine;

public class ControllerBase : MonoBehaviour
{
    protected Define.Direction direction = Define.Direction.None;
    protected Define.State state = Define.State.Idle;
    protected Define.Status status = new Define.Status();
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    public float HP { get { return status.CurrentHp; } protected set { status.CurrentHp = value; } }
    public float MaxHp { get { return status.MaxHp; } protected set { status.CurrentHp = value; } }
    public float Speed { get { return status.Speed; } protected set { status.Speed = value; } }
    public float Damage { get { return status.Damage; } protected set { status.Damage = value; } }

    protected Vector2 position = Vector2.zero;

    public void Start()
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
    }

    private void Update()
    {
        UpdateTransform();
    }

    protected virtual void UpdateTransform()
    {
        Vector2 nextPos = position;
        float deltaPos = Speed * Time.deltaTime;
        if ((direction & Define.Direction.Up) != 0)
            nextPos.y = position.y + deltaPos;
        if ((direction & Define.Direction.Right) != 0)
            nextPos.x = position.x + deltaPos;
        if ((direction & Define.Direction.Down) != 0)
            nextPos.y = position.y - deltaPos;
        if ((direction & Define.Direction.Left) != 0)
            nextPos.x = position.x - deltaPos;

        // check Can Go
        position = nextPos;
        transform.position = position;
    }
}
