using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponSwitchStatus
{
    None, Switching, Switch
}

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Settings")]
    [SerializeField] private float _life = 15;
    private float _currentLife;
    [SerializeField, Range(1, 5)] private float speed = 15;
    [SerializeField, Range(1, 1000)] private float mouseSensibility = 400;
    [SerializeField] private LayerMask interactionMask;
    [SerializeField, Range(0.1f, 1f)] private float interactionRange = 0.35f;

    [Header("Ground")]
    [SerializeField] private float _groundDistance = 0.4f;
    [SerializeField] private float _gravity = 0.4f;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Transform _groundCheck;
    private bool _isGrounded;
    private Vector3 _groundVelocity;

    [Header("References")]
    [SerializeField] private Animator _anim;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Transform _body;
    [SerializeField] private Image _damagedOverlay;
    [SerializeField] private Image _crosshair;
    [SerializeField] private Image _targetCrosshair;
    private CharacterController _cController;

    [Header("Crosshair")]
    [SerializeField] private Sprite _normalCrosshair;
    [SerializeField] private Sprite _interactableCrosshair;

    [Header("Sounds")]
    [SerializeField] private AudioSource _extraAudioSource;
    [SerializeField] private List<AudioClip> _gettingDamageClips;

    [Header("Weapons")]
    [SerializeField, Range(0.5f, 1)] private float _weaponSwitchTime = 0.5f;
    [SerializeField] private WeaponKitchenKnife _weaponKitchenKnife;
    [SerializeField] private WeaponGlock _weaponGlock;
    private WeaponTypes _currentWeapon = WeaponTypes.None;
    private bool _isKitchenKnifeEnabled;
    private bool _isGlockEnabled;
    private float _currentWeaponSwitchTime;
    private WeaponSwitchStatus _switchStatus = WeaponSwitchStatus.None;
    
    private AudioSource _heartBeatLoopSound;
    private Transform _cameraTransform;
    private RaycastHit _rHit;
    private float _xRot;

    private bool _lIsPlayerEnabled = true;

    public WeaponTypes CurrentWeapon => _currentWeapon;
    public event Action<float> OnAnyDamage;
    public Action<WeaponTypes> OnGiveWeapon;
    public Action OnPlayerDeath;

    private void Start()
    {
        _currentWeaponSwitchTime = _weaponSwitchTime;

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
            WeaponSwitchTiming(delta);

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

            switch (CurrentWeapon)
            {
                case WeaponTypes.None:
                    _targetCrosshair.color = new Color(0, 0, 0, 0);
                    break;
                case WeaponTypes.KitchenKnife:
                    if (CrosshairRaycastObjective(_weaponKitchenKnife.WeaponData.Range))
                        _targetCrosshair.color = new Color(1, 1, 1, 1);
                    else
                        _targetCrosshair.color = new Color(0, 0, 0, 0);

                    break;
                case WeaponTypes.Glock:
                    if (CrosshairRaycastObjective(_weaponGlock.WeaponData.Range))
                        _targetCrosshair.color = new Color(1, 1, 1, 1);
                    else
                        _targetCrosshair.color = new Color(0, 0, 0, 0);

                    break;
            }
        }
    }

    private bool CrosshairRaycastObjective(float range)
    {
        RaycastHit hit;

        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out hit, range))
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Damageable")) 
                return true;

        return false;
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
        _currentLife -= amount;

        if (_currentLife <= 0)
        {
            OnDeath();
            return;
        }

        OnAnyDamage(amount);
        _extraAudioSource.PlayOneShot(_gettingDamageClips[UnityEngine.Random.Range(0, _gettingDamageClips.Count -1)]);

        if (_currentLife <= 5 && !_heartBeatLoopSound.isPlaying) _heartBeatLoopSound.Play();
        else if (_currentLife > 5 && _heartBeatLoopSound.isPlaying) _heartBeatLoopSound.Stop();
    }

    public void OnDeath()
    {
        OnPlayerDeath?.Invoke();
    }

    private void Movement(float delta)
    {
        // Check ground
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

        if (_isGrounded && _groundVelocity.y < 0)
            _groundVelocity.y = -2f;

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

        // Gravity
        _groundVelocity.y += _gravity * delta;
        _cController.Move(_groundVelocity * delta);
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

                    _weaponKitchenKnife.WeaponIn();
                }
                _currentWeapon = WeaponTypes.KitchenKnife;
                break;

            case WeaponTypes.KitchenKnife:
                if (_isGlockEnabled && _weaponKitchenKnife.CanAttackAgain)
                {
                    _weaponKitchenKnife.gameObject.SetActive(false);
                    _weaponGlock.gameObject.SetActive(true);

                    _weaponGlock.WeaponIn();
                }
                _currentWeapon = WeaponTypes.Glock;
                break;

            case WeaponTypes.Glock:
                if (_isKitchenKnifeEnabled && !_weaponGlock.IsReloading && _weaponGlock.CanShootAgain)
                {
                    _weaponKitchenKnife.gameObject.SetActive(true);
                    _weaponGlock.gameObject.SetActive(false);

                    _weaponKitchenKnife.WeaponIn();
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
        }

        SimpleWeaponSwap();
    }

    private void WeaponSwitchTiming(float deltaTime)
    {
        if (!_isKitchenKnifeEnabled || !_isGlockEnabled) return;

        // Trigger
        if (Input.GetKeyDown(KeyCode.Q) && _switchStatus == WeaponSwitchStatus.None)
        {
            switch (CurrentWeapon)
            {
                case WeaponTypes.KitchenKnife:
                    if (!_weaponKitchenKnife.CanAttackAgain) return;
                    break;
                case WeaponTypes.Glock:
                    if (!_weaponGlock.CanShootAgain) return;
                    break;
            }

            _currentWeaponSwitchTime = _weaponSwitchTime;
            _switchStatus = WeaponSwitchStatus.Switching;

            switch (_currentWeapon)
            {
                case WeaponTypes.KitchenKnife:
                    _weaponKitchenKnife.WeaponOut();
                    break;
                case WeaponTypes.Glock:
                    _weaponGlock.WeaponOut();
                    break;
            }
        }

        switch (_switchStatus)
        {
            case WeaponSwitchStatus.None:
                break;
            case WeaponSwitchStatus.Switching:
                _currentWeaponSwitchTime -= deltaTime;

                if (_currentWeaponSwitchTime <= 0)
                    _switchStatus = WeaponSwitchStatus.Switch;

                break;
            case WeaponSwitchStatus.Switch:
                SimpleWeaponSwap();
                _switchStatus = WeaponSwitchStatus.None;
                break;
            default:
                break;
        }
    }
}