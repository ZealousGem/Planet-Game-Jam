using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Settlement : MonoBehaviour
{

    [SerializeField] private float Health;
    [SerializeField] private Image HealthBar;
    [SerializeField] private GameObject debris;
    private bool isDead = false;
    private float maxHealth;

    private void Awake()=> maxHealth = Health; 
    public void DamageTower(float Damage)
    {
        if (isDead) return;
        Health -= Damage;
  
        HealthBar.fillAmount = Health / maxHealth;

        if (Health <= 0)
        {
            isDead = true;

            Health = 0;

            Transform Parent = HealthBar.gameObject.transform.parent;
            Parent.gameObject.SetActive(false);

            EventBus.Act(new SettlementCounterEvent(1));

            StartCoroutine(BlowUpSettlement());
        }
    }

    private IEnumerator BlowUpSettlement()
    {
        yield return null;
        Destroy(gameObject);
    }
}
