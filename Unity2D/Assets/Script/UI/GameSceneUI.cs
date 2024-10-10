using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
{
    [SerializeField]
    Text timerText;
    [SerializeField]
    Slider expBar;
    [SerializeField]
    Text levelText;

    private void Awake()
    {
        if (timerText == null)
            timerText = GetComponent<Text>();
        if (expBar == null)
            expBar = GetComponent<Slider>();
    }

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
        int currentExp = Managers.Player.GetComponent<ControllerBase>().currentExp;
        int currentLevel = Managers.Player.GetComponent<ControllerBase>().currentLevel;
        int totalExp = Managers.Data.GetRequiredExpPerLevel(currentLevel);
        expBar.value = (float)currentExp / totalExp;
        levelText.text = $"{currentLevel.ToString("D2")}";
    }
}
