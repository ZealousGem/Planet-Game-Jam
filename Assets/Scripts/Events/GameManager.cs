using UnityEngine;

public enum GameState { Start, Ongoing, Success, Failure, EndedWave, StartWave, Upgrades, Paused }

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
        EventBus.Subscribe<EnemiesKilledEvent>(RetrieveData);
        EventBus.Subscribe<SettlementCounterEvent>(RetrieveData);
        EventBus.Subscribe<SpawnTowerEvent>(RetrieveData);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<GameManagerEvent>(retrieveData);
        EventBus.Unsubscribe<EnemiesKilledEvent>(RetrieveData);
        EventBus.Unsubscribe<SettlementCounterEvent>(RetrieveData);
        EventBus.Unsubscribe<SpawnTowerEvent>(RetrieveData);
    }

    private void RetrieveData(SpawnTowerEvent data)
    {
        TowerAmount = TowerAmount + data.counter;
        EventBus.Act(new NumUIEvent(UIevents.Bases, TowerAmount));
    }

    private void retrieveData(GameManagerEvent data)
    {
        setGameState(data.gameState);
    }

    private void RetrieveData(SettlementCounterEvent data) => TowerCounter();

    private void RetrieveData(EnemiesKilledEvent data) => IncreaseBotKilledCount(data.num);

    void Start()
    {
        EventBus.Act(new NumUIEvent(UIevents.Bases, TowerAmount));
        EventBus.Act(new NumUIEvent(UIevents.Waves, waveIndex));

        // setGameState(GameState.StartWave);
        // setGameState(GameState.Ongoing);
    }

    private void TowerCounter()
    {
        TowerAmount--;
        EventBus.Act(new NumUIEvent(UIevents.Bases, TowerAmount));

        if (TowerAmount <= 0)
        {
            setGameState(GameState.Failure);
            return;
        }

        EventBus.Act(new PopUpEvent(UIevents.Tower, "Base Destroyed"));
    }

    private void setGameState(GameState gameState)
    {
        currentGamestate = gameState;

        switch (currentGamestate)
        {
            case GameState.Success: EventBus.Act(new GameStateEvent(GameState.Success)); break;

            case GameState.StartWave:
                EventBus.Act(new WaveStateEvent(GameState.StartWave, TotalEnemiesInWave));
                EventBus.Act(new NumUIEvent(UIevents.EnemiesLeft, TotalEnemiesInWave)); break;

            case GameState.Failure: EventBus.Act(new GameStateEvent(GameState.Failure)); break;
            case GameState.Upgrades: EventBus.Act(new GameStateEvent(GameState.Upgrades)); break;
        }
    }
    private void Update()
    {
        if (currentGamestate == GameState.Ongoing || currentGamestate == GameState.EndedWave) ingameTimer += Time.deltaTime;
    }

    private void IncreaseBotKilledCount(int num)  // increases the enemiy kill count everytime enemy has been killed 
    {
        currentEnemiesKilld += num;
        EventBus.Act(new NumUIEvent(UIevents.EnemiesLeft, TotalEnemiesInWave - currentEnemiesKilld));

        if (currentEnemiesKilld >= TotalEnemiesInWave)
        {
            currentEnemiesKilld = 0;
            int rand = Random.Range(TotalEnemiesInWave + 1, TotalEnemiesInWave + 5);
            TotalEnemiesInWave = rand;

            waveIndex++;
            EventBus.Act(new NumUIEvent(UIevents.Waves, waveIndex));

            setGameState(GameState.Upgrades);

        }
    }
}
