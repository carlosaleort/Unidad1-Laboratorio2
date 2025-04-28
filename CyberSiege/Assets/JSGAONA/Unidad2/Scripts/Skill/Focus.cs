using System.Collections;
using UnityEngine;

namespace Assets.JSGAONA.Unidad2.Scripts.Skill {

    public class Focus : MonoBehaviour, IHackeable {
        
        [SerializeField] private Light mainLight;
        [SerializeField] private float flickerDuration = 1.5f;
        [SerializeField] private float flickerInterval = 0.1f;

        public bool ItsHacked { get; set; }


        void Start() {
            ItsHacked = false;
        }

        public void Hack(float timeHack) {
            if(!ItsHacked) StartCoroutine(RecoverySequence(timeHack - flickerDuration));
        }

        // [SerializeField] private float 



        private IEnumerator RecoverySequence(float timeHack) {
            yield return new WaitForSeconds(0.15f);
            ItsHacked = true;
            mainLight.enabled = false;
            yield return new WaitForSeconds(timeHack);
            float elapsed = 0f;
            while (elapsed < flickerDuration) {
                // Alterna el estado: si estaba apagado lo enciende, y viceversa
                mainLight.enabled = !mainLight.enabled;
                yield return new WaitForSeconds(flickerInterval);
                elapsed += flickerInterval;
            }

            // Paso 3: Al finalizar el efecto, enciende el foco para indicar que ha recuperado la señal
            mainLight.enabled = true;
            ItsHacked = false;
        }
    }
}