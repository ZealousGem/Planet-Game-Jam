using UnityEngine;

[CreateAssetMenu(fileName = "RestoreATowerHealthUpgrade", menuName = "Upgrades/RestoreATowerHealthUpgrade")]
public class RestoreAtowerHealthUpgrade : BaseUpgrade
{
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private float detectionRadius = 5f;
    public override void Effect()
    {
        Transform position = UIInput.Instance.transform;
        Collider2D[] settlements = Physics2D.OverlapCircleAll(position.position, detectionRadius, targetLayerMask);

        if (settlements.Length <= 0) return;

        float LowestHealth = 0;

        string TowerName = "";

        for (int i = 0; i < settlements.Length; i++)
        {
            if (!settlements[i].TryGetComponent(out Settlement settlement))
            {
                continue;
            }

            else
            {
                var (Tower, health) = CheckHealth(settlement, LowestHealth, TowerName);

                TowerName = Tower;
                LowestHealth = health;
            }

        }
        // Debug.Log(TowerName + " Healed");
        EventBus.Act(new HealATowerEvent(TowerName));
    }

    private (string, float) CheckHealth(Settlement settlement, float currentHealth, string TowerName)
    {

        if (currentHealth == 0)
        {
            currentHealth = settlement.getCurrentHealth();
            TowerName = settlement.gameObject.transform.name;
        }

        else if (currentHealth > settlement.getCurrentHealth())
        {
            currentHealth = settlement.getCurrentHealth();
            TowerName = settlement.gameObject.transform.name;
        }

        return (TowerName, currentHealth);
    }
}
