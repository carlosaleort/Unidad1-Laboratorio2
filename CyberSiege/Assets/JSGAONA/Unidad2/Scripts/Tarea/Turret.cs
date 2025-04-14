using UnityEngine;

public class Turret : EnemyBase
{
    public float fireRate = 1f;
    public Transform firePoint;
    public GameObject projectilePrefab;

    private float cooldown;

    protected override void Update()
    {
        base.Update();

        if (playerDetected)
        {
            if (cooldown <= 0f)
            {
                Shoot();
                cooldown = fireRate;
            }
        }

        cooldown -= Time.deltaTime;
    }

    private void Shoot()
    {
        GameObject projectile = ObjectPool.Instance.GetFromPool(projectilePrefab);
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = Quaternion.LookRotation(player.position - firePoint.position);
        projectile.SetActive(true);
    }
}
