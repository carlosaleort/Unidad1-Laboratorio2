using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace Assets.JSGAONA.Unidad1.Scripts {

    // Componentes requeridos para que funcione el script
    [RequireComponent(typeof(NavMeshAgent))]

    // Este script se emplea para gestionar la logica de los enemigos
    public class Enemy : MonoBehaviour {
        
        // Variables visibles desde el inspector de Unity
        [SerializeField] private float speedRotation = 120;
        [SerializeField] private float chaseDistance = 3.5f;
        [SerializeField] private float approachDistance = 0.5f;
        [SerializeField] private float updateInterval  = 0.25f;
        [SerializeField] private LayerMask obstacleMask; // <--- NUEVA VARIABLE


        // Variables ocultas desde el inspector de Unity
        public bool isPlayerInRange = false;
        public bool inRange = false;
        private NavMeshAgent agent;
        public Transform player;
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
            if(player != null) StartCoroutine(UpdateEnemyBehavior());
        }
        
        // Metodo de llamada de Unity, se llama en el momento de que el GameObject es destruido
        private void OnDestroy() {
            if(player != null) StopCoroutine(UpdateEnemyBehavior());
        }


        // Metodo de llamada de Unity, se activa cuando el renderizador del objeto entra en el campo
        // de vision de la camara activa
        private void OnBecameVisible() {
            enabled = true;
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
        }


        // Metodo de llamada de Unity, se activa cuando el renderizador del objeto sale del campo de
        // vision de la camara.
        private void OnBecameInvisible() {
            enabled = false;
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
        }


        // Metodo de llamada de Unity, se llama en cada frame del computador
        // Se realiza la logica de control del enemigo
        private void Update() {
            // Se valida que exista una referencia valida del personaje a seguir y este esta a rango
            if (isPlayerInRange) {
                // Mira al jugador suavemente
                Vector3 direction = (player.position - transform.position).normalized;
                direction.y = 0; // para evitar rotación vertical
                if (direction != Vector3.zero) {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation,
                        speedRotation * Time.deltaTime);
                }
                // El enemigo esta en rango de alcance del personaje
                if(inRange) {
                    agent.ResetPath();
                } else {
                    agent.SetDestination(player.position);
                }
            }
        }


        // Coroutine para optimizar las actualizaciones
        private IEnumerator UpdateEnemyBehavior()
        {
            while (true)
            {
                float distance = Vector3.Distance(transform.position, player.position);

                if (distance <= chaseDistance)
                {
                    // Dirección del enemigo al jugador
                    Vector3 directionToPlayer = (player.position - transform.position).normalized;

                    // Posición del raycast (un poco elevada para evitar errores de suelo)
                    Vector3 rayOrigin = transform.position;

                    // Lanza raycast hacia el jugador
                    if (Physics.Raycast(rayOrigin, directionToPlayer, out RaycastHit hit, chaseDistance))
                    {
                        // Verifica si el raycast golpeó al jugador directamente
                        if (hit.transform == player)
                        {
                            isPlayerInRange = true;
                        }
                        else
                        {
                            isPlayerInRange = false;
                        }
                    }
                    else
                    {
                        isPlayerInRange = false;
                    }
                }
                else
                {
                    isPlayerInRange = false;
                }

                inRange = distance <= approachDistance;
                yield return new WaitForSeconds(updateInterval);
            }
        }


    }
}