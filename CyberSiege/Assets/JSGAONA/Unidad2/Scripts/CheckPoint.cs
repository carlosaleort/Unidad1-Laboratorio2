using UnityEngine;

namespace Assets.JSGAONA.Unidad2.Scripts {

    // Se emplea este script para gestionar el punto de control del personaje
    public class CheckPoint : MonoBehaviour {
        
        // Metodo de llamada de Unity, se activa al momento que una colision ingresa a esta
        // marcada como isTrigger
        private void OnTriggerEnter(Collider other) {
            // Se valida que el punto de control no se haya activado y que la colision sea un player
            if(other.CompareTag("Player")) {
                // Se valida que la instancia singleton este habilitada
                if(CheckPointManager.CheckPointInstance != null) {
                    CheckPointManager.CheckPointInstance.UpdateCheckPoint(transform.position);
                }
                // Se desabilita el script, con la finalidad que deje de activar el trigger
                // Cada vez que ingresa al jugador, optimizando la llamada al disparador
                enabled = false;
            }
        }
    }
}