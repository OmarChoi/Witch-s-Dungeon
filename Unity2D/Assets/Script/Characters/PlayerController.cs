using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : ControllerBase
{
    void GetKeyBoardInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            direction |= Define.Direction.Up;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            direction |= Define.Direction.Up;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            direction |= Define.Direction.Up;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            direction |= Define.Direction.Up;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            direction ^= Define.Direction.Up;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            direction ^= Define.Direction.Up;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            direction ^= Define.Direction.Up;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            direction ^= Define.Direction.Up;
        }
    }
}
