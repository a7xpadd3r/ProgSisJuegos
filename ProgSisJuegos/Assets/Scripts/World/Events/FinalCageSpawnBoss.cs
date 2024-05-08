using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCageSpawnBoss : MonoBehaviour
{
    public Camera theCamera;
    public GameManager manager;
    public UIManager uiManager;
    public GameObject thePlayer;

    public int _currentIndex = 0;
    public List<CameraSequence> sequences = new List<CameraSequence>();

    private List<MonsterFaster> _bosses = new List<MonsterFaster>();

    void Start()
    {
        StartSequence();
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

    void StartingSequence(bool fadeIn, float duration)
    {
        if (fadeIn) uiManager.OnCameraFade(true, duration, Color.black);
    }

    void EndingSequence(bool fadeOut, float duration)
    {
        if (fadeOut) uiManager.OnCameraFade(false, duration, Color.black);
        _currentIndex++;

        if (_currentIndex >= sequences.Count) return;
        sequences[_currentIndex]?.StartSequence(theCamera);

        if (_currentIndex >= sequences.Count -1)
        {
            theCamera.gameObject.SetActive(false);
            thePlayer.SetActive(true);

            foreach (CameraSequence sequence in sequences)
            {
                sequence.OnSequenceStart -= StartingSequence;
                sequence.OnSequenceEnded -= EndingSequence;
            }

            foreach (MonsterFaster boss in _bosses)
                boss.enabled = true;
        }
    }

    void BossDeath()
    {
        foreach (EnemyBase boss in _bosses)
            if (boss.IsAlive) return;

        foreach (EnemyBase boss in _bosses)
            boss.OnEnemyDeath -= BossDeath;

        manager.OnGameFinised();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy") || _bosses.Count >= 2) return;

        MonsterFaster bossScript = other.gameObject.GetComponent<MonsterFaster>();

        // Set stuff and disable main script
        bossScript._thePlayer = thePlayer;
        bossScript.OnEnemyDeath += BossDeath;        
        bossScript.enabled = false;
        _bosses.Add(bossScript);
    }
}
