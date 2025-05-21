using UnityEngine;
using System.Collections; // Required for Coroutine

namespace Assets.JSGAONA.Unidad2.Scripts
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Transform cannon;
        [SerializeField] private float shotForce = 25f;
        [SerializeField] private ParticleSystem fire;
        [SerializeField] private AudioSource audioSourcefire;
        [SerializeField] private float firstShotDelay = 2f; // Delay before first shot

        private bool firstShotFired = false;

        // Método que permite que el arma se dispare con retraso en el primer disparo
        public void Fire()
        {
            if (!firstShotFired)
            {
                firstShotFired = true;
                StartCoroutine(DelayedFire());
            }
            else
            {
                ExecuteFire();
            }
        }

        IEnumerator DelayedFire()
        {
            yield return new WaitForSeconds(firstShotDelay);
            ExecuteFire();
        }

        void ExecuteFire()
        {
            if (cannon != null)
            {
                Pool.PoolInstance.SpawnBullet(cannon.position, cannon.forward * shotForce);
                if (fire != null) fire.Play();
                if (audioSourcefire != null) audioSourcefire.Play();
            }
        }
    }
}