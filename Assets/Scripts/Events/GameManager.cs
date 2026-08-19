using UnityEngine;

public enum GameState { Start, Ongoing, Success, Failure, EndedWave, StartWave, Upgrades }

public class GameManager : MonoBehaviour
{
    private GameState currentGamestate = GameState.Start;
    private float ingameTimer = 0f;
    private int waveIndex = 1;
    [SerializeField] private int TowerAmount = 0;

    private int TotalEnemiesInWave = 0;

    private int currentEnemiesKilld = 0;

    private void TowerCounter()
    {
        TowerAmount--;

        if (TowerAmount <= 0)
        {
            setGameState(GameState.Failure);
        }
    }

    private void setGameState(GameState gameState)
    {
        currentGamestate = gameState;

        switch (currentGamestate)
        {
            case GameState.Success: break;
            case GameState.Failure: break;
            case GameState.EndedWave: break;
            case GameState.StartWave: break;
            case GameState.Ongoing: break;
        }
    }
    private void Update()
    {
        if (currentGamestate == GameState.Ongoing || currentGamestate == GameState.EndedWave) ingameTimer += Time.deltaTime;
    }

    void IncreaseBotKilledCount(int num)  // increases the enemiy kill count everytime enemy has been killed 
    {
        currentEnemiesKilld += num;

        if (currentEnemiesKilld >= TotalEnemiesInWave)
        {
            currentEnemiesKilld = 0;
            TotalEnemiesInWave += 4;
            waveIndex++;

            setGameState(GameState.StartWave);

        }
    }
}
