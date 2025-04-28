using UnityEngine;
using Assets.JSGAONA.Unidad1.Scripts;
using System;

namespace Assets.JSGAONA.Unidad2.Scripts {
    
    // Arquitectura M-V-C  <!-- MODELO -->
    // Se emplea este script para gestionar el sistema de combate del personaje
    public class PlayerCombat : MonoBehaviour {
        
        [SerializeField] private int maxLifePoint = 250;
        [SerializeField] private int maxResourcePoint = 100;

        [SerializeField] private GameObject nullPulse;
        [SerializeField] private int cost;

        [SerializeField] private ParticleSystem blood;



        // Delegado para cuando la vida cambia
        public delegate void OnHealthChanged(int amount, int maxLifePoint);
        public event OnHealthChanged HealthChanged;

        // Delegado para cuando el recurso cambia
        public delegate void OnResourceChanged(int amount, int maxResourcePoint);
        public event OnResourceChanged ResourceChanged;


        private bool isAlive = true;
        private int currentLifePoint;
        private int currentResourcePoint;
        private PlayerController playerController;

        public int MaxLifePoint => maxLifePoint;
        public int MaxResourcePoint => maxResourcePoint;


        private void Awake() {
            playerController = GetComponent<PlayerController>();
        }


        private void Start() {
            currentLifePoint = maxLifePoint;
            currentResourcePoint = maxResourcePoint;
        }


        public void NullPulse(){
            // Se valida que exista referencia de la habilidad y el pj tenga mana
            if(nullPulse != null && currentResourcePoint >= cost) {
                nullPulse.SetActive(true);
                currentResourcePoint -= cost;
                ResourceChanged?.Invoke(currentResourcePoint, maxResourcePoint);
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
            }else{
                // Animacion de recibir daño
            }
            if(blood != null) blood.Play();
            HealthChanged?.Invoke(currentLifePoint, maxLifePoint);
        }


        // // Metodo que permite tomar recurso        
        // public void TakeResource(int amount) {
        //     // Si esta muerto no hacer nada
        //     if(!isAlive) return;
        //     // Me he quedado sin recurso
        //     if(currentResourcePoint > 0) {
        //         currentResourcePoint -= amount;
        //         ResourceChanged?.Invoke(currentResourcePoint, maxResourcePoint);
        //     }
        // }
    }
}