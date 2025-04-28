using Jsgaona;
using UnityEngine;
using System.Collections;
using Assets.JSGAONA.Unidad1.Scripts;

namespace Assets.JSGAONA.Unidad2.Scripts {

    // Se utiliza este script para gestionar eficazmente cada punto de control del videojuego
    public class CheckPointManager : MonoBehaviour {

        // Variables visibles desde el inspector de Unity
        [SerializeField] private float timeManageControlPlayer = 1.0f;

        // Punto de control por defecto
        [SerializeField] private Transform defaultCheckPoint;


        // Variables ocultas desde el inspector de Unity
        private bool isReturningCheckpoint = false;
        private Vector3 currentCheckPoint;
        private PlayerController playerController;

        // Se emplea el patron Singleton, para permitir una unica instancia del administrador
        public static CheckPointManager CheckPointInstance { private set; get; }



        // Metodo de llamada de Unity, se llama una unica vez al iniciar el aplicativo
        // Se declaran todos los componentes necesarios para el funcionamiento del script
        private IEnumerator Start(){
            // Esperar un frame
            yield return null;
            // Asegura que solo haya una instancia de esta clase 'Patron de disenio Singleton'
            if(CheckPointInstance == null) {
                CheckPointInstance = this;
                DontDestroyOnLoad(gameObject);
                playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                currentCheckPoint = defaultCheckPoint.position;
            }else{
                Destroy(gameObject);
            }
        }


        // Metodo que permite gestionar el punto de control
        public void ReturnCheckPoint () {
            // Si ya se esta regresando al punto de control no hacer nada
            if(isReturningCheckpoint || currentCheckPoint == null) return;
            // Se valida que exista referencia del controlador del personaje
            if(playerController != null) StartCoroutine(WaitTime());
            // Se valida que exista referencia de la instancia singleton de caragr escena
            if(SceneLoadingManager.SceneInstance != null) SceneLoadingManager.SceneInstance.LoadBlinking();
        }


        // Metodo que permite actualizar el punto de control
        public void UpdateCheckPoint (Vector3 newCheckPoint) {
            currentCheckPoint = newCheckPoint;
        }


        // Corotuina que permite gestionar el controlador del personaje y poner regresarlo al check point
        private IEnumerator WaitTime() {
            isReturningCheckpoint = true;
            playerController.ManagerMovement(false);
            yield return new WaitForSeconds(timeManageControlPlayer);
            // Se valida que exista referencia del controlador del personaje
            playerController.ReturnCheckPoint(currentCheckPoint);
            yield return new WaitForSeconds(timeManageControlPlayer);
            playerController.ManagerMovement(true);
            isReturningCheckpoint = false;
        }
    }
}