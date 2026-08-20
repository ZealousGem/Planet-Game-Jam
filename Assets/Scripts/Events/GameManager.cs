using UnityEngine;

public enum GameState { Start, Ongoing, Success, Failure, EndedWave, StartWave, Upgrades }

public class GameManager : MonoBehaviour
{
    private GameState currentGamestate = GameState.Start;
    private float ingameTimer = 0f;
    private int waveIndex = 1;

    [Header("Amount od Settlements in scene")]
    [SerializeField] private int TowerAmount = 0;

    [Header("Amount of Enemies in Each Wave")]
    [SerializeField] private int TotalEnemiesInWave = 10;

    private int currentEnemiesKilld = 0;

    private void OnEnable()
    {
        EventBus.Subscribe<GameManagerEvent>(retrieveData);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<GameManagerEvent>(retrieveData);
    }

    private void retrieveData(GameManagerEvent data)
    {
        setGameState(data.gameState);
    }

    void Start()
    {
        EventBus.Act(new NumUIEvent(UIevents.Bases, TowerAmount));
        EventBus.Act(new NumUIEvent(UIevents.Waves, waveIndex));

        setGameState(GameState.StartWave);
        setGameState(GameState.Ongoing);
    }

    private void TowerCounter()
    {
        TowerAmount--;
        EventBus.Act(new NumUIEvent(UIevents.Bases, TowerAmount));

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
            case GameState.StartWave:
                EventBus.Act(new WaveStateEvent(GameState.StartWave, TotalEnemiesInWave));
                EventBus.Act(new NumUIEvent(UIevents.EnemiesLeft, TotalEnemiesInWave)); break;
            case GameState.Failure: break;
        }
    }
    private void Update()
    {
        if (currentGamestate == GameState.Ongoing || currentGamestate == GameState.EndedWave) ingameTimer += Time.deltaTime;
    }

    void IncreaseBotKilledCount(int num)  // increases the enemiy kill count everytime enemy has been killed 
    {
        currentEnemiesKilld += num;
        EventBus.Act(new NumUIEvent(UIevents.EnemiesLeft, TotalEnemiesInWave - currentEnemiesKilld));

        if (currentEnemiesKilld >= TotalEnemiesInWave)
        {
            currentEnemiesKilld = 0;
            TotalEnemiesInWave += 4;

            waveIndex++;
            EventBus.Act(new NumUIEvent(UIevents.Waves, waveIndex));

            setGameState(GameState.StartWave);

        }
    }
}
