using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPool : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Weapon")]
    [SerializeField] private GameObject Missile;
    private Queue<GameObject> bullets;

    private void Awake()
    {
        bullets = new Queue<GameObject>();
    }

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

    // Update is called once per frame
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        bullets.Enqueue(obj);
    }
}
