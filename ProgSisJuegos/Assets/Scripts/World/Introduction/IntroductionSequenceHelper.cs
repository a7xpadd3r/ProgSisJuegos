using UnityEngine;
using UnityEngine.UI;

public class IntroductionSequenceHelper : MonoBehaviour
{
    public Image fadeImage;
    public Image oldFadeImage;
    public GameObject nonInteractablePills;
    public GameObject interactablePills;

    void Start()
    {
        fadeImage.gameObject.SetActive(true);
        interactablePills.gameObject.SetActive(true);
        oldFadeImage.gameObject.SetActive(false);
        nonInteractablePills.gameObject.SetActive(false);
        fadeImage.CrossFadeColor(new Color(0,0,0,0), 15, true, true);
    }
}
