using UnityEngine;

namespace Assets.JSGAONA.Unidad2.Scripts {

    // Se emplea este script para gestionar la trampa
    public class Trap : MonoBehaviour {

        // Metodo de llamada de Unity, se activa al momento que una colision ingresa a esta
        // marcada como isTrigger
        private void OnTriggerEnter(Collider other) {
            // Se verifica si un player ingresa a la colision
            if(other.CompareTag("Player")) {
                if(CheckPointManager.CheckPointInstance != null) {
                    CheckPointManager.CheckPointInstance.ReturnCheckPoint();
                }
            }
        }
    }
}