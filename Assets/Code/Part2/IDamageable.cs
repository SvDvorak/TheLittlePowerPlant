internal interface IDamageable
{
    void DoDamage(float damage);
    float InitialHealth { get; }
}