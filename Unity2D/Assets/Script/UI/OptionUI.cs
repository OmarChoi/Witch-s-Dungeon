using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] GameObject soundController;

    int[,] resolution =
    {
        { 1280, 720 },
        { 1920, 1080 },
        { 2560, 1440 }
    };

    public void ButtonClicked(string name)
    {
        switch (name)
        {
            case "Screen":
                int selectindex = GetComponentInChildren<Dropdown>().value;
                int width = 0;
                int height = 0;
                bool bFullScreen = false;
                if(selectindex >= resolution.Length / 2)
                {
                    selectindex = selectindex - resolution.Length / 2;
                    bFullScreen = true;
                }
                width = resolution[selectindex, 0];
                height = resolution[selectindex, 1];
                Screen.SetResolution(width, height, bFullScreen);
                break;
            case "Sound":
                soundController.gameObject.SetActive(true);
                break;
            case "Quit":
                this.gameObject.SetActive(false);
                break;
        }
    }
}
