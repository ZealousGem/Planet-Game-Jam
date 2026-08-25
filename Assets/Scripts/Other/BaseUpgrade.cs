using UnityEngine;

public interface IIEffect
{
    void Effect();
}

[CreateAssetMenu(fileName = "BaseUpgrade", menuName = "Scriptable Objects/BaseUpgrade")]
public abstract class BaseUpgrade : ScriptableObject, IIEffect
{
    [Header("Title of Upgrade")]
    public string Title;

    [Header("Description of Upgrade")]
    public string Description;
    public abstract void Effect();
}


[CreateAssetMenu(fileName = "DamageUpgrade", menuName = "Upgrades/DamageUpgrade")]
public class DamageUpgrade : BaseUpgrade
{
    public float Damage;
    public override void Effect()
    {
        //throw new System.NotImplementedException();
    }
}

[CreateAssetMenu(fileName = "SpeedUpgrade", menuName = "Upgrades/SpeedUpgrade")]
public class SpeedUpgrade : BaseUpgrade
{
    public float Speed;
    public override void Effect()
    {
        //throw new System.NotImplementedException();
    }
}

[CreateAssetMenu(fileName = "AmmoUpgrade", menuName = "Upgrades/AmmoUpgrade")]
public class AmmoUpgrade : BaseUpgrade
{
    public int Ammo;
    public override void Effect()
    {
        //throw new System.NotImplementedException();
    }
}

[CreateAssetMenu(fileName = "ReloadCooldownUpgrade", menuName = "Upgrades/ReloadCooldown")]
public class ReloadCooldownUpgrade : BaseUpgrade
{
    public float Speed;
    public override void Effect()
    {
        //throw new System.NotImplementedException();
    }
}

[CreateAssetMenu(fileName = "ShotCooldownUpgrade", menuName = "Upgrades/ShotCooldownUpgrade")]
public class ShotCooldownUpgrade : BaseUpgrade
{
    public float Speed;
    public override void Effect()
    {
        //throw new System.NotImplementedException();
    }
}

[CreateAssetMenu(fileName = "CameraZoomLenth", menuName = "Upgrades/CameraZoomLenth")]
public class CameraZoomLenthUpgrade : BaseUpgrade
{
    public float Speed;
    public override void Effect()
    {
        //throw new System.NotImplementedException();
    }
}

[CreateAssetMenu(fileName = "RestoreATowerHealthUpgrade", menuName = "Upgrades/RestoreATowerHealthUpgrade")]
public class RestoreATowerHealthUpgrade : BaseUpgrade
{
    public override void Effect()
    {
        //throw new System.NotImplementedException();
    }
}

[CreateAssetMenu(fileName = "IncreaseHitBoxUpgrade", menuName = "Upgrades/IncreaseHitBoxUpgrade")]
public class IncreaseHitBoxUpgrade : BaseUpgrade
{
    public float scale;
    public override void Effect()
    {
        //throw new System.NotImplementedException();
    }
}

[CreateAssetMenu(fileName = "SpawnAnotherTowerUpgrade", menuName = "Upgrades/SpawnAnotherTowerUpgrade")]
public class SpawnAnotherTowerUpgrade : BaseUpgrade
{
    public float scale;
    public override void Effect()
    {
        //throw new System.NotImplementedException();
    }
}

[CreateAssetMenu(fileName = "RestoreAllTowersHealth", menuName = "Upgrades/RestoreAllTowersHealth")]
public class RestoreAllTowersHealth : BaseUpgrade
{
    public override void Effect()
    {
        //throw new System.NotImplementedException();
    }
}


