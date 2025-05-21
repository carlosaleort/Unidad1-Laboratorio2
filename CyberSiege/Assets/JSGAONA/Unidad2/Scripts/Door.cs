using Jsgaona;
using UnityEngine;

namespace Assets.JSGAONA.Unidad3.Scripts {

    // Se emplea este script para gestionar el acceso a las puertas
    public class Door : MonoBehaviour {
        
        // Identificador unico de la puerta
        [SerializeField] private int id;

        // Referencia del id de la peurta con la que conecta
        [SerializeField] private int conexionId;

        // Id de la escena a cargar
        [SerializeField] private int idScene;

        // Propiedad
        public int ConexionId => conexionId;


        // Metodo de llamada de Unity, se llama cuando una colision ingresa a esta marcada como
        // isTrigger
        private void OnTriggerEnter(Collider other) {
            if(other.CompareTag("Player")) {
                // Evita que el player se destruya al cambiar de escena
                DontDestroyOnLoad(other.gameObject);

                // Se asigna el valor de la siguiente puerta donde debe aparecer el pj
                // y se procede a cargar la siguiente escena
                SceneLoadingManager.SceneInstance.DoorId = conexionId;
                SceneLoadingManager.SceneInstance.LoadGameScene(idScene);
            }
        }
    }
}