using System.Collections;
using UnityEngine;

namespace Assets.JSGAONA.Unidad2.Scripts.Skill {
 
    public class RedCancellation : MonoBehaviour {
        
        [SerializeField] private float timeHack = 3.0f;
        [SerializeField] [Range(1f, 15f)] private float radius = 15.0f;
        [SerializeField] private LayerMask includeLayer;
        [SerializeField] private int searchSize = 15;
        [SerializeField] private float timeInScene = 1.0f;

        private Collider[] detectedColliders;

        private void Start() {
            detectedColliders = new Collider[searchSize];
        }


        private void OnEnable() {
            StartCoroutine(ActiveRedCancellation());
        }


        // Se emplea este metodo para poder Activar la habilidad de hackeo
        private IEnumerator ActiveRedCancellation() {
            // Crea un barrido circular que no genera Garbage y solo busca a layer includeLayer
            int hitCount = Physics.OverlapSphereNonAlloc(transform.position, radius,
                    detectedColliders, includeLayer, QueryTriggerInteraction.Ignore);
            
            // Procesamos los colliders detectados
            for (int i = 0; i < hitCount; i++) {
                // Se obtiene la referencia de cada objetivo detectado y se realizan calculos
                Collider col = detectedColliders[i];
                float distance = Vector3.Distance(transform.position, col.transform.position);
                Vector3 directionTarget = (col.transform.position - transform.position).normalized;

                // Se verifica si existe un obstaculo entre el personaje y el objetivo para hackear
                if(!Physics.Raycast(transform.position, directionTarget, distance, ~includeLayer)) {
                    if(col.TryGetComponent(out IHackeable hackeable)) hackeable.Hack(timeHack);
                }
            }
            yield return new WaitForSeconds(timeInScene);
            gameObject.SetActive(false);
        }


    #if UNITY_EDITOR
        // Metodo de llamada de Unity, se emplea para visualizar en escena, acciones del codigo
        [SerializeField] private Color colorSkill; 
        private void OnDrawGizmos() {
            Gizmos.color = colorSkill;
            Gizmos.DrawSphere(transform.position, radius);
        }
    #endif
    }
}