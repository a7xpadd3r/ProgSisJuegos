using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Settings")]
    [SerializeField] private float _life = 15;
    [SerializeField, Range(1, 5)] private float speed = 15;
    [SerializeField, Range(1, 1000)] private float mouseSensibility = 400;
    [SerializeField] private LayerMask interactionMask;
    [SerializeField, Range(0.1f, 1f)] private float interactionRange = 0.35f;
    [SerializeField] private Color _damageColor = new Color(1,0,0, 0.2f);
    
    [Header("References")]
    [SerializeField] private Animator _anim;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Transform _body;
    [SerializeField] private Image _damagedOverlay;
    private CharacterController _cController;

    [Header("Crosshair")]
    [SerializeField] private Image _crosshair;
    [SerializeField] private Sprite _normalCrosshair;
    [SerializeField] private Sprite _interactableCrosshair;

    private AudioSource _heartBeatLoopSound;
    private float _xRot;
    private float _currentLife;
    private Transform _cameraTransform;
    private RaycastHit _rHit;

    private bool _lIsPlayerEnabled = true;

    public event Action<float> OnAnyDamage;

    private void Start()
    {
        _cController = GetComponent<CharacterController>();
        _heartBeatLoopSound = GetComponent<AudioSource>();
        _cameraTransform = _playerCamera.transform;
        _currentLife = _life;
        Gizmos.color = Color.red;
        
        // Hide and lock mouse to game
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (_lIsPlayerEnabled)
        {
            float delta = Time.deltaTime;

            // Player movement
            Vector2 movementInput = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical"));

            Transform transf = transform;
            Vector3 move = transf.right * movementInput.x + transf.forward * movementInput.y;
            _cController.Move(speed * delta * move);

            // Player animation
            if (move != Vector3.zero) _anim.SetBool("Walk", true);
            else _anim.SetBool("Walk", false);

            // Mouse movement
            Vector2 mouseInput = new Vector2(
                Input.GetAxis("Mouse X") * mouseSensibility * delta,
                Input.GetAxis("Mouse Y") * mouseSensibility * delta);

            _xRot -= mouseInput.y;
            _xRot = Mathf.Clamp(_xRot, -90, 90);

            _playerCamera.transform.localRotation = Quaternion.Euler(_xRot, 0, 0);
            _body.Rotate(Vector3.up * mouseInput.x);

            // Interaction
            if (Input.GetKeyDown(KeyCode.E))
                Interact(_cameraTransform, interactionRange);

            if (_crosshair != null)
            {
                if (Physics.Raycast(
                    _cameraTransform.position,
                    _cameraTransform.forward,
                    out _rHit,
                    interactionRange
                    ))
                    if (_rHit.transform.gameObject.layer == LayerMask.NameToLayer("Interactable"))
                    {
                        _crosshair.sprite = _interactableCrosshair;
                        _crosshair.color = Color.white;
                        return;
                    }               
                
                _crosshair.sprite = _normalCrosshair;
                _crosshair.color = new Color(1,1,1, 0.15f);
            }
        }
    }

    private void Interact(Transform transformInput, float maxRange)
    {
        if (Physics.Raycast(
                transformInput.position,
                transformInput.forward,
                out _rHit,
                maxRange
            ))
            if (_rHit.transform.gameObject.layer == LayerMask.NameToLayer("Interactable"))
            {
                IInteractable interactable = _rHit.transform.gameObject.GetComponent<IInteractable>();
                interactable?.Interact();
            }
    }
    
    void OnDrawGizmos()
    {
        if (_cameraTransform != null)
            Gizmos.DrawRay(_cameraTransform.position, _cameraTransform.forward * interactionRange);
    }

    public void LightDisable(bool isEnabled)
    {
        _lIsPlayerEnabled = isEnabled;
    }

    public GameObject kitchenknife;
    public void TheKK()
    {
        kitchenknife.gameObject.SetActive(true);
    }

    public void EDisableCController(bool isEnabled)
    {
        this._cController.enabled = isEnabled;
    }

    public void AnyDamage(float amount)
    {
        OnAnyDamage(amount);
        _currentLife -= amount;

        if (_currentLife <= 5) _heartBeatLoopSound.Play();
    }

    public void OnDeath()
    {
        throw new NotImplementedException();
    }
}