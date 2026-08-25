using UnityEngine;

public class ShootingEnemy : MeleeEnemy
{
    [SerializeField] private GameObject Bullet;
    [SerializeField] private Transform nozzle;
    [SerializeField] private float ShootingSpeed = 10f;
    public override void OnHitFrame()
    {
        if (Target != null)
        {
            GameObject spawnBullet = Instantiate(Bullet, nozzle.position, Quaternion.identity);

            Vector2 Direction = ((Vector2)Target.position - (Vector2)nozzle.position).normalized; ;
            if (spawnBullet.TryGetComponent(out Bulllet bullet))
            {
                bullet.Damage = Damage;
                bullet.rb.linearVelocity = Direction * ShootingSpeed;
            }

        }
    }
}
