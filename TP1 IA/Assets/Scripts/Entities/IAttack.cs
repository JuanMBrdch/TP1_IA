public interface IAttack
{
    void Attack();
    bool IsAttacking { get; set; }
    float AttackRange { get; set; }
    float AttackCooldown { get; set; }
}
