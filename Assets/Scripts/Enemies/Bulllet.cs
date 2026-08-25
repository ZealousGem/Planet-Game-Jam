using UnityEngine;

public class Bulllet : MonoBehaviour
{
    [SerializeField] private float timer = 1f;
    [HideInInspector] public float Damage = 1;
    private float currentTimer = 0f;
    public Rigidbody2D rb { get; private set; }

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Settlement settlement))
        {
            // EventBus.Act(new DamageHero(Damage));
            settlement.DamageTower(Damage);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        currentTimer += Time.deltaTime; // once counter has been reached bomb will despawn to save peformance, this is only done if the turret or enemy miises their shot 

        if (currentTimer >= timer)
        {
            Destroy(gameObject);
        }
    }
}
