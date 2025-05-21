using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Assets.JSGAONA.Unidad2.Scripts.Skill;
using Assets.JSGAONA.Unidad2.Scripts.FMS.Temple;

namespace Assets.JSGAONA.Unidad2.Scripts.FMS {

    [RequireComponent(typeof(NavMeshAgent))]
    // Se emplea este script para generar la IA del enemigo con NavMeshAgent
    public class EnemyAI : MonoBehaviour, IHackeable {

        // Variables visibles desde el inspector de Unity
        [Header("Adjust movement")]
        [SerializeField] private float speedMovement = 2.5f;
        [SerializeField] private float speedRotation = 120;
        [SerializeField] private LayerMask includeLayer;

        [Header("Adjust wayPoint")]
        // [SerializeField] private bool useRandomPatrol = false;
        [SerializeField] private Transform[] wayPoints;

        
        [Header("Adjust States Temple")]
        [SerializeField] private AIState currentState;
        [SerializeField] private AIBehaviorTemplate behaviorTemplate;

        public Transform player;

        // Variables ocultas desde el inspector de Unity
        private int currentIndex = 0;
        public bool inRange = false;
        private float nextCheck = 0.25f;
        private float timeStopMotion = 0.0f;
        private NavMeshAgent agent;
        private Vector3 initialPoint;
        private Quaternion initialRotation;
        private Animator animController;
        private readonly Dictionary<string, ActiveState> ActiveStates = new ();

        // Propiedades
        public bool ItsHacked { get; set; }



        // Metodo de llamada de Unity, se llama una unica vez al iniciar el app, es el primer
        // metodo en ejecutarse, se realiza la asignacion de componentes
        private void Awake(){
            agent = GetComponent<NavMeshAgent>();
            animController = GetComponent<Animator>();
        }


        // Metodo de llamada de Unity, se llama una unica vez al iniciar el app, se ejecuta despues
        // de Awake, se realiza la asignacion de variables y configuracion del script
        private void Start() {
            initialPoint = transform.position;
            initialRotation = transform.rotation;

            agent.speed = speedMovement;

            // El pj dispone de un estado de reposo
            if (behaviorTemplate.DefaultState != null) {
                AddState(behaviorTemplate.DefaultState, behaviorTemplate.DefaultStateTemplate);
            }
            // El pj dispone de un estado de persecucion
            if (behaviorTemplate.ChaseState != null) {
                AddState(behaviorTemplate.ChaseState, behaviorTemplate.ChaseTemplate);
            }
            // El pj dispone de un estado de reinicio
            if (behaviorTemplate.ChaseState != null) {
                AddState(behaviorTemplate.ResetState, null);
            }
            // El pj dispone de un estado de ataque basico
            if (behaviorTemplate.BasicAttackState != null) {
                AddState(behaviorTemplate.BasicAttackState, behaviorTemplate.BasicAttackTemplate);
            }
            // El pj dispone de un estado de hack
            if (behaviorTemplate.HackState != null) {
                AddState(behaviorTemplate.HackState, null);
            }

            // Se recorre todos los estados y se los inicializa
            foreach (ActiveState activeState in ActiveStates.Values) {
                activeState.State.Initialize(this, activeState.StateTemplate);
            }
            // Se establece el estado por defecto
            currentState = GetDefaultState();
            currentState.Enter();
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        }


        // Metodo de llamada de Unity, se llama en cada frame del computador
        // Se realiza la logica de control del enemigo mediante Estados
        private void Update() {
            // Se valida que el estado a ejecutar no sea nulo
            if(currentState != null) currentState.Execute();

            // El enemigo esta a rango del jugador
            if(inRange && !ItsHacked) {
                // Mira al jugador suavemente
                Vector3 direction = (player.position - transform.position).normalized;
                direction.y = 0;
                // Se voltea a ver simepre y cuando no exista movimiento
                if (direction != Vector3.zero) {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    float newSpeedRot = speedRotation * Time.deltaTime;
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, newSpeedRot);
                }
            }
        }


        // Metodo de llamada de Unity, se activa cuando el renderizador del objeto entra en el campo
        // de vision de la camara activa
        private void OnBecameVisible() {
            enabled = true;
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        }


        // Metodo de llamada de Unity, se activa cuando el renderizador del objeto sale del campo de
        // vision de la camara.
        private void OnBecameInvisible() {
            enabled = false;
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
        }


