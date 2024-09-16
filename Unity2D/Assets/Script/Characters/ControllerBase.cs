using UnityEngine;

public class ControllerBase : MonoBehaviour
{
    protected Define.Direction direction = Define.Direction.None;
    Vector2 position = Vector2.zero;
    protected float speed = 5;

    private void Update()
    {
        UpdateTransform();
    }

    public void UpdateTransform()
    {
        Vector2 nextPos = position;
        switch (direction)
        {
            case Define.Direction.Left:
                nextPos.x = position.x - speed * Time.deltaTime;
                break;
            case Define.Direction.Right:
                nextPos.y = position.y + speed * Time.deltaTime;
                break;
            case Define.Direction.Up:
                nextPos.x = position.x + speed * Time.deltaTime;
                break;
            case Define.Direction.Down:
                nextPos.y = position.y - speed * Time.deltaTime;
                break;
        }

        // check Can Go
        position = nextPos;
        transform.position = position;
    }
}
