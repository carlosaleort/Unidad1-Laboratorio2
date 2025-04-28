using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.JSGAONA.Unidad2.Scripts {

    // Arquitectura M-V-C  <!-- VISTA -->
    // Se emplea este script para gestionar las acciones en UI del personaje
    public class PlayerHealthView : MonoBehaviour {
        
        // Referencias de los sliders de la vida y el recurso
        [SerializeField] private Slider sliderLifePoint;
        [SerializeField] private Slider sliderResourcePoint;

        // Referencias de los labels de la vida y el recurso
        [SerializeField] private TMP_Text txtLifePoint;
        [SerializeField] private TMP_Text txtResourcePoint;

        // referencia del sistema de combate y movimiento del perosnaje
        public PlayerCombatController PlayerCombatController { get; set; }



        // Metodo que permite configurar la vida y el recurso de la UI
        public void ConfigureLifeAndResource(int maxLifePoint, int maxResourcePoint) {
            // El valor minimo de la vida y recurso es cero
            sliderLifePoint.minValue = 0;
            sliderResourcePoint.minValue = 0;
            // El valor maximo de la vida y recurso es el que viene cuando se llama al metodo
            sliderLifePoint.maxValue = maxLifePoint;
            sliderResourcePoint.maxValue = maxResourcePoint;
            // La barra de vida y recurso sera en funcion de los puntos maximos
            sliderLifePoint.value = maxLifePoint;
            sliderResourcePoint.value = maxResourcePoint;

            txtLifePoint.text = string.Format("{0}/{1}", maxLifePoint, maxLifePoint);
            txtResourcePoint.text = string.Format("{0}/{1}", maxResourcePoint, maxResourcePoint);
        }


        // metodo que permite ajustar la barra de salud
        public void SetLifePoint(int amount, int maxLifePoint) {
            sliderLifePoint.value = Mathf.Clamp(amount, 0, maxLifePoint);
            txtLifePoint.text = string.Format("{0}/{1}", amount, maxLifePoint);
        }


        // metodo que permite ajustar la barra de salud
        public void SetResourcePoint(int amount, int maxResourcePoint) {
            sliderResourcePoint.value = Mathf.Clamp(amount, 0, maxResourcePoint);
            txtResourcePoint.text = string.Format("{0}/{1}", amount, maxResourcePoint);
        }


        // Metodo que permite activar el salto desde la UI
        public void Jump() {
            PlayerCombatController.Jump();
        }


        // Metodo que permite activar el dash desde la UI
        public void Dash() {
            PlayerCombatController.Dash();
        }


        // Metodo que permite activar el pulso de anulacion desde la UI
        public void NullPulse() {
            PlayerCombatController.NullPulse();
        }
    }
}