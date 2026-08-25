using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Settlement : MonoBehaviour
{

    [Header("Health Properties")]
    [SerializeField] private float Health;
    [SerializeField] private Image HealthBar;

    [Header("Building")]
    [SerializeField] private SpriteRenderer builind;

    [Header("DebrisPrefab")]
    [SerializeField] private GameObject debris;

    [Header("DamagePopUp")]
    [SerializeField] private GameObject PopUpUI;
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

        HealthBarLogic();

        DamagePopUp.CreatePopUp(PopUpUI, transform, (int)Damage);

        if (Health <= 0)
        {
            isDead = true;

            if (HealthBarEffect != null) StopCoroutine(HealthBarEffect);
            Health = 0;

            Transform Parent = HealthBar.gameObject.transform.parent;
            Parent.gameObject.SetActive(false);

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
        builind.enabled = false;
        debris.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
