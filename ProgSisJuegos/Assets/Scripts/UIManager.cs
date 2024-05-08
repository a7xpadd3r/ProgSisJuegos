using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private Image damageImage;
    [SerializeField] private PlayerController thePlayer;

    public Action<bool, float, Color> OnCameraFade;
    public Action<Color> OnForcedQuickFade;
    public Action OnDeathFade;

    private void Awake()
    {
        OnCameraFade += Fade;
        OnForcedQuickFade += SnapFade;
        OnDeathFade += DeathFade;

        thePlayer.OnAnyDamage += DamageFade;
    }

    private void OnDestroy()
    {
        OnCameraFade -= Fade;
        OnForcedQuickFade -= SnapFade;
        OnDeathFade -= DeathFade;

        thePlayer.OnAnyDamage -= DamageFade;
    }

    private void SnapFade(Color color)
    {
        fadeImage.gameObject.SetActive(false);
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = color;
    }

    private void Fade(bool fadeIn, float duration, Color color)
    {
        fadeImage.gameObject.SetActive(false);
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(true);

        if (fadeIn)
        {
            fadeImage.CrossFadeAlpha(1, 0, true);
            fadeImage.CrossFadeAlpha(0, duration, true);
        }
        else
        {
            fadeImage.CrossFadeAlpha(0, 0, true);
            fadeImage.CrossFadeAlpha(1f, duration, true);
        }

    }

    private void DamageFade(float amount)
    {
        damageImage.CrossFadeAlpha(0.2f, 0f, true);

        if (amount > 0)
            damageImage.color = new Color(1, 0, 0, 0.2f);
        else
            damageImage.color = new Color(0, 1, 0, 0.2f);

        damageImage.CrossFadeAlpha(0, 0.5f, true);
    }

    private void DeathFade()
    {
        fadeImage.gameObject.SetActive(false);
        damageImage.gameObject.SetActive(false);

        damageImage.color = new Color(1, 0, 0, 1);
        damageImage.gameObject.SetActive(true);
        damageImage.CrossFadeColor(Color.black, 2, true, false);
    }
}
