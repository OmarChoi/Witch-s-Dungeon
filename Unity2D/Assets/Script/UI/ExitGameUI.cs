using UnityEngine;

public class ExitGameUI : MonoBehaviour
{
    // ���� ���� Ȯ�� ���
    public void ButtonClicked(bool ExitGame)
    {
        if (ExitGame)
            Application.Quit();
        else
            Managers.UI.DeActivateUI();
    }
}
