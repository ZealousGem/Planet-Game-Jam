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
    public int num;

    public UpgradeEvent(int num)
    {
        this.num = num;
    }
}


