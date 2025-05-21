using UnityEngine;

namespace Assets.JSGAONA.Unidad2.Scripts {

    // Se emplea este script para gestionar el sistema de combate del enemigo
    public class EnemyCombat : MonoBehaviour {

        // Variables visibles desde el inspector de Unity
        [SerializeField] private Weapon weapon;
        [SerializeField] private string nameParameter = "isAttacking";

        // Variables ocultas desde el inspector de Unity
        private float nextCheck = 1f;
        private Animator animController;

        // Propiedades
        public bool IsAttacking => animController.GetBool(nameParameter);
        

        // Metodo de llamada de Unity, se llama una unica vez al iniciar el app, es el primer
        // metodo en ejecutarse, se realiza la asignacion de componentes
        private void Awake(){
            animController = GetComponent<Animator>();
        }


        // Se emplea este script para gestionar el ataque basico del personaje
        public void BasicAttack(string nameAnim){
            // Se ejecuta la animacion de ataque
            if(!string.IsNullOrEmpty(nameAnim)) animController.CrossFade(nameAnim, 0.1f);
        }

        
        // Se emplea este metodo para poder verificar el siguiente intervalo de ataque
        public bool CheckIntervalBasicAttack(float speedAttack) {
            if (Time.timeSinceLevelLoad <= nextCheck) return false;
            // Se justa el tiempo para determinar el proximo chequeo
            nextCheck = Time.timeSinceLevelLoad + speedAttack;
            return true;
        }


        // Se emplea este metodo para gestionar el sistema de combate del enemigo a rango
        public void UseWeapon(){
            if(weapon != null) weapon.Fire();
        }
    }
}