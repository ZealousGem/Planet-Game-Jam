using UnityEngine;

[CreateAssetMenu(fileName = "SpeedUpgrade", menuName = "Upgrades/SpeedUpgrade")]
public class SpeedUpgrade : BaseUpgrade
{
    public float Speed;
    public override void Effect()
    {
        //throw new System.NotImplementedException();
        EventBus.Act(new SpeedEvent(Speed));
        Debug.Log("speed");
    }
}
