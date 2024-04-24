using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class MonsterWheelchairBase : MonsterBase, IDamageable
{
    [SerializeField] private Animator _anim;
    [SerializeField] private MonsterWheelchairSFX _additionalSFXScript;
    public Transform theplayer;

    private Transform _selfTransform;
    private CharacterController _cController;
    private float _currentHealth = 1;

    private bool _canAttackAgain = true;
    private float _currentMeleeAttackCooldown;
    private RaycastHit _rHit;

    private void Start()
    {
        _currentHealth = MonsterData.Life;
        _selfTransform = this.transform;
        _cController= this.GetComponent<CharacterController>();
    }

    public void AnyDamage(float amount, bool isDamage = true)
    {
        if (_currentHealth > 0) 
        {
            _currentHealth -= amount;

            if (_currentHealth <= 0)
            {
                _anim.SetTrigger("Death");
                MonsterDeath();
                return;
            }

            bool randBool = Random.Range(0, 10f) <= MonsterData.DamageStunChance;

            if (randBool)
                _anim.SetTrigger("Damaged");
        }
    }

    public override void MonsterDeath()
    {
        Destroy(_cController);
    }

    public override void MeleeAttack()
    {
        if (!_additionalSFXScript.IsStunned)
        {
            _currentMeleeAttackCooldown = MonsterData.AttackCooldown;
            _canAttackAgain = false;

            bool randBool = Random.Range(0, 10f) <= MonsterData.AttackChance;
            if (randBool)
            {
                _anim.SetTrigger("Attack");

                // Find player and apply damage
                Collider[] objectsInRange = Physics.OverlapSphere(transform.position, 10, LayerMask.NameToLayer("Damageable"));
                for (int i = 0; i < objectsInRange.Length; i++)
                {
                    if (objectsInRange[i].CompareTag("Player"))
                    {
                        objectsInRange[i].GetComponent<IDamageable>()?.AnyDamage(1);
                        return;
                    }
                }
            }
            else
                _currentMeleeAttackCooldown = MonsterData.AttackCooldown / 2;
        }
    }

    public override void MoveTo(Vector3 location, float delta)
    {
        _cController.Move(MonsterData.MovementSpeed * delta * location);
    }

    public void OnDeath()
    {
        print("OnDeath");
    }

    private void Update()
    {
        if (_currentHealth > 0 && !_additionalSFXScript.IsStunned) 
        {
            float delta = Time.deltaTime;

            // Attack status reset
            if (_currentMeleeAttackCooldown > 0 && !_canAttackAgain)
                _currentMeleeAttackCooldown -= delta;

            else if (_currentMeleeAttackCooldown <= 0 && !_canAttackAgain)
                _canAttackAgain = true;

            // Chase & attack player
            if (theplayer != null)
            {
                Vector3 dir = theplayer.position - _selfTransform.transform.position;
                Vector3 dirNorm = dir.normalized;
                Vector3 lookDir = new Vector3(-dirNorm.x, 0, -dirNorm.z); // Negative fixes wrong look direction from mesh

                if (dir.magnitude > MonsterData.AttackRange)
                {
                    _anim.SetBool("Walking", true);
                    MoveTo(dirNorm, delta);
                }
                else
                {
                    _anim.SetBool("Walking", false);

                    if (_canAttackAgain)
                        MeleeAttack();
                }

                transform.right = Vector3.LerpUnclamped(transform.right, lookDir, delta);
            }
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, MonsterData.AttackRange);
    }

    public bool IsAlive => _currentHealth > 0;
}
