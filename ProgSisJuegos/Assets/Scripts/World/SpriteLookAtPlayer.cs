using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLookAtPlayer : MonoBehaviour
{
    public GameObject playerReference;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        if (playerReference != null)
            transform.forward = playerReference.transform.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _animator.SetBool("Trigger", true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _animator.SetBool("Trigger", false);
    }
}
