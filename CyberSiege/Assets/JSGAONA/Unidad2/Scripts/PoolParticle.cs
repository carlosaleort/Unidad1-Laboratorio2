using UnityEngine;
using UnityEngine.Pool;

namespace Assets.JSGAONA.Unidad2.Scripts {

    public class PoolParticle : MonoBehaviour {
    // Se emplea el patron Singleton, para permitir una unica instancia del administrador
        public static PoolParticle PoolInstanceParticle { get; private set; }

        [SerializeField] private ParticleSystem prefabParticle;
        [SerializeField] private int preloadCount = 20;
        [SerializeField] private int maxSizeCount = 50;

        private ObjectPool<ParticleSystem> pool;

        private void Awake() {
            // Asegura que solo haya una instancia de esta clase 'Patron de disenio Singleton'
            if(PoolInstanceParticle == null) {
                PoolInstanceParticle = this;
                DontDestroyOnLoad(gameObject);

                pool = new ObjectPool<ParticleSystem>(
                    CreateImpact,           // Cómo se crean los bullet  (Crea e instancia un bullet)
                    OnTakeFromPool,         // Que hacer cuando se obtiene del pool (reactiva o inicia)
                    OnReturnedToPool,       // Que hacer cuando se devuelve (limpia y desactiva)
                    OnDestroyImpact,        // Que hacer si se destruye, si se exede el maxSize (se destruye)
                    collectionCheck: false, // Valida que no libere dos veces (poner en false en producion)
                    defaultCapacity: preloadCount, // Al iniciar (capacidad inicial reservada)
                    maxSize: maxSizeCount   // Al liberar objetos al pool (Limita el tamnaño maximo del pool)
                );

                // Opcional: precalentar el pool
                for (int i = 0; i < preloadCount; i++) {
                    ParticleSystem impact = pool.Get();
                    pool.Release(impact);
                }

            }else{
                Destroy(gameObject);
            }
        }

        
        // Metodo que se emplea para crear un nuevo efecto de impacto y agregar al pool
        private ParticleSystem CreateImpact() {
            ParticleSystem impact = Instantiate(prefabParticle, transform);
            return impact;
        }

        
        // Metodo que permite obtener del pool el efecto de impacto
        private void OnTakeFromPool(ParticleSystem bullet) {
            bullet.gameObject.SetActive(true);
        }


        // Cuando el efecto de impacto regresa al pool
        private void OnReturnedToPool(ParticleSystem bullet) {
            bullet.gameObject.SetActive(false);
            bullet.transform.position = Vector3.zero;
        }


        // Metodo que se llama cuando se exede el tamanio del pool
        private void OnDestroyImpact(ParticleSystem bullet) {
            Destroy(bullet.gameObject);
        }


        public void ReturnImpact(ParticleSystem bullet) {
            pool.Release(bullet);
        }

        
        // Permite 
        public void SpawnImpact(Vector3 position) {
            ParticleSystem impact = pool.Get();
            impact.transform.position = position;
        }
    }
}