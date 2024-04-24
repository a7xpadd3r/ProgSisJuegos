using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image fadeImage;
    public Image damageImage;

    public void Fade(bool fadeIn, float duration)
    {
        if (fadeIn)
        {
            fadeImage.CrossFadeAlpha(1f, 0f, true);
            fadeImage.CrossFadeAlpha(0, duration, true);
        }
        else
        {
            fadeImage.CrossFadeAlpha(0, 0f, true);
            fadeImage.CrossFadeAlpha(1f, duration, true);
        }

    }

    public void DamageFade(Color color, float duration = 0.5f)
    {
        damageImage.CrossFadeAlpha(0.2f, 0f, true);
        damageImage.color = color;
        damageImage.CrossFadeAlpha(0, duration, true);
    }
}
