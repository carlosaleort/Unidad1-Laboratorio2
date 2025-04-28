using UnityEngine;
using Assets.JSGAONA.Unidad1.Scripts;

namespace Assets.JSGAONA.Unidad2.Scripts {

    // Arquitectura M-V-C  <!-- CONTROLADOR -->
    // Se emplea este script para gestionar el controlador del personaje
    public class PlayerCombatController : MonoBehaviour {
        
        // Referencida del sistema de combate del personaje
        private PlayerCombat playerCombat;

        private PlayerController playerController;

        // Propiedad que referencia del sistema de la vista UI del personaje
        public PlayerHealthView PlayerView { get; set; }


        private void Awake() {
            playerCombat = GetComponent<PlayerCombat>();
            playerController = GetComponent<PlayerController>();
        }


        private void Start() {
            playerCombat.HealthChanged += OnHealthChanged;
            playerCombat.ResourceChanged += OnResourceChanged;
            PlayerView.ConfigureLifeAndResource(playerCombat.MaxLifePoint, playerCombat.MaxResourcePoint);
        }


        private void OnDestroy() {
            playerCombat.HealthChanged -= OnHealthChanged;
            playerCombat.ResourceChanged -= OnResourceChanged;
        }


        // Metodo que permite actualizar la UI de la barra de salud
        private void OnHealthChanged(int amount, int maxLifePoint) {
            PlayerView.SetLifePoint(amount, maxLifePoint);
        }


        // Metodo que permite actualizar la UI de la barra de salud
        private void OnResourceChanged(int amount, int maxResourcePoint) {
            PlayerView.SetResourcePoint(amount, maxResourcePoint);
        }


        // Metodo que permite recibir algo de danio
        public void TakeDamage(int amount) {
            if(playerCombat != null) playerCombat.TakeDamage(amount);
        }


        // Metodo que permite curar al personaje
        public void Heal(int amount) {
            if(playerCombat != null) playerCombat.Heal(amount);
        }


        // Metodo que permite activar el salto desde la UI
        public void Jump() {
            if(playerController != null) playerController.Jump();
        }


        // Metodo que permite activar el dash desde la UI
        public void Dash() {
            if(playerController != null) playerController.Dash();
        }


        // Metodo que permite activar el pulso de anulacion desde la UI
        public void NullPulse() {
            if(playerCombat != null) playerCombat.NullPulse();
        }
    }
}