using UnityEngine;
using Assets.JSGAONA.Unidad1.Scripts;

namespace Assets.JSGAONA.Unidad2.Scripts {
    
    // Arquitectura M-V-C  <!-- MODELO -->
    // Se emplea este script para gestionar el sistema de combate del personaje
    public class PlayerCombat : MonoBehaviour {
        
        // Variables visibles desde el inspector de Unity
        [Header("Life and Resoruce")]
        [SerializeField] private int maxLifePoint = 250;
        [SerializeField] private int maxResourcePoint = 100;

        [Header("Skill")]
        [SerializeField] private GameObject nullPulse;
        [SerializeField] private int cost;
        [SerializeField] private float cooldown;

        [Header("Player effects")]
        [SerializeField] private ParticleSystem blood;

        // Delegado para cuando la vida cambia
        public delegate void OnHealthChanged(int amount, int maxLifePoint);
        public event OnHealthChanged HealthChanged;

        // Delegado para cuando el recurso cambia
        public delegate void OnResourceChanged(int amount, int maxResourcePoint, float cooldown);
        public event OnResourceChanged ResourceChanged;

        // Delegado cuando el pj muere y esta sin vida
        public delegate void OnDeadActive();
        public OnDeadActive onDead;


        // Variables visibles desde el inspector de Unity
        private bool isAlive = true;
        private int currentLifePoint;
        private int currentResourcePoint;
        private Animator animController;
        private PlayerController playerController;

        // Propiedades
        public int MaxLifePoint => maxLifePoint;
        public int MaxResourcePoint => maxResourcePoint;


        // Metodo de llamada de Unity, se llama una unica vez al iniciar el app, es el primer
        // metodo en ejecutarse, se realiza la asignacion de componentes
        private void Awake() {
            animController = GetComponent<Animator>();
            playerController = GetComponent<PlayerController>();
        }


        // Metodo de llamada de Unity, se llama una unica vez al iniciar el app se llama despues de
        // Awake, se realiza la configuracion previa al inicio de la lógica del juego
        private void Start() {
            currentLifePoint = maxLifePoint;
            currentResourcePoint = maxResourcePoint;
        }


        private void Update() {
            if(Input.GetKeyDown(KeyCode.C)) NullPulse(); 
        }


        // Metodo que se emplea para poder utilizar la habilidad
        public void NullPulse(){
            // Se valida que exista referencia de la habilidad y el pj tenga mana
            if(nullPulse != null && currentResourcePoint >= cost) {
                animController.SetTrigger("hack");
                nullPulse.SetActive(true);
                currentResourcePoint -= cost;
                ResourceChanged?.Invoke(currentResourcePoint, maxResourcePoint, cooldown);
            }
        }


        // Metodo que permite curar al personaje
        public void Heal(int amount) {
            // Si esta muerto no hacer nada
            if(!isAlive) return;
            currentLifePoint += amount;
            // La vida ha superado el maximo permitido
            if(currentLifePoint > maxLifePoint) {
                currentLifePoint = maxLifePoint;
            }
            HealthChanged?.Invoke(currentLifePoint, maxLifePoint);
        }


        // Metodo que permite recibir algo de danio
        public void TakeDamage(int amount) {
            // Si esta muerto no hacer nada
            if(!isAlive) return;
            currentLifePoint -= amount;
            // Me he quedado sin vida
            if(currentLifePoint <= 0){
                currentLifePoint = 0;
                isAlive = false;
                playerController.ManagerMovement(false);
                onDead?.Invoke();
            }else{
                // Animacion de recibir daño
            }
            if(blood != null) blood.Play();
            HealthChanged?.Invoke(currentLifePoint, maxLifePoint);
        }
    }
}