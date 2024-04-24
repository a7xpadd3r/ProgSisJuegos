public interface IWeapon
{
    float recoil { get; set; }
    void Attack();
    void AttackMeleeRay();
    void AttackMissSound();
    void AttackHitSound();
}
