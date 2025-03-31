
using UnityEngine;
using System.Collections;
namespace Assets.JSGAONA.Unidad1.Scripts {

    // Se emplea para obligar al GameObject asignar el componente para su correcto funcionamiento
    [RequireComponent(typeof(CharacterController))]

    // Se emplea este script para gestionar el controlador del personaje jugable
    public class PlayerController : MonoBehaviour {
        
        [Header("Adjust movement")]
        [SerializeField] private float speedMove;
        [SerializeField] private float speedRotation;
        [SerializeField] private float jumpForce = 3.0f;
        [SerializeField] private int maxJumpCount = 1;
        [SerializeField]  private float minFallVelocity = -2;
        [SerializeField] [Range(0, 5)] private float gravityMultiplier = 1;
        [SerializeField] private Joystick joystick;

        [Header("Adjust to gorund")]
        [SerializeField] private float radiusDetectedGround = 0.2f;
        [SerializeField] private float groundCheckDistance = 0.0f;
        [SerializeField] private LayerMask ignoreLayer;

        // Variables ocultas desde el inspector de Unity
        private readonly float gravity = -9.8f;
        private int currentJumpCount = 0;
        public bool onGround = false;
        public float fallVelocity = 0;
        private Vector3 dirMove;
        private Vector3 positionFoot;
        private CharacterController charController;
        // Dash
        [SerializeField] private float dashDistance = 5f;
        [SerializeField] private float dashDuration = 2f;
        [SerializeField] private float dashCooldown = 3f;
        [SerializeField] private LayerMask obstacleLayers;

        private bool isDashing = false;
        private bool canDash = true;


        // Metodo de llamada de Unity, se llama una unica vez al iniciar el app, es el primer
        // metodo en ejecutarse, se realiza la asignacion de componentes
        private void Awake(){
            charController = GetComponent<CharacterController>();
        }
        
        
        // Metodo de llamada de Unity, se llama en cada frame del computador
        // Se realiza la logica de control del personaje jugable
        private void Update() {
            dirMove = new Vector3(joystick.Horizontal, 0, joystick.Vertical).normalized;

            // ****************** 2) Mecánica de dash o movimiento rápido ****************** //

            // Esta conectado a tierra
            if(onGround){
                fallVelocity = Mathf.Max(minFallVelocity, fallVelocity + gravity * Time.deltaTime);
            
            // No esta conectado a tierra, por ende se esta en el aire
            }else {
                fallVelocity += gravity  * gravityMultiplier * Time.deltaTime;
            }

            // Rotacion del personaje
            if (dirMove != Vector3.zero) {
                Quaternion targetRotation = Quaternion.LookRotation(dirMove);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speedRotation);
            }

            dirMove *= speedMove;
            dirMove.y = fallVelocity;
            charController.Move(dirMove * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && canDash)
            {
                StartCoroutine(PerformDash());
            }
            if (isDashing) return;
        }
        public void DashButton()
        {
            if (!isDashing && canDash)
            {
                StartCoroutine(PerformDash());
            }
        }

        // Metodo de llamada de Unity, se llama en cada actualizacion constante 0.02 seg
        // Se realiza la logica de gestion de fisicas del motor
        private void FixedUpdate() {
            positionFoot = transform.position + Vector3.down * groundCheckDistance;
            onGround = Physics.CheckSphere(positionFoot, radiusDetectedGround, ~ignoreLayer);
        }


        // Metodo publico que se emplea para gestionar los saltos del personaje
        public void Jump() {
            if (isDashing) return;
            // Esta conectado a tierra
            if (onGround) {
                onGround = false;
                CountJump(false);
            
            // No esta conectado a tierra, por ende se esta en el aire
            }else{
                // Se valida si el contador de saltos a superado los permitidos
                if(currentJumpCount < maxJumpCount) {
                    dirMove.y = 0;
                    CountJump(true);
                }
            }
        }


        // Metodo que se utiliza para poder controlar el contador de los saltos
        private void CountJump(bool accumulate) {
            currentJumpCount = accumulate ? (currentJumpCount + 1): 1;
            fallVelocity = jumpForce;
        }
        private IEnumerator PerformDash()
        {
            // Dirección del dash (hacia donde mira el jugador)
            Vector3 dashDirection = transform.forward;

            // Verificar si hay obstáculo
            if (Physics.Raycast(transform.position, dashDirection, dashDistance, obstacleLayers))
            {
                yield break; // Hay un obstáculo enfrente, no hacer dash
            }

            isDashing = true;
            canDash = false;

            float timeElapsed = 0f;
            float speed = dashDistance / dashDuration;

            // Guardar la posición Y para evitar que la gravedad lo afecte
            float initialY = transform.position.y;

            while (timeElapsed < dashDuration)
            {
                Vector3 move = dashDirection * speed * Time.deltaTime;
                move.y = 0; // evitar que suba o baje
                charController.Move(move);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            isDashing = false;

            // Cooldown
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }

#if UNITY_EDITOR
        // Metodo de llamada de Unity, se emplea para visualizar en escena, acciones del codigo
        private void OnDrawGizmos() {
            Gizmos.color = Color.green;
            positionFoot = transform.position + Vector3.down * groundCheckDistance;
            Gizmos.DrawSphere(positionFoot, radiusDetectedGround);
        }
    #endif
    }

}
