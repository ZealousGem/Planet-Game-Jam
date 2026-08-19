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
    private List<EnemyWeights> Enemies = new List<EnemyWeights>();
    private List<GameObject> CurrentEnemies = new List<GameObject>();
    private List<GameObject> EnemiesToSpawn = new List<GameObject>();
    private GameState currentGameState = GameState.Start;
    private int EnemeyWaveAmount = 0;
    private int ChangeWaveTypeIndex = 0;
    private int waveIndex = 0;
    private float counter = 0;
    private LayerMask layerMask;
    private Collider[] colliders;

    private void Awake() => initialiseFirstWave();
    private void initialiseFirstWave()
    {
        CurrentEnemies = EnemyPrefabs[0].Enemies;
        waveIndex++;
        ChangeWaveTypeIndex = 3;
        CalEnemyWieghts();
    }

    private IEnumerator StartWave()
    {
        float currentTime = 5f;
        while (currentTime >= 1)
        {

            currentTime -= Time.deltaTime;


            string annouceText = " New Wave Starting in " + currentTime.ToString("F0");
            Debug.Log(annouceText);


            yield return null;
        }
        // EndGameEvent ui = new EndGameEvent(StatsChange.EnemieLeft, maxbotKilled);
        // EventBus.Act(ui);
        // WaveAnnouncer.text = "";
        // SpawnEnemiesCounter = maxbotKilled;
        // isFound = true;
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

    void ChangeWave(GameState gameState) // changes the wave if enemies have reached max amount of enemies killed 
    {
        currentGameState = gameState;

        if (SpawnCooldown > 0.2) SpawnCooldown -= 0.4f;
        else SpawnCooldown = 0.2f;

        ChangeWavetype();
        CalEnemyWieghts();
    }

    void ChangeWavetype() // introduces new enemy types to enchance diffuculty.
    {
        if (waveIndex == ChangeWaveTypeIndex && waveIndex < EnemyPrefabs.Count) // if current equals wave type change wave, the new enemy types will be added 
        {
            CurrentEnemies = EnemyPrefabs[waveIndex].Enemies;
            int rand = UnityEngine.Random.Range(ChangeWaveTypeIndex + 1, ChangeWaveTypeIndex + 4);
            ChangeWaveTypeIndex = rand; // random sets wave type variable to create unprediability when enemy types are added 
        }

        waveIndex++;

    }

    // Update is called once per frame
    void Update()
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
        if (counter <= SpawnCooldown) return;

        int random = UnityEngine.Random.Range(0, EnemiesToSpawn.Count);
        GameObject Enemy = EnemiesToSpawn[random];

        FindSpawnCoordinatesforEnemy(Enemy);

        Transform EnemySettlementTarget = currentSettlements[0];

        for (int i = 0; i < currentSettlements.Count; i++)
        {
            float distanceA = Vector3.Distance(Enemy.transform.position, EnemySettlementTarget.position);
            float distanceB = Vector3.Distance(Enemy.transform.position, currentSettlements[i].position);

            if (distanceA > distanceB)
            {
                EnemySettlementTarget = currentSettlements[i];
            }

        }

        if (Enemy.TryGetComponent(out BaseEnemy enemy))
        {
            enemy.InstatiateTarget(EnemySettlementTarget);
        }

        counter = 0f;

    }

    private void FindSpawnCoordinatesforEnemy(GameObject enemy)
    {
        bool canSpawn = false;
        Vector3 SpawnPos = new Vector3();
        int safetyNet = 0;

        while (!canSpawn)
        {
            float SpawnPointX = Random.Range(MaxX, MinX);
            float SpawnPointY = Random.Range(MinY, MaxY);

            SpawnPos = new Vector3(SpawnPointX, SpawnPointY, 0);
            canSpawn = PreventOverlap(SpawnPos);

            safetyNet++;

            if (safetyNet > 50)
            {
                Debug.Log("could not find suitable spaw point");
                break;
            }
        }

        Instantiate(enemy, SpawnPos, Quaternion.identity);
    }

    private bool PreventOverlap(Vector3 SpawnPos)
    {
        colliders = Physics.OverlapSphere(transform.position, radius, layerMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Vector3 CentrePoint = colliders[i].bounds.center;
            float width = colliders[i].bounds.extents.x;
            float heigth = colliders[i].bounds.extents.y;

            float leftExtent = CentrePoint.x - width;
            float rightExtent = CentrePoint.x + width;
            float lowerExtent = CentrePoint.y - heigth;
            float upperExtent = CentrePoint.y + heigth;

            if (SpawnPos.x >= leftExtent && SpawnPos.x <= rightExtent)
            {
                if (SpawnPos.z >= lowerExtent && SpawnPos.z >= upperExtent)
                {
                    return false;
                }
            }

        }

        return true;

    }

}




