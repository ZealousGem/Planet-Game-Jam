using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct EnemyWeights
{
    private GameObject Enemy;

    private int weight;

    public EnemyWeights(GameObject _Enemy, int _weight)
    {
        Enemy = _Enemy;
        weight = _weight;
    }

    public GameObject getEnemy()
    {
        return Enemy;
    }

    public int getWeight()
    {
        return weight;
    }
}

public class WaveManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("SettlementCoordinates")]
    [SerializeField] private List<Transform> currentSettlements;

    [Header("Radius Properties")]
    [SerializeField] private float MinX = -10, MaxX = 70;
    [SerializeField] private float MinY = -30, MaxY = 40;

    [Header("Spawncooldown")]
    [SerializeField] private float SpawnCooldown = 1f;

    [Header("Enenimies that could spawn")]
    [SerializeField] private List<EnemyWave> EnemyPrefabs; // enemies types wavemanager will spawn 

    [Header("Layermask stuff to prevent overlap")]
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private float radius = 5f;

    // private variables
    private int EnemeyWaveAmount = 10;
    private List<EnemyWeights> Enemies = new List<EnemyWeights>();
    private List<GameObject> CurrentEnemies = new List<GameObject>();
    private List<GameObject> EnemiesToSpawn = new List<GameObject>();
    private GameState currentGameState = GameState.Start;
    private int ChangeWaveTypeIndex = 0;
    private int EnemyTypeIndex = 1;
    private float counter = 0;

    private void Awake() => initialiseFirstWave();

    void OnEnable()
    {
        EventBus.Subscribe<GameStateEvent>(RetrieveData);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe<GameStateEvent>(RetrieveData);
    }

    private void RetrieveData(GameStateEvent data)
    {

        switch (data)
        {
            case WaveStateEvent wave:
                EnemeyWaveAmount = wave.WaveAmount;
                Debug.Log(EnemeyWaveAmount);
                StartCoroutine(StartWave()); break;
            default: currentGameState = data.gameState; break;
        }

    }

    private void initialiseFirstWave()
    {
        CurrentEnemies = EnemyPrefabs[0].Enemies;
        ChangeWaveTypeIndex = 3;
        CalEnemyWieghts();
    }

    private IEnumerator StartWave()
    {
        float currentTime = 5f;
        int lastLoggedSecond = -1;

        yield return new WaitForSeconds(0.3f);
        EventBus.Act(new PopUpEvent(UIevents.WaveStart, "Starting Wave"));
        yield return new WaitForSeconds(0.8f);

        while (currentTime >= 1)
        {

            currentTime -= Time.deltaTime;

            int currentSecond = Mathf.CeilToInt(currentTime);

            if (currentSecond != lastLoggedSecond)
            {
                lastLoggedSecond = currentSecond;
                EventBus.Act(new PopUpEvent(UIevents.WaveStart, lastLoggedSecond.ToString()));
                // string annouceText = " New Wave Starting in " + currentTime.ToString("F0");
                // Debug.Log(annouceText);
            }

            yield return null;
        }

        if (GameState.Start != currentGameState)
        {
            ChangeWave();
        }

        yield return new WaitForSeconds(1f);
        EventBus.Act(new PopUpEvent(UIevents.WaveStart, "Protect The Bases"));
        yield return new WaitForSeconds(0.8f);
        EventBus.Act(new PopUpEvent(UIevents.WaveStart, ""));

        currentGameState = GameState.Ongoing;

        // call event here 
    }
    private void CalEnemyWieghts() // uses proablity wight sysystem to  from the enemy prefab to create unprediable waves 
    {
        Enemies.Clear();
        EnemiesToSpawn.Clear();
        int totalWieght = 0;

        if (CurrentEnemies == null || CurrentEnemies.Count == 0)
        {
            Debug.LogError("CurrentEnemies list is empty! Cannot calculate weights.");
            return;
        }
        foreach (GameObject enemy in CurrentEnemies) // adds a weight to each enemy type
        {
            int randomWeight = UnityEngine.Random.Range(1, 6);
            EnemyWeights en = new EnemyWeights(enemy, randomWeight);
            Enemies.Add(en);
            totalWieght += randomWeight;
        }

        int RandomEnemyAmount = UnityEngine.Random.Range(2, 8); // gets a random to determine amount of enemies in the wave
        for (int i = 0; i < RandomEnemyAmount; i++)
        {
            int RandomWeight = UnityEngine.Random.Range(0, totalWieght); // randomly genereates required weight 
            foreach (EnemyWeights e in Enemies) // if enemies weight is highewr than the required weight, the enemy will spawn in the next wave
            {
                RandomWeight -= e.getWeight();
                if (RandomWeight < 0)
                {
                    GameObject gameObject = e.getEnemy();
                    EnemiesToSpawn.Add(gameObject);
                    break;
                }
            }
        }

    }

    private void ChangeWave() // changes the wave if enemies have reached max amount of enemies killed 
    {
        if (SpawnCooldown > 0.2) SpawnCooldown -= 0.4f;
        else SpawnCooldown = 0.2f;

        ChangeWavetype();
        CalEnemyWieghts();
    }

    void ChangeWavetype() // introduces new enemy types to enchance diffuculty.
    {
        if (EnemyTypeIndex == ChangeWaveTypeIndex && EnemyTypeIndex < EnemyPrefabs.Count) // if current equals wave type change wave, the new enemy types will be added 
        {
            CurrentEnemies = EnemyPrefabs[EnemyTypeIndex].Enemies;
            int rand = UnityEngine.Random.Range(ChangeWaveTypeIndex + 1, ChangeWaveTypeIndex + 4);

            EnemyTypeIndex++;
            ChangeWaveTypeIndex = rand; // random sets wave type variable to create unprediability when enemy types are added 
        }

    }

    // Update is called once per frame
    private void Update()
    {
        if (currentGameState != GameState.Ongoing) return;

        SpawningEnemies();

    }

    private void SpawningEnemies()
    {
        if (EnemeyWaveAmount <= 0)
        {
            currentGameState = GameState.EndedWave;
            Debug.Log("Wave Finished Spawning.");
            return;
        }

        counter += Time.deltaTime;
        if (counter < SpawnCooldown) return;

        if (EnemiesToSpawn == null) return;

        counter = 0f;
        EnemeyWaveAmount--;

        GameObject Enemy = EnemiesToSpawn[Random.Range(0, EnemiesToSpawn.Count)];

        GameObject spawnedEnemy = FindSpawnCoordinatesforEnemy(Enemy);
        if (spawnedEnemy == null) return;

        Transform nearestSettlement = NearestSettlement(spawnedEnemy.transform.position);

        if (nearestSettlement != null && spawnedEnemy.TryGetComponent(out BaseEnemy enemy))
        {
            enemy.Initialise(nearestSettlement);
        }

    }

    private Transform NearestSettlement(Vector3 position)
    {
        Transform nearest = null;
        float shortestDistanceSqr = Mathf.Infinity;

        for (int i = 0; i < currentSettlements.Count; i++)
        {
            if (currentSettlements[i] == null) continue;

            float distanceSqr = (position - currentSettlements[i].position).sqrMagnitude;
            if (distanceSqr < shortestDistanceSqr)
            {
                shortestDistanceSqr = distanceSqr;
                nearest = currentSettlements[i];
            }
        }

        return nearest;
    }

    private GameObject FindSpawnCoordinatesforEnemy(GameObject enemy)
    {
        bool canSpawn = false;
        Vector3 SpawnPos = new Vector3();
        int safetyNet = 0;

        while (!canSpawn && safetyNet < 50)
        {
            float SpawnPointX = Random.Range(MinX, MaxX);
            float SpawnPointY = Random.Range(MinY, MaxY);

            SpawnPos = new Vector3(SpawnPointX, SpawnPointY, 0f);
            canSpawn = !Physics2D.OverlapCircle(SpawnPos, radius, targetLayers);
            safetyNet++;
        }

        if (!canSpawn)
        {
            Debug.Log("could not find suitable spaw point");
            return null;
        }

        return Instantiate(enemy, SpawnPos, Quaternion.identity);
    }

}




