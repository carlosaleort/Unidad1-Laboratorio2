using UnityEngine;

namespace Assets.JSGAONA.Unidad2.Scripts {

    // Script demo que permite realizar test de danio
    public class Sword : MonoBehaviour {

        // Variables visibles desde el inspector de Unity
        [SerializeField] private int damage = 10;



        // Metodo de llamada de Unity, se llama al momento que una colision ingresa a esta
        private void OnTriggerEnter(Collider other) {
            // El test solo hace danio a el jugador
            if(other.CompareTag("Player")) {
                // Se valida que la colision que ingresa sea un jugador con el script adjunto
                if(other.TryGetComponent(out PlayerCombatController player)) player.TakeDamage(damage);
            }
        }
    }
}