        // Se emplea este metodo para poder agregar estados al diccionario
        private void AddState(AIState state, AIStateTemplate template) {
            // Se crea un nuevo ActiveState
            ActiveState newState = new ActiveState {
                State = Instantiate(state),
                StateTemplate = template
            };
            ActiveStates.Add(state.name, newState);
        }


        // Metodo que permite cambiar el estado del enemigo
        public void ChangeState(AIState newState) {
            // Se valida que el estado actual no sea nulo para dejarlo
            if (currentState != null) {
                currentState.Exit();
            }
            // Se fija el nuevo estado
            currentState = newState;
            if (currentState != null) {
                currentState.Enter();
            }
        }


        // Metodo que sirve para detectar la fecuencia de actualizacion de cada estado
        public bool CheckInterval() {
            if (Time.timeSinceLevelLoad <= nextCheck) return false;
            // Se justa el tiempo para determinar el proximo chequeo
            nextCheck = Time.timeSinceLevelLoad + behaviorTemplate.CheckInterval;
            return true;
        }


        // Metodo que permite ajustar la velocidad de movimiento del personaje
        public void AdjustAgent(int idAdjut, float modifiedSpeed = 1) {
            // Seleccionar el caso del ajuste
            switch (idAdjut) {
                // Ajustar el agente sin velocidad IDLE
                case 0:
                    agent.speed = 0;
                    agent.stoppingDistance = 0.1f;
                    break;
                
                // Ajustar el agente con velocidad normal PATROL
                case 1:
                    agent.speed = speedMovement * modifiedSpeed;
                    agent.stoppingDistance = 0.1f;;
                    break;


                // Ajustar el agente con velocidad de persecucion CHASE
                case 2:
                    agent.speed = speedMovement * modifiedSpeed;
                    agent.stoppingDistance = behaviorTemplate.MinDistanceFromTarget;
                    break;
            }
            animController.SetFloat("speed", agent.speed);
        }


        // Permite obtener el estado por defecto: IDLE
        public AIState GetDefaultState() {
            return ActiveStates[behaviorTemplate.DefaultState.name].State;
        }


        // Se obtiene el estado de persecucion: CHASE
        public AIState GetChaseState(){
            // Se verifica si existe un estado de persecucion
            if (behaviorTemplate.ChaseState == null) return null;
            return ActiveStates[behaviorTemplate.ChaseState.name].State;
        }


        // Se obtiene el estado de reseteo: RESET
        public AIState GetResetState(){
            // Se verifica si existe un estado de persecucion
            if (behaviorTemplate.ResetState == null) return null;
            return ActiveStates[behaviorTemplate.ResetState.name].State;
        }


        // Se obtiene el estado de combat basico: BASICATTACK
        public AIState GetBasicAttackState(){
            // Se verifica si existe un estado de ataque basico
            if (behaviorTemplate.BasicAttackState == null) return null;
            return ActiveStates[behaviorTemplate.BasicAttackState.name].State;
        }


        // Se obtiene el estado de hackeo: HACK
        public AIState GetHackState(){
            // Se verifica si existe un estado de persecucion
            if (behaviorTemplate.HackState == null) return null;
            return ActiveStates[behaviorTemplate.HackState.name].State;
        }


        // Se emplea este metodo para dteerminar la distancia entre en enemigo y el jugador
        public float GetDistance() {
            return Vector3.Distance(transform.position, player.position);
        }


        // Se emplea este metodo para detectar si exite algun obstaculo
        public bool DetectedObstacle(float distance) {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            return Physics.Raycast(transform.position, directionToPlayer, distance, includeLayer, QueryTriggerInteraction.Ignore);
        }


        // Permite verificar si el ataque se encuentra al alcance del objetivo
        public bool ValidateView(AIStateIdleTemplate template, float distance) {
            // Se calcula el angulo de vision
            Vector3 directionToTarget = player.position - transform.position;
            
            // Se valida primeramente si el personaje se encuentra dentro del area de auto aggro
            if(distance < template.AutoAggroDistance) {
                // Realizar un Raycast para verificar que no exista obstaculos
                return !Physics.Raycast(transform.position, directionToTarget,
                    distance, includeLayer, QueryTriggerInteraction.Ignore);
            }

            float angleToTarget = Vector3.Angle(directionToTarget, transform.forward);
            // Si el angulo hacia el objetivo esta dentro del area de vision y el alcance
            if (angleToTarget <= template.ViewAngle && distance >= 0 && distance <= template.DetectionDistance) {
                // Realizar un Raycast para verificar que no exista obstaculos
                return !Physics.Raycast(transform.position, directionToTarget,
                    distance, includeLayer, QueryTriggerInteraction.Ignore);
            }
            // Si el angulo o la distancia estan fuera del rango, el objetivo no es visible
            return false;
        }


