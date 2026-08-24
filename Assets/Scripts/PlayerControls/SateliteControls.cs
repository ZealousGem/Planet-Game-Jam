using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SateliteControls : PlayerMovement
{

    /// <summary>
    /// 
    /// Script handles shooting controls 
    /// 
    /// </summary>
    [Header("Cooldowns")]
    [SerializeField] private float recoilSpeed = 1f;
    [SerializeField] private float reloadSpeed = 3f;

    [Header("Weapon")]
    [SerializeField] private Transform Nozzle;

    [Header("Pool")]
    [SerializeField] private GunPool pool;

    [Header("GunAnimation")]
    [SerializeField] private Animator animator;
    private bool reloading = false;

    private int Maxbulletcount = 15;

    private int bulletCounter;

    // binds current count to max at begining of scene
    protected override void Awake()
    {
        base.Awake();
        bulletCounter = Maxbulletcount;
    }

    private void Start() => EventBus.Act(new NumUIEvent(UIevents.Ammo, bulletCounter));

    // uses shooting bullet input uses pooling system to spawn or relocate object
    protected override void ShootBullet(InputAction.CallbackContext value)
    {
        if (reloading) return;

        Vector3 spawnPosition = Nozzle.position;
        spawnPosition.z = 0f;

        if (animator != null) animator.SetBool("Attack", true);

        pool.getObj(spawnPosition, transform.rotation);

        Recoil();

    }

    // cooldown method that also decrease bullets overtime if bullet counter is under then function will force player to reload
    private void Recoil()
    {
        bulletCounter--;

        reloading = true;

        if (bulletCounter <= 0)
        {

            StartCoroutine(ReloadSpeed(reloadSpeed, Maxbulletcount));

        }

        else
        {
            StartCoroutine(ReloadSpeed(recoilSpeed));

        }

    }

    // reload courtine stop player from shooting for a couple of seconds
    private IEnumerator ReloadSpeed(float counter)
    {
        yield return new WaitForSeconds(0.1f);
        if (animator != null) animator.SetBool("Attack", false);
        float cur = 0f;
        EventBus.Act(new NumUIEvent(UIevents.Ammo, bulletCounter));

        while (cur < counter)
        {
            cur += Time.deltaTime;
            yield return null;
        }

        reloading = false;
    }

    private IEnumerator ReloadSpeed(float counter, int bullets)
    {
        yield return new WaitForSeconds(0.1f);
        if (animator != null) animator.SetBool("Attack", false);

        float cur = 0f;
        //   Debug.Log("reloading");
        EventBus.Act(new NumUIEvent(UIevents.NoAmmo, bulletCounter));

        while (cur < counter)
        {
            cur += Time.deltaTime;
            yield return null;
        }
        bulletCounter = bullets;
        EventBus.Act(new NumUIEvent(UIevents.Ammo, bulletCounter));
        reloading = false;
        // Debug.Log("ready");
    }
}
