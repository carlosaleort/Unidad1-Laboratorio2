using UnityEngine;
using System.Collections;
using Assets.JSGAONA.Unidad2.Scripts.Skill;

namespace Assets.JSGAONA.Unidad2.Scripts {

    // Se emplea este script para poder hackear teminales de seguridad
    public class SecurityTerminal : MonoBehaviour, IHackeable {

        // Variables visibles desde el inspector de Unity
        [SerializeField] private GameObject trap;
        
        public bool ItsHacked { get; set; }


        // Metodo que permite hackear
        public void Hack(float timeHack) {
            if(!ItsHacked) StartCoroutine(WaitTime(timeHack));
        }


        // Coroutina que permite esperar un tiempo para devolcer al estado inicial el objeto hackeado
        private IEnumerator WaitTime(float timeHack){
            ItsHacked = true;
            trap.SetActive(false);
            yield return new WaitForSeconds(timeHack);
            trap.SetActive(true);
            ItsHacked = false;
        }
    }
}