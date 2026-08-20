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
    // bullet life span once active
    private GunPool pool;

    // instantiaes pool from the Gunpool class
    public void SetPool(GunPool poolRef)
    {
        pool = poolRef;
    }

    // uses invoke to player life span if object has no collided with anything
    private void OnEnable()
    {
        Invoke(nameof(ReturnToPool), lifeTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    // bullet will be deactived if triggered or colliding an enemy 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BaseEnemy enemy))
        {
            // Deal damage here if needed
            enemy.DamageEnemy(Damage);
            ReturnToPool();
        }
    }

    // readds the bullet into the queue and deactivates
    private void ReturnToPool()
    {
        if (pool != null)
        {
            pool.ReturnObject(gameObject);
        }
        else
        {
            Destroy(gameObject); // Fallback safety if spawned without a pool
        }
    }
}
