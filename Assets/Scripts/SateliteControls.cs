using UnityEngine;
using UnityEngine.InputSystem;

public class SateliteControls : PlayerMovement
{
    [Header("Cooldowns")]
    [SerializeField] private float recoilSpeed = 1f;
    [SerializeField] private float reloadSpeed = 3f;

    [Header("Weapon")]
    [SerializeField] private Transform Nozzle;


    [Header("Pool")]
    [SerializeField] private GunPool pool;
    private bool reloading = false;

    protected override void ShootBullet(InputAction.CallbackContext value)
    {
        Debug.Log("firing");

        if (reloading) return;

        Vector3 spawnPosition = Nozzle.position;
        spawnPosition.z = 0f;

        pool.getObj(spawnPosition, transform.rotation);


    }
}
