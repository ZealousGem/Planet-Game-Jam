using UnityEngine;

[CreateAssetMenu(fileName = "DamageUpgrade", menuName = "Upgrades/DamageUpgrade")]
public class DamageUpgrade : BaseUpgrade
{
    public float Damage;
    public override void Effect()
    {
        EventBus.Act(new DamageEvent(Damage));
    }
}
