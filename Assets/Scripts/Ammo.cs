using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] private float lifeTime = 0.8f;
    private GunPool pool;

    public void SetPool(GunPool poolRef)
    {
        pool = poolRef;
    }

    private void OnEnable()
    {
        Invoke(nameof(ReturnToPool), lifeTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Deal damage here if needed
            ReturnToPool();
        }
    }

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
