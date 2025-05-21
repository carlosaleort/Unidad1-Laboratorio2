using UnityEngine;
using System.Collections;

namespace Assets.JSGAONA.Unidad2.Scripts.Skill {
 
    // Se emplea este script para gestionar la habilidad pulso de anulacion
    public class NullPulse : MonoBehaviour {
        
        // Variables visibles desde el inspecto de Unity
        [SerializeField] private float timeHack = 3.0f;
        [SerializeField] [Range(1f, 15f)] private float radius = 15.0f;
        [SerializeField] private LayerMask includeLayer;
        [SerializeField] private int searchSize = 15;
        [SerializeField] private float timeInScene;

        // Variables ocultas desde el inspecto de Unity
        private Collider[] detectedColliders;
        private ParticleSystem particleNullPulse;


        // Metodo de llamada de Unity, se llama una unica vez al iniciar el app, es el primer
        // metodo en ejecutarse, se realiza la asignacion de componentes
        private void Awake(){
            particleNullPulse = GetComponent<ParticleSystem>();
            detectedColliders = new Collider[searchSize];
            timeInScene = particleNullPulse.main.duration;
        }


        // Metodo de llamada de Unity, se llama una unica vez al iniciar el app se llama despues de
        // Awake, se realiza la configuracion previa al inicio de la lógica del juego
        // private void Start() {
        // }


        // Metodo de llamada de Unity, se llama una unica vez al iniciar el app se llama despues de
        // Awake y antes de Start, se realiza la activacion de la habilidad
        private void OnEnable() {
            StartCoroutine(ActiveRedCancellation());
        }


        // Se emplea este metodo para poder Activar la habilidad de hackeo
        private IEnumerator ActiveRedCancellation() {
            yield return new WaitForSeconds(0.1f);
            if(particleNullPulse != null) particleNullPulse.Play();
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
        [SerializeField] private Color colorSkill = Color.cyan;
        private void OnDrawGizmos() {
            Gizmos.color = colorSkill;
            Gizmos.DrawSphere(transform.position, radius);
        }
    #endif
    }
}