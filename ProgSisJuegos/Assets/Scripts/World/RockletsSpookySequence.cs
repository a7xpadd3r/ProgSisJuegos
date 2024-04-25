using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;

public class RockletsSpookySequence : MonoBehaviour
{
    public List<GameObject> allTheSpookyWalls;
    public MonsterWheelchairBase wheelchairMonster;
    public UIManager uiManager;
    public Transform teleportDestionationAfterSequence;
    public PlayerController thePlayer;
    public GameObject rockletsStuffToDestroy;

    [Header("Settings")]
    public bool isSpookySequenceReady;

    [Header("Spooky faces")]
    public List<MeshRenderer> spookyFaces;
    public float timeBetweenFacesSwap = 0.35f;
    public float _currentTimeBetweenFacesSwap;
    public int _currentFaceIndex;

    void Start()
    {
        for (int i = 0; i < allTheSpookyWalls.Count; i++)
            allTheSpookyWalls[i].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpookySequenceReady)
        {
            float delta = Time.deltaTime;
            
            if (!wheelchairMonster.IsAlive)
            {
                thePlayer.EDisableCController(false);
                thePlayer.transform.position = new Vector3(teleportDestionationAfterSequence.position.x, thePlayer.transform.position.y, teleportDestionationAfterSequence.position.z);
                thePlayer.EDisableCController(true);

                uiManager.OnCameraFade(true, 0.5f, Color.white);
                isSpookySequenceReady = false;                
                
                for (int i = 0; i < allTheSpookyWalls.Count; i++)
                    allTheSpookyWalls[i].SetActive(false);

                Destroy(rockletsStuffToDestroy);
            }

            if (_currentTimeBetweenFacesSwap < timeBetweenFacesSwap)
                _currentTimeBetweenFacesSwap += delta;
            else
            {
                spookyFaces[_currentFaceIndex].enabled = false;

                if (_currentFaceIndex >= spookyFaces.Count - 1)
                    _currentFaceIndex = 0;
                else
                    _currentFaceIndex++;

                spookyFaces[_currentFaceIndex].enabled = true;

                _currentTimeBetweenFacesSwap = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < allTheSpookyWalls.Count; i++)
        {
            allTheSpookyWalls[i].SetActive(true);
        }

        isSpookySequenceReady = true;
    }
}
