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

public class GameManagerEvent : EventData
{
    public GameState gameState;

    public GameManagerEvent(GameState gameState)
    {
        this.gameState = gameState;
    }
}
