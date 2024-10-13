using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    [SerializeField] Image fadePanel;
    bool IsActive = false;
    float execTime = 1.0f;
    float duration = 1.0f;

    public void FadeIn(float time)
    {
        duration = time;
        IsActive = true;
        StartCoroutine(StartFadeIn());
    }

    IEnumerator StartFadeIn()
    {
        Color color = fadePanel.color;
        execTime = 0;
        while (color.a < 1)
        {
            execTime += Time.deltaTime / duration;
            color.a = Mathf.Lerp(0, 1, execTime);
            fadePanel.color = color;
            yield return null;
        }
        yield break;
    }

    public void FadeOut()
    {

    }
}
