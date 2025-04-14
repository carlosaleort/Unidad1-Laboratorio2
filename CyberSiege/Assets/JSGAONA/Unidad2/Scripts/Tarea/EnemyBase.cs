using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Detección")]
    public float detectionRadius = 10f;
    public float visionAngle = 60f;
    public Transform player;
    public LayerMask obstacleMask;
    public LayerMask playerMask;

    protected bool playerDetected;

    protected virtual void Update()
    {
        playerDetected = CheckVision();
    }

    protected bool CheckVision()
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer < detectionRadius)
        {
            float angle = Vector3.Angle(transform.forward, dirToPlayer);
            if (angle < visionAngle / 2f)
            {
                if (!Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, obstacleMask))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Vector3 rightLimit = Quaternion.Euler(0, visionAngle / 2, 0) * transform.forward * detectionRadius;
        Vector3 leftLimit = Quaternion.Euler(0, -visionAngle / 2, 0) * transform.forward * detectionRadius;
        Gizmos.DrawLine(transform.position, transform.position + rightLimit);
        Gizmos.DrawLine(transform.position, transform.position + leftLimit);
    }
}
