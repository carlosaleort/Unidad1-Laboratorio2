using System.Collections;
using UnityEngine;

namespace Assets.JSGAONA.Unidad2.Scripts {

    // Componentes requeridos para el funcionamiento del script
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    public class Bullet : MonoBehaviour {

        [SerializeField] private float timeInScene = 3.0f;
        private Rigidbody rb;


        private void Awake() {
            rb = GetComponent<Rigidbody>();
        }



        private void OnEnable() {
            StartCoroutine(ReturPool());
        }


        public void Launch(Vector3 direction){
            rb.AddForce(direction, ForceMode.Impulse);
        }


        private IEnumerator ReturPool(){
            yield return new WaitForSeconds(timeInScene);
            if(Pool.PoolInstance != null) Pool.PoolInstance.ReturnBullet(this);
        }


        private void OnCollisionEnter(Collision collision) {
            rb.velocity = Vector3.zero;
            // Se valida que el impacto sea un jugador
            if(collision.gameObject.CompareTag("Player")) {
                PlayerCombatController player = collision.gameObject.GetComponent<PlayerCombatController>();
                player.TakeDamage(10);
            }else{
                PoolParticle.PoolInstanceParticle.SpawnImpact(transform.localPosition);
            }
        }
    }
}
