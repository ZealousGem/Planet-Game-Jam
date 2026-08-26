using UnityEngine;

[CreateAssetMenu(fileName = "SpawnAnotherTowerUpgrade", menuName = "Upgrades/SpawnAnotherTowerUpgrade")]
public class SpawnAnotherTowerUpgrade : BaseUpgrade
{
    [SerializeField] private GameObject SettlementPrefab;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float spawnClearanceRadius = 2f;
    [SerializeField] private float MinX = -10, MaxX = 70;
    [SerializeField] private float MinY = -30, MaxY = 40;
    public override void Effect()
    {
        bool canSpawn = false;
        Vector3 SpawnPos = new Vector3();
        int safetyNet = 0;

        while (!canSpawn && safetyNet < 50)
        {
            //  float padding = 5f;

            float SpawnPointX = Random.Range(MinX / 5, MaxX / 5);
            float SpawnPointY = Random.Range(MinY / 5, MaxY / 5);

            SpawnPos = new Vector3(SpawnPointX, SpawnPointY, 0f);
            canSpawn = !Physics2D.OverlapCircle(SpawnPos, spawnClearanceRadius, targetLayerMask);
            safetyNet++;
        }

        if (!canSpawn)
        {
            Debug.Log("could not find suitable spaw point");
            return;
        }

        GameObject obj = Instantiate(SettlementPrefab, SpawnPos, Quaternion.identity);

        if (obj.TryGetComponent(out Settlement settlement))
        {
            Collider2D[] settlements = Physics2D.OverlapCircleAll(obj.transform.position, detectionRadius, targetLayerMask);
            int Settlementnum = settlements.Length;

            settlement.AddNumber(Settlementnum);
        }

        EventBus.Act(new SpawnTowerEvent(1, obj));
    }
}
