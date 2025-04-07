using UnityEngine;

namespace Assets.JSGAONA.Unidad1.Scripts
{
    public class Checkpoint : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerController player = other.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.SetCheckpoint(transform.position);
                }
            }
        }
    }
}