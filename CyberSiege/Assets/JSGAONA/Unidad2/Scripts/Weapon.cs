using UnityEngine;

namespace Assets.JSGAONA.Unidad2.Scripts {

    // Se emplea este script para gestionar las armas dentro del juego
    public class Weapon : MonoBehaviour {

        // Variables visibles desde el inspector de Unity
        [SerializeField] private Transform cannon;
        [SerializeField] private float shotForce = 25f;
        [SerializeField] private ParticleSystem fire;
        [SerializeField] private AudioSource audioSourcefire;
        
        
        // Metodo que permite que el arma se dispare
        public void Fire(){
            if(cannon != null){
                Pool.PoolInstance.SpawnBullet(cannon.position, cannon.forward * shotForce);
                if(fire != null) fire.Play();
                if(audioSourcefire != null) audioSourcefire.Play();
            }
        }
    }
}