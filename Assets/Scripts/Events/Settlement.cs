using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Settlement : MonoBehaviour
{

    [SerializeField] private float Health;
    [SerializeField] private Image HealthBar;
    [SerializeField] private GameObject debris;
    private Coroutine HealthBarEffect;
    private bool isDead = false;
    private float maxHealth;

    private void Awake()
    {
        maxHealth = Health; if (HealthBar != null &&
        HealthBar.gameObject.activeSelf) HealthBar.gameObject.SetActive(false);
    }

    public void DamageTower(float Damage)
    {
        if (isDead) return;
        Health -= Damage;

        if (!HealthBar.gameObject.activeSelf && Health != 0)
        {
            HealthBarLogic();
        }

        if (Health <= 0)
        {
            isDead = true;
            if (HealthBarEffect != null) StopCoroutine(HealthBarEffect);

            Health = 0;
            HealthBar.gameObject.SetActive(false);

            EventBus.Act(new SettlementCounterEvent(1));

            StartCoroutine(BlowUpSettlement());
        }
    }

    private void HealthBarLogic()
    {
        if (HealthBarEffect == null)
        {
            HealthBarEffect = StartCoroutine(HealthBarTransition());
        }

        else
        {
            StopCoroutine(HealthBarEffect);
            HealthBarEffect = StartCoroutine(HealthBarTransition());
        }
    }

    private IEnumerator HealthBarTransition()
    {
        HealthBar.gameObject.SetActive(true);
        HealthBar.fillAmount = Health / maxHealth;

        yield return new WaitForSeconds(5f);

        HealthBar.gameObject.SetActive(false);
    }

    private IEnumerator BlowUpSettlement()
    {
        yield return null;
        Destroy(gameObject);
    }
}
