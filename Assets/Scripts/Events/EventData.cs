using Unity.Mathematics;
using UnityEngine;

public class EventData
{
    public EventData() { }
}


public class GameStateEvent : EventData
{
    public GameState gameState;

    public GameStateEvent(GameState gameState)
    {
        this.gameState = gameState;
    }

}

public class WaveStateEvent : GameStateEvent
{
    public int WaveAmount;

    public WaveStateEvent(GameState gameState, int WaveAmount) : base(gameState)
    {
        this.WaveAmount = WaveAmount;
    }
}

public class GameManagerEvent : EventData
{
    public GameState gameState;

    public GameManagerEvent(GameState gameState)
    {
        this.gameState = gameState;
    }
}

public class UIEvent : EventData { }

public class NumUIEvent : UIEvent
{
    public UIevents uiType;
    public int num;

    public NumUIEvent(UIevents uIevents, int num)
    {
        this.num = num;
        this.uiType = uIevents;
    }
}

public class PopUpEvent : UIEvent
{
    public UIevents uiType;
    public string stringText;

    public PopUpEvent(UIevents uIevents, string stringText)
    {
        this.stringText = stringText;
        this.uiType = uIevents;
    }
}

public class EnemiesKilledEvent : EventData
{
    public int num;

    public EnemiesKilledEvent(int num)
    {
        this.num = num;
    }
}

public class SettlementCounterEvent : EventData
{
    public int num;

    public SettlementCounterEvent(int num)
    {
        this.num = num;
    }
}

public class UpgradeEvent : EventData
{


    public UpgradeEvent()
    {
    }
}

public class DamageEvent : UpgradeEvent
{
    public float Damage;
    public DamageEvent(float Damage)
    {
        this.Damage = Damage;
    }
}

public class AmmoEvent : UpgradeEvent
{
    public float Ammo;
    public AmmoEvent(float Damage)
    {
        this.Ammo = Damage;
    }
}

public class ShotCoolDownEvent : UpgradeEvent
{
    public float CoolDown;
    public ShotCoolDownEvent(float Damage)
    {
        this.CoolDown = Damage;
    }
}

public class ReloadTimerEvent : UpgradeEvent
{
    public float CoolDown;
    public ReloadTimerEvent(float Damage)
    {
        this.CoolDown = Damage;
    }
}

public class SpeedEvent : UpgradeEvent
{
    public float Speed;
    public SpeedEvent(float Damage)
    {
        this.Speed = Damage;
    }
}

public class HitBoxEvent : UpgradeEvent
{
    public Vector2 Scale;
    public HitBoxEvent(Vector2 Damage)
    {
        this.Scale = Damage;
    }
}

public class CameraZoomEvent : UpgradeEvent
{
    public float Size;
    public CameraZoomEvent(float Damage)
    {
        this.Size = Damage;
    }
}

public class SpawnTowerEvent : UpgradeEvent
{
    public GameObject Tower;
    public int counter = 1;
    public SpawnTowerEvent(GameObject Damage)
    {
        this.Tower = Damage;
    }
}

public class HealATowerEvent : UpgradeEvent
{
    public string Name;
    public HealATowerEvent(string Damage)
    {
        this.Name = Damage;
    }
}

public class HealALLTowerEvent : UpgradeEvent
{

    public HealALLTowerEvent()
    {
    }
}




