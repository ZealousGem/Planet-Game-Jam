using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPool : MonoBehaviour
{
    /// <summary>
    /// 
    /// 
    /// Pooling System to sapwn bullets cleanly instead of destroy them constantly
    /// 
    /// 
    /// </summary>


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Weapon")]
    [SerializeField] private GameObject Missile;
    [SerializeField] private float Damage = 0;
    private Queue<GameObject> bullets;

    // creates a new queue at begining of scene so variable isnt null during runtime
    private void Awake()
    {
        bullets = new Queue<GameObject>();
    }

    void OnEnable()
    {
        EventBus.Subscribe<UpgradeEvent>(RetrieveData);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe<UpgradeEvent>(RetrieveData);
    }

    public float getBonusDamage()
    {
        return Damage;
    }

    private void RetrieveData(UpgradeEvent events)
    {
        switch (events)
        {
            case DamageEvent changeDamage: Damage += changeDamage.Damage; break;
        }
    }

    // activates any deactived bullets from queue  or instaniate bullet if queue is empty

    public GameObject getObj(Vector3 transform, Quaternion rotation)
    {
        GameObject obj;
        if (bullets.Count > 0)
        {
            obj = bullets.Dequeue();
            obj.transform.position = transform;
            obj.transform.rotation = rotation;

            obj.SetActive(true);
            return obj;
        }

        else
        {
            obj = Instantiate(Missile, transform, rotation);
        }

        if (obj.TryGetComponent<Ammo>(out var bullet))
        {
            bullet.SetPool(this);
        }

        return obj;
    }

    /// <summary>
    /// 
    /// empties pool once weapon or bullet is changed based on the upgrades 
    /// 
    /// </summary>
    public void ClearPool()
    {
        while (bullets.Count > 0)
        {
            GameObject obj = bullets.Dequeue();
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        bullets.Clear();
    }

    // reque object once bullet has despawned
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        bullets.Enqueue(obj);
    }
}
