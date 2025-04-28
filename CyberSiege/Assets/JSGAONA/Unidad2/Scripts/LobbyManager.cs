using Jsgaona;
using UnityEngine;

namespace Assets.JSGAONA.Unidad2.Scripts {

    // Se emplea este script para gestionar el menu de lobby del juego
    public class LobbyManager : MonoBehaviour {

        // Indice de escena que se ha selecionado
        [SerializeField] private int indexScene = 0;

        // Referencia del prefab del cargador de escenas
        [SerializeField] private GameObject loadingSceneManager;


        // Metodo de llamada de Unity, se llamada una unica vez al iniciar el app despues de Awake
        // Se inicializa las variables principales de modificacion
        protected virtual void Start() {
            // Si no existe en escena una referencia valida del administrador de escenas, se lo crea
            if(FindObjectOfType<SceneLoadingManager>() == null){
                Instantiate(loadingSceneManager, Vector3.zero, Quaternion.identity);
            }

            // Se busca la referencia de la escena instanciada
            if(SceneLoadingManager.SceneInstance != null){
                // Se valida si la escena cargada es la del lobby
                if(SceneLoadingManager.SceneInstance.GetActiveScene() == 0) {
                    // Se pregunta si existe la referencia del jugador para destruirlo
                    // Debido a que en el lobby no debe existir el personaje jugable
                    GameObject playerDetected = GameObject.FindGameObjectWithTag("Player");
                    if(playerDetected != null) Destroy(playerDetected);
                }
            }
        }


        // Metodo que permite dar inicio al juego
        public void StartGame(){
            SceneLoadingManager.SceneInstance.LoadGameScene(indexScene);
        }


        // Metodo que permite cerrar el juego
        public void ExitGame(){
            Application.Quit();
        }
    }
}