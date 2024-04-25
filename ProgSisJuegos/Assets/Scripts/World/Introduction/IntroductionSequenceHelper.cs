using UnityEngine;
using UnityEngine.UI;

public class IntroductionSequenceHelper : MonoBehaviour
{
    public Image oldFadeImage;
    public GameObject nonInteractablePills;
    public GameObject interactablePills;

    void Start()
    {
        interactablePills.gameObject.SetActive(true);
        oldFadeImage.gameObject.SetActive(false);
        nonInteractablePills.gameObject.SetActive(false);
    }
}
