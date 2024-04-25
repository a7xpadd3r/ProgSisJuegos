using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroductionSequence : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;
    public List<Transform> positions;
    public Image fadeImage;
    public Light bathroomLight;
    public GameObject thePlayer;
    public GameObject theMannequin;
    public UIManager uiManager;

    [Header("Settings")]
    [Range(0.01f, 1)] public List<float> deltaTimes;
    [Range(0.1f, 10)] public List<float> camTimes;
    [Range(0.1f, 3)] public float initialDelay = 1;
    [Range(0.1f, 20)] public float fadeInTime = 2;
    [Range(0.1f, 1)] public float fadeOutTime = 1;

    private float _startingDelay;
    private float _currentCamTime;
    private int _currentIndex;
    private Transform currentTransform;
    private float _currentDelayToGameplay;
    private bool _mainSeqEnded;

    public Color _fadeColor = new Color(0,0,0,0);

    private void Start()
    {
        currentTransform = positions[0];
        _currentDelayToGameplay = fadeOutTime + fadeOutTime + fadeOutTime + fadeOutTime + fadeOutTime;
    }

    void Update()
    {
        float delta = Time.deltaTime;
        BathroomLightIntensity(delta);

        // Little delay before starting
        if (_startingDelay < initialDelay)
        {
            _startingDelay += delta;
            return;
        }

        // Initial camera fade in
        if (_currentIndex == 0)
            uiManager.OnCameraFade(true, fadeInTime, _fadeColor);

        // While introduction is going
        if (_mainSeqEnded && _currentDelayToGameplay > 0)
            _currentDelayToGameplay -= delta;

        // Gameplay should start
        else if (_mainSeqEnded && _currentDelayToGameplay <= 0 && !thePlayer.activeSelf)
        {
            theMannequin.gameObject.SetActive(false);
            thePlayer.SetActive(true);
            mainCamera.gameObject.SetActive(false);
        }

        // Finishing introduction & camera fade out to gameplay
        if (_currentIndex >= positions.Count && !_mainSeqEnded)
        {
            uiManager.OnCameraFade(false, fadeOutTime, _fadeColor);
            _mainSeqEnded = true;
            return;
        }
        
        // Camera lerp between points
        if (!_mainSeqEnded)
        {
            Transform currentCamTransform = mainCamera.transform;

            mainCamera.transform.position = Vector3.Lerp(currentCamTransform.position, currentTransform.position, delta * deltaTimes[_currentIndex]);
            mainCamera.transform.rotation = Quaternion.Lerp(currentCamTransform.rotation, currentTransform.rotation, delta * deltaTimes[_currentIndex]);

            if (_currentCamTime >= camTimes[_currentIndex])
            {

                _currentCamTime = 0;
                _currentIndex++;

                if (_currentIndex < positions.Count)
                    currentTransform = positions[_currentIndex];
            }
            else
                _currentCamTime += delta;
        }
    }

    void BathroomLightIntensity(float delta)
    {
        bathroomLight.intensity -= delta;
        if (bathroomLight.intensity < 0.85) bathroomLight.intensity = 1;
    }
}
