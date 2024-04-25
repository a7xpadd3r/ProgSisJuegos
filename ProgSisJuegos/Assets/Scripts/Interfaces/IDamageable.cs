using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void AnyDamage(float amount);
    void OnDeath();
}
