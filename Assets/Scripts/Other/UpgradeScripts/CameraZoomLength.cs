using UnityEngine;

[CreateAssetMenu(fileName = "CameraZoomLenth", menuName = "Upgrades/CameraZoomLenth")]
public class CameraZoomUpgrade : BaseUpgrade
{
    public float Speed;
    public override void Effect()
    {
        EventBus.Act(new CameraZoomEvent(Speed));
    }
}
