using System;
using System.Collections;
using UnityEngine;

public class CameraSequence : MonoBehaviour
{
    [Range(0.1f, 5)] public float duration = 1;
    [Range(0.1f, 5)] public float fadeDuration = 1f;
    [Range(0.1f, 10)] public float transitionSpeed = 1;
    public bool fadeIn = false;
    public bool fadeOut = false;

    public Action<bool, float> OnSequenceStart;
    public Action<bool, float> OnSequenceEnded;

    private Camera theCamera;
    private Coroutine _timer;

    public void StartSequence(Camera camera)
    {
        theCamera = camera;
        _timer = StartCoroutine(SequenceWait());
    }

    private void Update()
    {
        if (_timer == null || theCamera == null) return;

        float delta = Time.deltaTime;
        theCamera.transform.position = Vector3.Lerp(theCamera.transform.position, transform.position, delta * transitionSpeed);
        theCamera.transform.rotation = Quaternion.Lerp(theCamera.transform.rotation, transform.rotation, delta * transitionSpeed);
    }

    IEnumerator SequenceWait()
    {
        OnSequenceStart?.Invoke(fadeIn, fadeDuration);
        yield return new WaitForSeconds(duration);

        OnSequenceEnded?.Invoke(fadeOut, fadeDuration);
        _timer = null;
    }
}
