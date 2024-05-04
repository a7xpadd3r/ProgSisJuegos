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

    [Header("Weapons")]
    [SerializeField] private WeaponKitchenKnife _weaponKitchenKnife;
    [SerializeField] private WeaponGlock _weaponGlock;
    private WeaponTypes _currentWeapon = WeaponTypes.None;
    private bool _isKitchenKnifeEnabled;
    private bool _isGlockEnabled;
    
    private AudioSource _heartBeatLoopSound;
    private float _xRot;
    private float _currentLife;
    private Transform _cameraTransform;
    private RaycastHit _rHit;

    private bool _lIsPlayerEnabled = true;

    public WeaponTypes CurrentWeapon => _currentWeapon;
    public event Action<float> OnAnyDamage;
    public Action<WeaponTypes> OnGiveWeapon;
    public Action OnPlayerDeath;

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

        OnGiveWeapon += GiveWeapon;
    }

    private void Update()
    {
        if (_lIsPlayerEnabled)
        {
            float delta = Time.deltaTime;

            Movement(delta);
            if (Input.GetKeyDown(KeyCode.Q)) SimpleWeaponSwap();

            // Interaction
            if (Input.GetKeyDown(KeyCode.E))
                Interact(_cameraTransform, interactionRange);
            
            // Crosshair changes when something interactable is in the way
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

    public void LightDisable(bool isEnabled)
    {
        _lIsPlayerEnabled = isEnabled;
    }

    public void EDisableCController(bool isEnabled)
    {
        this._cController.enabled = isEnabled;
    }

    public void AnyDamage(float amount)
    {
        OnAnyDamage(amount);
        _currentLife -= amount;

        if (_currentLife <= 5 && !_heartBeatLoopSound.isPlaying) _heartBeatLoopSound.Play();
        else if (_currentLife > 5 && _heartBeatLoopSound.isPlaying) _heartBeatLoopSound.Stop();

        if (_currentLife <= 0)
            OnDeath();
    }

    public void OnDeath()
    {
        OnPlayerDeath?.Invoke();
    }

    private void Movement(float delta)
    {
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

        // Mouse movement to camera view
        Vector2 mouseInput = new Vector2(
            Input.GetAxis("Mouse X") * mouseSensibility * delta,
            Input.GetAxis("Mouse Y") * mouseSensibility * delta);

        _xRot -= mouseInput.y;
        _xRot = Mathf.Clamp(_xRot, -90, 90);

        _playerCamera.transform.localRotation = Quaternion.Euler(_xRot, 0, 0);
        _body.Rotate(Vector3.up * mouseInput.x);
    }

    private void SimpleWeaponSwap()
    {
            switch (_currentWeapon)
            {
                case WeaponTypes.None:
                    if (_isKitchenKnifeEnabled)
                    {
                        _weaponKitchenKnife.gameObject.SetActive(true);
                        _weaponGlock.gameObject.SetActive(false); // Just in case
                    }
                    _currentWeapon = WeaponTypes.KitchenKnife;
                    break;

                case WeaponTypes.KitchenKnife:
                    if (_isGlockEnabled)
                    {
                        _weaponKitchenKnife.gameObject.SetActive(false);
                        _weaponGlock.gameObject.SetActive(true);
                    }
                    _currentWeapon = WeaponTypes.Glock;
                    break;

                case WeaponTypes.Glock:
                    if (_isKitchenKnifeEnabled)
                    {
                        _weaponKitchenKnife.gameObject.SetActive(true);
                        _weaponGlock.gameObject.SetActive(false);
                    }
                    _currentWeapon = WeaponTypes.KitchenKnife;
                    break;
            }
    }

    private void GiveWeapon(WeaponTypes type)
    {
        switch (type)
        {
            case WeaponTypes.KitchenKnife:
                _isKitchenKnifeEnabled = true;
                break;
            case WeaponTypes.Glock:
                _isGlockEnabled = true;
                break;
            default:
                break;
        }

        SimpleWeaponSwap();
    }
}