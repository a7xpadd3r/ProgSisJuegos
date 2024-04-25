using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroductionSequencePills : MonoBehaviour, IInteractable
{
    [Range(0.1f, 2)] public float glowSpeed = 1;
    [Range(1, 5)] public float delayToChangeLevel = 3;
    Light _ilumination;
    bool _brightnessUp;
    AudioSource _sfx;
    BoxCollider _boxCollider;
    bool _interacted;
    bool _noSpamChangeLevel = true;

    [Header("References")]
    public UIManager uiManager;
    public PlayerController pControllerScript;
    public List<GameObject> disableThisSfxs= new List<GameObject>();
    public GameObject mesh;
    public string nextLevelScene;

    private void Start()
    {
        _ilumination = GetComponent<Light>();
        _sfx = GetComponent<AudioSource>();
        _boxCollider = GetComponent<BoxCollider>();
    }

    public void Interact()
    {
        for (int i = 0; i < disableThisSfxs.Count; i++)
            disableThisSfxs[i].gameObject.SetActive(false);

        _interacted = true;
        _sfx.Play();
        Destroy(_boxCollider);
        Destroy(mesh);
        _ilumination.intensity = 0;
        pControllerScript.LightDisable(false);

        uiManager.OnForcedQuickFade(Color.black);
    }

    void Update()
    {
        float delta = Time.deltaTime;

        if (!_interacted)
        {
            if (_brightnessUp)
            {
                _ilumination.intensity += delta * glowSpeed;

                if (_ilumination.intensity >= 1)
                    _brightnessUp = false;
            }
            else
            {
                _ilumination.intensity -= delta * glowSpeed;

                if (_ilumination.intensity <= 0.1)
                    _brightnessUp = true;
            }
        }

        else
        {
            if (delayToChangeLevel > 0)
                delayToChangeLevel -= delta;
            else
                _noSpamChangeLevel = false;

            if (!_noSpamChangeLevel)
            {
                StartCoroutine(GotoLevel());
                _noSpamChangeLevel = true;
            }

        }
    }

    IEnumerator GotoLevel()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextLevelScene);

        while (!asyncLoad.isDone)
            yield return null;
    }
}
