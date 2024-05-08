using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElevatorStatus
{
    Opening, Closing, Open, Closed
}

public class NightmareElevatorScene : MonoBehaviour
{
    public int _currentIndex = 0;

    public UIManager uiManager;
    public Camera theCamera;
    public List<CameraSequence> sequences = new List<CameraSequence>();

    public PlayerController thePlayer;
    public GameObject nightmareObject;
    public Transform nightmareDestination;

    [Header("Elevator stuff")]
    public float openingSpeed = 1;
    public float openOffset = 4.5f;
    private float startingZ = 0;
    public GameObject leftDoor;
    public GameObject rightDoor;
    public AudioSource audioSource;
    public AudioClip movementClip;
    public AudioClip stopClip;
    public ElevatorStatus elevStatus = ElevatorStatus.Closed;

    private void Start()
    {
        if (uiManager == null || theCamera == null || sequences.Count == 0)
            return;

        startingZ = leftDoor.transform.localPosition.z;
        StartSequence();
    }

    private void Update()
    {
        if (elevStatus == ElevatorStatus.Open || elevStatus == ElevatorStatus.Closed) return;

        float finalSpeed = (Time.deltaTime * openingSpeed) / 100;
        switch (elevStatus)
        {
            case ElevatorStatus.Opening:
                if (leftDoor.transform.localPosition.z > startingZ - openOffset)
                {
                    leftDoor.transform.localPosition = new Vector3(
                        leftDoor.transform.localPosition.x,
                        leftDoor.transform.localPosition.y,
                        leftDoor.transform.localPosition.z - finalSpeed);

                    rightDoor.transform.localPosition = new Vector3(
                        rightDoor.transform.localPosition.x,
                        rightDoor.transform.localPosition.y,
                        rightDoor.transform.localPosition.z + finalSpeed);
                }
                else
                {
                    audioSource.PlayOneShot(stopClip);
                    elevStatus = ElevatorStatus.Open;
                }
                break;
            case ElevatorStatus.Closing:
                if (leftDoor.transform.localPosition.z < startingZ - openOffset)
                {
                    leftDoor.transform.localPosition = new Vector3(
                        leftDoor.transform.localPosition.x,
                        leftDoor.transform.localPosition.y,
                        leftDoor.transform.localPosition.z + finalSpeed);

                    rightDoor.transform.localPosition = new Vector3(
                        rightDoor.transform.localPosition.x,
                        rightDoor.transform.localPosition.y,
                        rightDoor.transform.localPosition.z - finalSpeed);
                }
                else
                {
                    audioSource.PlayOneShot(stopClip);
                    elevStatus = ElevatorStatus.Closed;
                }
                break;
        }
    }

    void StartSequence()
    {
        foreach (CameraSequence sequence in sequences)
        {
            sequence.OnSequenceStart += StartingSequence;
            sequence.OnSequenceEnded += EndingSequence;
        }

        sequences[_currentIndex].StartSequence(theCamera);
    }

    private void OpenElevator()
    {
        elevStatus = ElevatorStatus.Opening;
        audioSource?.PlayOneShot(movementClip);
    }

    private void CloseElevator()
    {
        elevStatus = ElevatorStatus.Closing;
        audioSource?.PlayOneShot(movementClip);
    }


    void StartingSequence(bool fadeIn, float duration)
    {
        if (fadeIn) uiManager.OnCameraFade(true, duration, Color.black);

        if (_currentIndex == 3) OpenElevator();
        if (_currentIndex == 6) EnableNightmares();
        if (_currentIndex == sequences.Count - 1) EndElevatorSequence();
    }

    void EnableNightmares()
    {
        nightmareObject.SetActive(true);
    }

    void EndElevatorSequence()
    {
        thePlayer.gameObject.transform.position = nightmareDestination.position;
        thePlayer.gameObject.transform.rotation = nightmareDestination.rotation;
        thePlayer.gameObject.SetActive(true);

        foreach (CameraSequence sequence in sequences)
        {
            sequence.OnSequenceStart -= StartingSequence;
            sequence.OnSequenceEnded -= EndingSequence;
        }

        gameObject.SetActive(false);
    }

    void EndingSequence(bool fadeOut, float duration)
    {
        if (fadeOut) uiManager.OnCameraFade(false, duration, Color.black);
        _currentIndex++;

        if (_currentIndex >= sequences.Count) return;
        sequences[_currentIndex]?.StartSequence(theCamera);
    }
}
