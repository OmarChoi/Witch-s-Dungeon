using UnityEngine;

public class RespawnMap : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area")) 
            return;

        Vector3 playerPos = Managers.Player.transform.position;
        Vector3 myPos = transform.position;
        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        Vector3 playerDir = Managers.Player.GetComponent<ControllerBase>().Direction;
        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        if (diffX > diffY)
        {
            transform.Translate(Vector3.right * dirX * 40);
        }
        else if (diffX < diffY)
        {
            transform.Translate(Vector3.up * dirY * 40);
        }
    }
}
