using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
{
    [SerializeField]
    Text Timer;


    public void UpdateTimer(double time)
    {
        if (Timer == null)
        {
            Debug.LogError("Timer doesn't Exist");
        }
        int minuate = (int)time / 60;
        int seconds = (int)time % 60;

        Timer.text = $"{minuate.ToString("D2")} : {seconds.ToString("D2")}";
    }
}
