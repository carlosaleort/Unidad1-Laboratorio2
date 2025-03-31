using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace Assets.JSGAONA.Unidad1.Scripts {

    // Componentes requeridos para que funcione el script
    [RequireComponent(typeof(NavMeshAgent))]

    // Este script se emplea para gestionar la logica de los enemigos
    public class Enemy : MonoBehaviour {
        
        // Variables visibles desde el inspector de Unity
        [SerializeField] private float chaseDistance = 0.5f;
        [SerializeField] private float updateInterval  = 0.25f;
        [SerializeField] private float minimumDistanceToPlayer = 1.5f;

        // Variables ocultas desde el inspector de Unity
        private bool isPlayerInRange = false;
        private NavMeshAgent agent;
        private Transform player;
        private Vector3 initialPosition;
        public Transform Player { set => player = value; }



        // Metodo de llamada de Unity, se llama una unica vez al iniciar el app, es el primer
        // metodo en ejecutarse, se realiza la asignacion de componentes
        private void Awake() {
            agent = GetComponent<NavMeshAgent>();
        }


        // Metodo de llamada de Unity, se llama una unica vez al iniciar el app, se ejecuta despues
        // de Awake, se realiza la asignacion de variables y configuracion del script
        private void Start() {
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
            StartCoroutine(UpdateEnemyBehavior());
            initialPosition = transform.position;
        }
        
        // Metodo de llamada de Unity, se llama en el momento de que el GameObject es destruido
        private void OnDestroy() {
            StopCoroutine(UpdateEnemyBehavior());
        }


        // Metodo de llamada de Unity, se activa cuando el renderizador del objeto entra en el campo
        // de vision de la camara activa
        private void OnBecameVisible() {
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
        }


        // Metodo de llamada de Unity, se activa cuando el renderizador del objeto sale del campo de
        // vision de la camara.
        private void OnBecameInvisible() {
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
        }


        // Coroutine para optimizar las actualizaciones
        private IEnumerator UpdateEnemyBehavior() {
            while (true)
            {
                float distance = Vector3.Distance(transform.position, player.position);
                isPlayerInRange = distance <= chaseDistance;

                if (isPlayerInRange)
                {
                    if (distance <= minimumDistanceToPlayer)
                    {
                        // Muy cerca del jugador, detener al agente
                        agent.ResetPath();
                    }
                    else
                    {
                        // A distancia segura, seguir al jugador
                        agent.SetDestination(player.position);
                    }
                }
                else
                {
                    // Jugador fuera de rango, regresar a la posición inicial
                    agent.SetDestination(initialPosition);
                }

                yield return new WaitForSeconds(updateInterval);
            }
        }


        // Metodo de llamada de Unity, se llama en cada frame del computador
        // Se realiza la logica de control del enemigo
        private void Update() {
            
        }
    }
}
       