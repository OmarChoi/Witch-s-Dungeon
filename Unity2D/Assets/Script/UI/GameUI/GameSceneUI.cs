using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
{
    [SerializeField] Text timerText;
    [SerializeField] Slider expBar;
    [SerializeField] Slider hpBar;
    [SerializeField] Text levelText;

    public void UpdateTimer(double time)
    {
        if (timerText == null)
        {
            Debug.LogError("Timer doesn't Exist");
        }
        int minuate = (int)time / 60;
        int seconds = (int)time % 60;

        timerText.text = $"{minuate.ToString("D2")} : {seconds.ToString("D2")}";
    }

    private void LateUpdate()
    {
        float currentExp = Managers.Player.GetComponent<ControllerBase>().currentExp;
        int currentLevel = Managers.Player.GetComponent<ControllerBase>().currentLevel;
        int totalExp = Managers.Data.GetRequiredExpPerLevel(currentLevel);
        expBar.value = currentExp / totalExp;
        hpBar.value = (float)Managers.Player.GetComponent<ControllerBase>().HP / Managers.Player.GetComponent<ControllerBase>().MaxHp;
        levelText.text = $"{currentLevel.ToString("D2")}";
    }

    private void Pause()
    {

    }
}
