using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField, Range(0, 10)] private float damage;
    [SerializeField, Range(1, 3)] private float life = 1;
    [SerializeField, Range(10, 50)] private float speed = 10;
    [SerializeField, Range(0.01f, 10)] private float _speedDesviation = 1.5f;
    [SerializeField, Range(0.01f, 0.2f)] private float _directionDesviation = 0.12f;
    [SerializeField, Range(0.01f, 1)] private float _fixHeight = 0.12f;
    public Vector3 direction;
    private Rigidbody _rBody;

    Coroutine _destroyTimer;
    public bool IsCooldown => _destroyTimer != null;

    private void Start()
    {
        _rBody = GetComponent<Rigidbody>();
        transform.rotation = Quaternion.LookRotation(direction);

        Vector3 randVariation = new Vector3(
            transform.forward.x + Random.Range(-_directionDesviation, _directionDesviation),
            transform.forward.y + Random.Range(-_directionDesviation, _directionDesviation + _fixHeight), /* go for the head */
            transform.forward.z + Random.Range(-_directionDesviation, _directionDesviation));

        _rBody.AddForce(Random.Range(speed - _speedDesviation, speed + _speedDesviation) * randVariation, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {

        collision.transform.TryGetComponent(out IDamageable damageable);
        damageable?.AnyDamage(damage);
        Destroy(this.gameObject);
    }

    private void Update()
    {
        life -= Time.deltaTime;

        if (life <= 0)
            Destroy(this.gameObject);
    }
}
