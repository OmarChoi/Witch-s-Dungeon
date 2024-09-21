using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class EnemyController : ControllerBase
{
    int monsterId = 0;
    string enemyType = string.Empty;
    Vector2 targetPos = Vector2.zero;
    Vector2 moveDirection = Vector2.zero;

    public override void Init()
    {
        base.Init();
        enemyType = transform.name;
        Managers.Data.enemyData.TryGetValue(enemyType, out status);
    }

    public void SpawnMonster(Vector2 pos, int id)
    {
        position = new Vector3(pos.x, pos.y, 0);
        monsterId = id;
    }

    public void StartMove(Vector3 target)
    {

    }

    public void UpdateTargetDirection(Vector3 target)
    {
        targetPos = target;
        moveDirection = (targetPos - position).normalized;
    }

    protected override void UpdateTransform()
    {
        UpdatePosition();
        UpdateAnimation();
    }

    private void UpdatePosition()
    { 
        Vector2 nextPos = position + moveDirection * (Speed * 0.1f) * Time.deltaTime;
        float dist = Vector2.Distance(nextPos, targetPos);
        if (dist > 1.0f)
        {
            position = nextPos;
            transform.position = position;
        }
    }

    private void UpdateAnimation()
    {
        switch (state)
        {
            case Define.State.Move:
                break;
        }
    }
}