        // Se emplea este metodo para poder obtener si el estado de Reset se a completado
        public bool GetResetEnemyAi(bool resetRotation) {
            // Espera a que se haya calculado el camino
            if (!agent.pathPending) {
                // Esta dentro del rango de parada
                if (agent.remainingDistance <= agent.stoppingDistance) {
                    // No hay mas camino o el agente se detuvo por completo
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                        if(resetRotation) transform.rotation = initialRotation;
                        return true;
                    }
                }
            }
            return false;
        }


        public bool CanChase(){
            // Verificar si el agente pierde el camino
            return agent.pathStatus == NavMeshPathStatus.PathComplete;
        }


        // Se emplea este metodo para gestionar el estado de persecucion
        public bool Chase(float distance) {
            // Se verifica si hay un obstaculo para seguirlo
            if(DetectedObstacle(distance)){
                if(inRange) inRange = false;
                agent.stoppingDistance = 0;
                // Si el objetivo se aleja, siguelo
                agent.SetDestination(player.position);
                return true;
            }

            // Se valida si el enemigo esta lo suficientemente cerca del jugador
            if(distance <= behaviorTemplate.MinDistanceFromTarget){
                // Una vez alcanzado al objetivo, detiene
                if(!inRange) {
                    agent.ResetPath();
                    inRange = true;
                }

            // Se valida si el personaje se alejado del jugador para proceder a seguirlo
            } else  if(distance > behaviorTemplate.MaxDistanceFromTarget) {
                if(inRange) inRange = false;
                // Si el objetivo se aleja, siguelo
                agent.SetDestination(player.position);
                return true;
            }
            return false;
        }


        // Metodo que permite establecer el siguiente punto de control de la patrulla
        public void SetDestinationPatrol () {
            currentIndex++;
            
            // Se valida si el indice ha superado al tamano del arreglo
            if(currentIndex >= wayPoints.Length){
                currentIndex = 0;
            }
            agent.SetDestination(wayPoints[currentIndex].position);
        }


        // Metodo que permite activar el estado de reset al momento de entrar
        public void ResetPositionEnemyAi() {
            agent.SetDestination(initialPoint);
        }


        // Metodo que permite hackear
        public void Hack(float timeHack) {
            ItsHacked = true;
            timeStopMotion = timeHack;
            ChangeState(GetHackState());
        }


        // Metodo que permite contar el tiempo de hackeo
        public bool HackingTime () {
            timeStopMotion -= Time.deltaTime;
            // Se valida si el contador a llegado a cero o menos
            return timeStopMotion < 0;
        }


        // Se emplea este metodo para gestionar la animacion de hack
        public void ManagerHack(bool status){
            if(status) {
                animController.SetTrigger("hack");
                agent.ResetPath();
            }else{
                animController.SetTrigger("endHack");
            }
        }



    #if UNITY_EDITOR
        // Metodo de llamada de Unity, permite dibujar sobre la ventana de escena una esfera
        // que brinda la visualizacion del rango de area sobre el cual patrulla en enemigo aleatoriamente
        private void OnDrawGizmos(){
            if(behaviorTemplate.DefaultStateTemplate == null) return;
            Handles.color = new Color(1f, 1f, 0f, 0.1f); // Amarillo semi-transparente
            Handles.DrawSolidArc(
                transform.position,
                transform.up,
                Quaternion.Euler(0f, -behaviorTemplate.DefaultStateTemplate.ViewAngle, 0f) * transform.forward,
                behaviorTemplate.DefaultStateTemplate.ViewAngle * 2.0f,
                behaviorTemplate.DefaultStateTemplate.DetectionDistance
            );

            Handles.color = new Color(1f, 0f, 0f, 0.1f); // Rojo semi-transparente
            Handles.DrawSolidDisc(transform.position, transform.up, behaviorTemplate.DefaultStateTemplate.AutoAggroDistance);
        }
    #endif
    }
}
