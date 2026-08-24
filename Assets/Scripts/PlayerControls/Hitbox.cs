using UnityEngine;

public class Hitbox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void TurnOnCollider()
    {
        GetComponentInParent<Ammo>()?.TurnOnCollider();
    }
}
