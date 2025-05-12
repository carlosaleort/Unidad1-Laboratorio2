
using UnityEngine;
using System.Collections;
using UnityEngine;
using System.Collections;

namespace Assets.JSGAONA.Unidad1.Scripts
{

    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {

        [Header("Adjust movement")]
        [SerializeField] private float speedMove;
        [SerializeField] private float speedRotation;
        [SerializeField] private float jumpForce = 3.0f;
        [SerializeField] private int maxJumpCount = 1;
        [SerializeField] private float minFallVelocity = -2;
        [SerializeField][Range(0, 5)] private float gravityMultiplier = 1;

        [Header("Adjust to ground")]
        [SerializeField] private float radiusDetectedGround = 0.2f;
        [SerializeField] private float groundCheckDistance = 0.0f;
        [SerializeField] private LayerMask ignoreLayer;

        [Header("Adjust Dash")]
        [SerializeField] private float dashWallDetec = 0.7f;
        [SerializeField] private float offsetTop = 0.25f;
        [SerializeField] private float offsetBottom = 0.25f;
        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private float dashDistance = 5f;
        [SerializeField] private float dashCooldown = 1f;

        public bool enableControl = true;
        private bool onGround = false;
        private bool isDashing = false;
        private bool dashInCooldown = false;
        private int currentJumpCount = 0;
        private float fallVelocity = 0;
        private readonly float gravity = -9.8f;
        private Vector3 dirMove;
        private Vector3 positionFoot;
        private CharacterController charController;
        private Joystick joystick;
        public Joystick JoystickController { set { joystick = value; } }

        // --> AÑADIDO PARA ANIMACIONES
        private Animator animator;

        private void Awake()
        {
            charController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>(); // <-- AÑADIDO: Asignar el Animator
        }

        private void Update()
        {
            if (enableControl)
            {
                if (!isDashing)
                {
                    dirMove = new Vector3(joystick.Horizontal, 0, joystick.Vertical).normalized;

                    if (onGround)
                    {
                        fallVelocity = Mathf.Max(minFallVelocity, fallVelocity + gravity * Time.deltaTime);
                    }
                    else
                    {
                        fallVelocity += gravity * gravityMultiplier * Time.deltaTime;
                    }

                    if (dirMove != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(dirMove);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speedRotation);
                    }
                    dirMove *= speedMove;
                    dirMove.y = fallVelocity;
                }
            }
            charController.Move(dirMove * Time.deltaTime);

            // --> AÑADIDO: Actualizar animaciones
            UpdateAnimations();
        }

        private void FixedUpdate()
        {
            positionFoot = transform.position + Vector3.down * groundCheckDistance;
            onGround = Physics.CheckSphere(positionFoot, radiusDetectedGround, ~ignoreLayer);
        }

        public void Jump()
        {
            if (onGround)
            {
                onGround = false;
                CountJump(false);

                // --> AÑADIDO: Activar animación de salto
                if (animator != null)
                {
                    animator.SetTrigger("Jump");
                }

            }
            else
            {
                if (currentJumpCount < maxJumpCount)
                {
                    dirMove.y = 0;
                    CountJump(true);

                    // --> AÑADIDO: Activar animación de doble salto
                    if (animator != null)
                    {
                        animator.SetTrigger("DoubleJump");
                    }
                }
            }
        }

        private void CountJump(bool accumulate)
        {
            currentJumpCount = accumulate ? (currentJumpCount + 1) : 1;
            fallVelocity = jumpForce;
        }

        public void Dash()
        {
            if (!isDashing && !dashInCooldown)
            {
                StartCoroutine(StartDash());

                // --> AÑADIDO: Activar animación de dash
                if (animator != null)
                {
                    animator.SetTrigger("Dash");
                }
            }
        }

        private IEnumerator StartDash()
        {
            if (DetectWallDuringDash()) yield break;
            dashInCooldown = true;
            isDashing = true;
            fallVelocity = 0;
            Vector3 dashVector = dashDistance * transform.forward;
            float elapsedTime = 0;
            while (elapsedTime < dashDuration)
            {
                if (DetectWallDuringDash())
                {
                    break;
                }
                dirMove = dashVector / dashDuration;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            dirMove = Vector3.zero;
            isDashing = false;
            yield return new WaitForSeconds(dashCooldown);
            dashInCooldown = false;
        }

        private bool DetectWallDuringDash()
        {
            Vector3 rayOriginTop = transform.position + (Vector3.up * offsetTop);
            Vector3 rayOriginBot = transform.position + (Vector3.down * offsetBottom);
            return HitDetectedWallOnDash(rayOriginTop) || HitDetectedWallOnDash(rayOriginBot);
        }

        private bool HitDetectedWallOnDash(Vector3 originRay)
        {
            return Physics.Raycast(originRay, transform.forward, dashWallDetec, ~ignoreLayer);
        }

        public void ManagerMovement(bool value)
        {
            enableControl = value;
            dirMove = Vector3.zero;
        }

        public void ReturnCheckPoint(Vector3 newCheckPoint)
        {
            charController.enabled = false;
            transform.position = newCheckPoint;
            charController.enabled = true;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            positionFoot = transform.position + Vector3.down * groundCheckDistance;
            Gizmos.DrawSphere(positionFoot, radiusDetectedGround);

            Vector3 rayOriginTop = transform.position + Vector3.up * offsetTop;
            Vector3 rayOriginBot = transform.position + Vector3.down * offsetBottom;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(rayOriginTop, transform.forward * dashWallDetec);

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(rayOriginBot, transform.forward * dashWallDetec);
        }
#endif

        // --> AÑADIDO: Método para actualizar animaciones de movimiento
        private void UpdateAnimations()
        {
            if (animator != null)
            {
                float moveAmount = new Vector3(joystick.Horizontal, 0, joystick.Vertical).magnitude;
                animator.SetFloat("Speed", moveAmount);
                animator.SetBool("OnGround", onGround);
                animator.SetBool("IsDashing", isDashing);
            }
        }
    }
}
