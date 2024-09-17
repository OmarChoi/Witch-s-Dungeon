using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : ControllerBase
{
    protected override void UpdateTransform()
    {
        base.UpdateTransform();
        GetKeyBoardInput();
        UpdateAnimation();
    }
    void GetKeyBoardInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            direction |= Define.Direction.Up;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            direction |= Define.Direction.Down;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            direction |= Define.Direction.Left;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            direction |= Define.Direction.Right;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            direction ^= Define.Direction.Up;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            direction ^= Define.Direction.Down;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            direction ^= Define.Direction.Left;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            direction ^= Define.Direction.Right;
        }

        if (direction == Define.Direction.None)
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
                if ((direction & Define.Direction.Left) != 0)
                    spriteRenderer.flipX = true;
                else if ((direction & Define.Direction.Right) != 0)
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
}
