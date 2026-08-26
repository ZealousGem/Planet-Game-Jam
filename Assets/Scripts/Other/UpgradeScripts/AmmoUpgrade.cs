using UnityEngine;

[CreateAssetMenu(fileName = "AmmoUpgrade", menuName = "Upgrades/AmmoUpgrade")]
public class AmmoUpgrade : BaseUpgrade
{
    public int Ammo;
    public override void Effect()
    {
        //throw new System.NotImplementedException();
        EventBus.Act(new AmmoEvent(Ammo));
    }
}

