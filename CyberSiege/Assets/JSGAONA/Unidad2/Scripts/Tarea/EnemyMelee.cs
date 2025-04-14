using UnityEngine;

public class EnemyMelee : EnemyBase
{
    public float attackRange = 2f;
    public float speed = 3f;
    public int damage = 10;

    private void FixedUpdate()
    {
        if (playerDetected)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist > attackRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
            else
            {
                AttackPlayer();
            }
        }
    }

    private void AttackPlayer()
    {
        // Animaci�n de ataque + l�gica de da�o
        Debug.Log("Melee attack!");
    }
}
