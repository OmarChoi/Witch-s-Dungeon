using UnityEngine;

public class ExitGameUI : MonoBehaviour
{
    // 게임 종료 확인 취소
    public void ButtonClicked(bool ExitGame)
    {
        if (ExitGame)
            Application.Quit();
        else
            Managers.UI.DeActivateUI();
    }
}
