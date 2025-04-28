using UnityEngine;
using UnityEngine.Pool;

namespace Assets.JSGAONA.Unidad2.Scripts {

    public class Pool : MonoBehaviour {
        
        // Se emplea el patron Singleton, para permitir una unica instancia del administrador
        public static Pool PoolInstance { get; private set; }

        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private int preloadCount = 20;
        [SerializeField] private int maxSizeCount = 50;

        private ObjectPool<Bullet> pool;

        private void Awake() {
            // Asegura que solo haya una instancia de esta clase 'Patron de disenio Singleton'
            if(PoolInstance == null) {
                PoolInstance = this;
                DontDestroyOnLoad(gameObject);

                pool = new ObjectPool<Bullet>(
                    CreateBullet,           // Cómo se crean los bullet  (Crea e instancia un bullet)
                    OnTakeFromPool,         // Que hacer cuando se obtiene del pool (reactiva o inicia)
                    OnReturnedToPool,       // Que hacer cuando se devuelve (limpia y desactiva)
                    OnDestroyBullet,        // Que hacer si se destruye, si se exede el maxSize (se destruye)
                    collectionCheck: false, // Valida que no libere dos veces (poner en false en producion)
                    defaultCapacity: preloadCount, // Al iniciar (capacidad inicial reservada)
                    maxSize: maxSizeCount   // Al liberar objetos al pool (Limita el tamnaño maximo del pool)
                );

                // Opcional: precalentar el pool
                for (int i = 0; i < preloadCount; i++) {
                    Bullet bullet = pool.Get();
                    pool.Release(bullet);
                }

            }else{
                Destroy(gameObject);
            }
        }

        
        // Metodo que se emplea para crear un nuevo Bullet y agregar al pool
        private Bullet CreateBullet() {
            Bullet bullet = Instantiate(bulletPrefab, transform);
            return bullet;
        }

        
        // Metodo que permite obtener del pool el Bullet
        private void OnTakeFromPool(Bullet bullet) {
            bullet.gameObject.SetActive(true);
        }


        // Cuando el bullet regresa al pool
        private void OnReturnedToPool(Bullet bullet) {
            bullet.gameObject.SetActive(false);
            bullet.transform.position = Vector3.zero;
        }


        // Metodo que se llama cuando se exede el tamanio del pool
        private void OnDestroyBullet(Bullet bullet) {
            Destroy(bullet.gameObject);
        }


        public void ReturnBullet(Bullet bullet) {
            pool.Release(bullet);
        }

        
        // Permite 
        public void SpawnBullet(Vector3 position, Vector3 direction) {
            Bullet bullet = pool.Get();
            bullet.transform.position = position;
            bullet.transform.rotation = Quaternion.LookRotation(direction);
            bullet.Launch(direction);
        }
    }
}