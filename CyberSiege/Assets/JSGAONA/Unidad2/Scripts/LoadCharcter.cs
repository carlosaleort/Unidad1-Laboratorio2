using Jsgaona;
using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
using Assets.JSGAONA.Unidad1.Scripts;
using Assets.JSGAONA.Unidad2.Scripts.FMS;
using Assets.JSGAONA.Unidad3.Scripts;

namespace Assets.JSGAONA.Unidad2.Scripts {

    // Se emplea este script para cargar del personaje, desde cualquier escena
    public class LoadCharcter : LobbyManager {
        
        // Referencia del prefab del personaje jugable
        [SerializeField] private GameObject prefabPlayer;

        // Referencia del prefab del canvas
        [SerializeField] private GameObject prefabCanvasUIPlayer;

        // Punto inicial donde aparece el personaje jugable
        [SerializeField] private Transform initialPoint;

        // Referencia de la camara de cinemachine
        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        [Header("All enemies game")]
        // Referencia de todos los enemigos dentro del juego
        [SerializeField] private List<EnemyAI> allEnemies;

        [Header("All doors")]
        [SerializeField] private List<Door> allDoors;

        // Referencia del playerCombat
        private PlayerCombat playerCombat;

    
        // Metodo de llamada de Unity, se llamada una unica vez al iniciar el app despues de Awake
        // Se inicializa las variables principales de modificacion
        protected override void Start() {
            base.Start();

            // Se pregunta si existe la referencia del jugador para destruirlo
            // Debido a que en el lobby no debe existir el personaje jugable
            GameObject playerDetected = GameObject.FindGameObjectWithTag("Player");
            if(playerDetected == null) {
                playerDetected = Instantiate(prefabPlayer, initialPoint.position, initialPoint.rotation);
            }

            // Se crea la suscripcion para cuando el player muere
            if(playerDetected.TryGetComponent(out PlayerCombat playerCombat)) {
                this.playerCombat = playerCombat;
                this.playerCombat.onDead += ResetAllEnemies;
            }

            PlayerHealthView canvas = FindObjectOfType<PlayerHealthView>();
            PlayerController playerControl = playerDetected.GetComponent<PlayerController>();

            // Si no existe en escena una referencia valida del administrador de escenas, se lo crea
            if(canvas == null){
                // Se genera la instancia del canvas
                GameObject canvasGo = Instantiate(prefabCanvasUIPlayer, Vector3.zero, Quaternion.identity);
                canvas = canvasGo.GetComponent<PlayerHealthView>();

                // Se valdia si el personaje dispone del controlador de movimiento para asignar el Joystick
                if(playerControl != null){
                    playerControl.JoystickController = canvas.gameObject.GetComponentInChildren<Joystick>();
                }

                // Se valida si el personaje dispone del controlador de combate
                if(playerDetected.TryGetComponent(out PlayerCombatController playerCombControl)) {
                    canvas.PlayerCombatController = playerCombControl;
                    playerCombControl.PlayerView = canvas;
                }
            }

            playerDetected.transform.rotation = Quaternion.Euler(0, 0, 0);

            // Se establece el objetivo y hacia donde mira la camara
            virtualCamera.Follow = playerDetected.transform;
            virtualCamera.LookAt = playerDetected.transform;
            // virtualCamera.transform.position = playerDetected.transform.position;

            // Se recorre cada enemigo para asignar la referencia del player
            foreach(EnemyAI currentEnemy in allEnemies){
                currentEnemy.player = playerDetected.transform;
            }

            // Se valida qeu la instancia de la escena este activa para asignarle la puerta
            if(SceneLoadingManager.SceneInstance != null){
                int idConnection = SceneLoadingManager.SceneInstance.DoorId;
                foreach(Door currentDoor in allDoors){
                    if(idConnection == currentDoor.ConexionId){
                        if(playerControl != null) playerControl.SetPositionAndRotation(currentDoor.transform);
                        break;
                    }
                }
            }
        }


        // Metodo de llamada de Unity, se llama una unica vez cuando el GameObject es destruido
        private void OnDestroy() {
            if(playerCombat != null) playerCombat.onDead -= ResetAllEnemies;
        }


        // Metodo que se emplea para resetear todas las entidades de enemigos
        private void ResetAllEnemies(){
            foreach(EnemyAI currentEnemy in allEnemies){
                currentEnemy.ChangeState(currentEnemy.GetResetState());
            }
        }



    }
}