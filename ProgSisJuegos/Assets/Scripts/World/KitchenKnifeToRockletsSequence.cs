using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class KitchenKnifeToRockletsSequence : MonoBehaviour
{
    [Header("References")]
    public PlayerController thePlayer;
    public ItemKitchenKnife kitchenKnifeScript;
    public GameObject rockletCamera;
    public GameObject fakeDavidAnimator;
    public GameObject fakeKnife;

    public Transform rocketsDestination;
    public Image uiFadeImage;

    [Header("Mini sequence")]
    public bool startFadeOut;
    public float timeBeforeFadeOut = 9;
    public bool noFadeSpam;

    [Header("Sequence finished")]
    public bool resumeGameplay;
    public float timeBeforeResumeGameplay = 5;
    public bool noSpamResume;

    void Update()
    {
        float delta = Time.deltaTime;

        if (startFadeOut)
        {
            if (!noFadeSpam && timeBeforeFadeOut > 0)
                timeBeforeFadeOut -= delta;

            else if (!noFadeSpam && timeBeforeFadeOut <= 0)
            {
                noFadeSpam = true;
                uiFadeImage.CrossFadeColor(new Color(0, 0, 0, 1), 1, true, true);
                resumeGameplay = true;
            }
        }

        if (resumeGameplay)
        {
            if (!noSpamResume && timeBeforeResumeGameplay > 0)
                timeBeforeResumeGameplay -= delta;

            else if (!noSpamResume && timeBeforeResumeGameplay <= 0)
            {
                noSpamResume = true;
                RestorePlayer();
                uiFadeImage.CrossFadeColor(new Color(0, 0, 0, 0), 1, true, true);
            }
        }
    }

    public void StartRockletsSequence()
    {
        startFadeOut = true;
        // Start fade in
        uiFadeImage.color = Color.black;
        uiFadeImage.CrossFadeColor(new Color(0, 0, 0, 0), 3, true, true);

        // Disable player, activate camera, teleport to rocklets
        rockletCamera.gameObject.SetActive(true);
        thePlayer.gameObject.SetActive(false);        
        thePlayer.transform.position = new Vector3(rocketsDestination.position.x, thePlayer.transform.position.y, rocketsDestination.position.z);

        // Enable animated david
        fakeDavidAnimator.gameObject.SetActive(true);        
    }

    private void RestorePlayer()
    {
        Destroy(fakeKnife); Destroy(fakeDavidAnimator);
        thePlayer.gameObject.SetActive(true);
        rockletCamera.gameObject.SetActive(false);
        kitchenKnifeScript.Interact();
        Destroy(this.gameObject);
    }
}
