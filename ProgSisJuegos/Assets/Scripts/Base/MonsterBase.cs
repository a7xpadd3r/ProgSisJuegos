using UnityEngine;

public abstract class MonsterBase : MonoBehaviour
{
    [SerializeField] private MonsterDatabase _monsterData;
    public MonsterDatabase MonsterData => _monsterData;

    public abstract void MoveTo(Vector3 location, float delta);
    public virtual void MeleeAttack() { }
    public virtual void RangedAttack() { }
    public abstract void MonsterDeath();
}
