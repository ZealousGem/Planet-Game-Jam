using UnityEngine;

public class Ammo : MonoBehaviour
{
    /// <summary>
    /// 
    /// 
    /// Handles Bullets lifespan and collision with any enemy objects 
    /// 
    /// 
    /// </summary>
    [SerializeField] private float lifeTime = 0.8f;
    [SerializeField] private float Damage = 45f;

    private float currentDamage;
    private Collider2D collider2D;
    // bullet life span once active
    private GunPool pool;

    // instantiaes pool from the Gunpool class
    public void SetPool(GunPool poolRef)
    {
        pool = poolRef;
    }

    void Awake()
    {
        collider2D = GetComponent<Collider2D>();
    }

    // uses invoke to player life span if object has no collided with anything
    private void OnEnable()
    {
        float bonus = (pool != null) ? pool.getBonusDamage() : 0f;
        currentDamage = Damage + bonus;
        Invoke(nameof(ReturnToPool), lifeTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    public void setDamage(float _Damage)
    {
        Damage += _Damage;
    }

    // bullet will be deactived if triggered or colliding an enemy 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BaseEnemy enemy))
        {
            // Deal damage here if needed
            enemy.DamageEnemy(currentDamage);
            // ReturnToPool();
        }
    }

    // readds the bullet into the queue and deactivates
    private void ReturnToPool()
    {
        if (pool != null)
        {
            collider2D.enabled = false;
            pool.ReturnObject(gameObject);
        }
        else
        {
            Destroy(gameObject); // Fallback safety if spawned without a pool
        }
    }

    public void TurnOnCollider()
    {
        if (collider2D != null) collider2D.enabled = true;
    }

}
