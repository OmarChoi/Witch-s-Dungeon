using UnityEngine;

public class ControllerBase : MonoBehaviour
{
    protected Define.Direction direction = Define.Direction.None;
    protected Define.State state = Define.State.Idle;
    protected float speed = 5;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;


    Vector2 position = Vector2.zero;

    public void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError($"{this.name} Animator Does't Exist");
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(spriteRenderer == null)
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
        float deltaPos = speed * Time.deltaTime;
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
