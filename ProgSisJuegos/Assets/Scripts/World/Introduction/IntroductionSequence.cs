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

    [Header("Settings")]
    [Range(0.01f, 1)] public List<float> deltaTimes;
    [Range(0.1f, 10)] public List<float> camTimes;
    [Range(0.1f, 3)] public float initialDelay = 1;
    [Range(0.1f, 20)] public float fadeInTime = 15;
    [Range(0.1f, 1)] public float fadOutTime = 1;

    private float _startingDelay;
    private float _currentCamTime;
    private int _currentIndex;
    private Transform currentTransform;
    private float _currentDelayToGameplay;
    private bool _mainSeqEnded;

    public Color _fadeInColor = new Color(0,0,0,0);
    public Color _fadeOutColor = new Color(0,0,0,1);

    private void Start()
    {
        currentTransform = positions[0];
        _currentDelayToGameplay = fadOutTime + fadOutTime + fadOutTime + fadOutTime + fadOutTime;
    }

    void Update()
    {
        float delta = Time.deltaTime;
        BathroomLightIntensity(delta);

        if (_startingDelay < initialDelay)
        {
            _startingDelay += delta;
            return;
        }

        if (_currentIndex == 0)
            CameraFade(true);
        else if (_currentIndex == 200)
        {
            fadeImage.gameObject.SetActive(false);
            _currentIndex = 250;
        }

        if (_mainSeqEnded && _currentDelayToGameplay > 0)
            _currentDelayToGameplay -= delta;
        else if (_mainSeqEnded && _currentDelayToGameplay <= 0 && !thePlayer.activeSelf)
        {
            theMannequin.gameObject.SetActive(false);
            thePlayer.SetActive(true);
            mainCamera.gameObject.SetActive(false);
            _currentIndex = 200;
        }

        if (_currentIndex >= positions.Count)
        {
            CameraFade(false);
            _mainSeqEnded = true;
            return;
        }
            
        
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

    void CameraFade(bool fadeIn)
    {
        if (fadeIn && fadeImage.color != _fadeInColor)
            fadeImage.CrossFadeColor(_fadeInColor, fadeInTime, true, true);

        else if (!fadeIn && fadeImage.color != _fadeOutColor)
            fadeImage.CrossFadeColor(_fadeOutColor, fadOutTime, true, true);
    }

    void BathroomLightIntensity(float delta)
    {
        bathroomLight.intensity -= delta;
        if (bathroomLight.intensity < 0.85) bathroomLight.intensity = 1;
    }
}
