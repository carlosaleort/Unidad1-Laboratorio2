using System.Collections;
using System.Collections.Generic;
using Assets.JSGAONA.Unidad1.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.JSGAONA.Unidad2.Scripts {
    
    public class PlayerCombat : MonoBehaviour {
        
        [SerializeField] private int maxLifePoint = 250;
        [SerializeField] private int maxResourcePoint = 100;

        [SerializeField] private Slider sliderLifePoint;
        [SerializeField] private Slider sliderResourcePoint;

        private bool isAlive = true;
        private int currentLifePoint;
        private int currentResourcePoint;
        private PlayerController playerController;

        [SerializeField] private float anulacionRange = 10f;
        [SerializeField] private float cooldownDuration = 5f;
        [SerializeField] private int resourceCost = 30;
        [SerializeField] private ParticleSystem anulacionEffect;

        private float cooldownTimer;
        private RaycastHit[] hitResults = new RaycastHit[20]; // Reutilizable


        private void Awake() {
            playerController = GetComponent<PlayerController>();
        }


        private void Start() {
            currentLifePoint = maxLifePoint;
            currentResourcePoint = maxResourcePoint;

            // Mi barra de salud y recurso es min 0 y max es el de la variable
            sliderLifePoint.minValue = 0;
            sliderResourcePoint.minValue = 0;

            sliderLifePoint.maxValue = maxLifePoint;
            sliderResourcePoint.maxValue = maxResourcePoint;

            sliderLifePoint.value = maxLifePoint;
            sliderResourcePoint.value = maxResourcePoint;
        }


        public void TakeDamage(int amount) {
            if(!isAlive) return;

            currentLifePoint -= amount;
            sliderLifePoint.value = Mathf.Clamp(currentLifePoint, 0, maxLifePoint);

            // Me he quedado sin vida
            if(currentLifePoint <= 0){
                currentLifePoint = 0;
                isAlive = false;
                playerController.StopMovement();
                // Accion de morir o reinicio del pj
            }else{
                // Animacion de recibir daño
            }
        }
        public void Heal(int amount)
        {
            if (!isAlive) return;
            currentLifePoint = Mathf.Min(currentLifePoint + amount, maxLifePoint);
            sliderLifePoint.value = currentLifePoint;
        }

        public void RestoreResource(int amount)
        {
            currentResourcePoint = Mathf.Min(currentResourcePoint + amount, maxResourcePoint);
            sliderResourcePoint.value = currentResourcePoint;
        }

        public void TakeResource(int amount){
            currentResourcePoint -= amount;
            // Me he quedado sin recurso
            if(currentResourcePoint <= 0){
                // Accion de morir o reinicio del pj
            }
        }

        public void PulsoDeAnulacion()
        {
            if (cooldownTimer > 0f || currentResourcePoint < resourceCost) return;

            // Gasta recurso
            TakeResource(resourceCost);

            // Inicia cooldown
            cooldownTimer = cooldownDuration;

            // Efecto visual
            if (anulacionEffect != null)
            {
                anulacionEffect.Play();
            }

            Vector3 origin = transform.position;
            Vector3 direction = Vector3.forward; // No importa en SphereCastAll, solo por sintaxis
            float radius = anulacionRange;

            int hitCount = Physics.SphereCastNonAlloc(origin, radius, direction, hitResults, 0f);

            for (int i = 0; i < hitCount; i++)
            {
                GameObject obj = hitResults[i].collider.gameObject;

                // Verifica línea de visión
                Vector3 targetDir = obj.transform.position - origin;
                if (Physics.Raycast(origin, targetDir.normalized, out RaycastHit rayHit, anulacionRange))
                {
                    if (rayHit.collider.gameObject != obj) continue; // Algo bloquea la visión
                }

                // Aplica la anulación si implementa la interfaz
                IAnulable anulable = obj.GetComponent<IAnulable>();
                if (anulable != null)
                {
                    anulable.Anular();
                }
            }
        }

        private void Update()
        {
            if (cooldownTimer > 0f)
            {
                cooldownTimer -= Time.deltaTime;
            }

            // Ejemplo para activarlo con una tecla (temporal para pruebas)
            if (Input.GetKeyDown(KeyCode.Q))
            {
                PulsoDeAnulacion();
            }
        }


    }
}