using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settlement : MonoBehaviour
{

    [Header("Health Properties")]
    [SerializeField] private float Health;
    [SerializeField] private Image HealthBar;
    [SerializeField] private TMP_Text Num;

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

    private void OnEnable()
    {
        EventBus.Subscribe<HealATowerEvent>(RetrieveData);
        EventBus.Subscribe<HealALLTowerEvent>(RetrieveData);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<HealATowerEvent>(RetrieveData);
        EventBus.Unsubscribe<HealALLTowerEvent>(RetrieveData);
    }

    public void AddNumber(int num)
    {
        Num.text = num.ToString();
    }

    private void RetrieveData(HealALLTowerEvent events)
    {

        Health = maxHealth;
        HealthBarLogic();
        DamagePopUp.CreatePopUp(PopUpUI, transform, "Healed");
        Debug.Log("Upgrades work");

    }

    private void RetrieveData(HealATowerEvent events)
    {
        if (events.Name == transform.name)
        {
            Debug.Log("Healed " + transform.name);
            Health = maxHealth; HealthBarLogic();
            DamagePopUp.CreatePopUp(PopUpUI, transform, "Healed");
        }
    }

    public void DamageTower(float Damage)
    {
        if (isDead) return;
        Health -= Damage;
        SoundPlayer.PlaySound("hit");

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

    public float getCurrentHealth()
    {
        return Health;
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

        SoundPlayer.PlaySound("Explosion");
        Destroy(gameObject);
    }
}
