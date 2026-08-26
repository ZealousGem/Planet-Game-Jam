using UnityEngine;

[CreateAssetMenu(fileName = "RestoreAllTowersHealth", menuName = "Upgrades/RestoreAllTowersHealth")]
public class RestoreAllTowersHealth : BaseUpgrade
{
    public override void Effect()
    {
        //throw new System.NotImplementedException();
        EventBus.Act(new HealALLTowerEvent());
    }
}
