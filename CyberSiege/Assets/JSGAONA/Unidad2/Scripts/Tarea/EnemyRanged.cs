using UnityEngine;

public class EnemyRanged : EnemyBase
{
    public float fireRate = 1f;
    public GameObject projectilePrefab;
    public Transform firePoint;

    private float fireCooldown;

    private void Update()
    {
        base.Update();

        if (playerDetected)
        {
            if (fireCooldown <= 0f)
            {
                Shoot();
                fireCooldown = fireRate;
            }
        }

        fireCooldown -= Time.deltaTime;
    }

    private void Shoot()
    {
        GameObject projectile = ObjectPool.Instance.GetFromPool(projectilePrefab);
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = Quaternion.LookRotation(player.position - firePoint.position);
        projectile.SetActive(true);
    }
}
