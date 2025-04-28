using System.Collections.Generic;
using Assets.JSGAONA.Unidad1.Scripts;
using Assets.JSGAONA.Unidad2.Scripts.FMS;
using Cinemachine;
using UnityEngine;

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


            PlayerHealthView canvas = FindObjectOfType<PlayerHealthView>();
            // Si no existe en escena una referencia valida del administrador de escenas, se lo crea
            if(canvas == null){
                // Se genera la instancia del canvas
                GameObject canvasGo = Instantiate(prefabCanvasUIPlayer, Vector3.zero, Quaternion.identity);
                canvas = canvasGo.GetComponent<PlayerHealthView>();

                // Se valdia si el personaje dispone del controlador de movimiento para asignar el Joystick
                if(playerDetected.TryGetComponent(out PlayerController playerControl)){
                    playerControl.JoystickController = canvas.gameObject.GetComponentInChildren<Joystick>();
                }

                // Se valida si el personaje dispone del controlador de combate
                if(playerDetected.TryGetComponent(out PlayerCombatController playerCombControl)) {
                    canvas.PlayerCombatController = playerCombControl;
                    playerCombControl.PlayerView = canvas;
                }
            }

            // Se establece el objetivo y hacia donde mira la camara
            virtualCamera.Follow = playerDetected.transform;
            virtualCamera.LookAt = playerDetected.transform;

            foreach(EnemyAI currentEnemy in allEnemies){
                currentEnemy.player = playerDetected.transform;
            }
        }
    }
}